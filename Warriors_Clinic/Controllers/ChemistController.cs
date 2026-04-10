using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;

namespace Warriors_Clinic.Controllers
{
    public class ChemistController : Controller
    {
        private readonly AppDbContext _context;

        public ChemistController(AppDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // 🔹 VIEW DRUG REQUESTS
        public IActionResult DrugRequests()
        {
            var requests = _context.DrugRequests
                .Include(d => d.Physician)
                .ToList();

            return View(requests);
        }

        public IActionResult CreatePO()
        {
            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.Drugs = _context.Drugs.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreatePO(int supplierId, List<int> drugIds, List<int> quantities)
        {
            // Create Header
            var header = new PurchaseOrderHeader
            {
                Podate = DateTime.Now,
                SupplierId = supplierId
            };

            _context.PurchaseOrderHeaders.Add(header);
            _context.SaveChanges(); // important to get POId

            // Create Lines
            for (int i = 0; i < drugIds.Count; i++)
            {
                if (quantities[i] > 0)
                {
                    var line = new PurchaseOrderLine
                    {
                        Poid = header.Poid,
                        DrugId = drugIds[i],
                        Quantity = quantities[i]
                    };

                    _context.PurchaseOrderLines.Add(line);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}