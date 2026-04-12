using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;

namespace Warriors_Clinic.Controllers
{
    public class SupplierController : Controller
    {
        private readonly AppDbContext _context;

        public SupplierController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var totalPO = _context.PurchaseOrderHeaders.Count();
            var pending = _context.PurchaseOrderHeaders.Count(p => p.Status == "Pending");
            var approved = _context.PurchaseOrderHeaders.Count(p => p.Status == "Approved");
            var rejected = _context.PurchaseOrderHeaders.Count(p => p.Status == "Rejected");

            ViewBag.TotalPO = totalPO;
            ViewBag.Pending = pending;
            ViewBag.Approved = approved;
            ViewBag.Rejected = rejected;

            return View();
        }

        // VIEW ALL PURCHASE ORDERS
        public IActionResult ViewPO()
        {
            var userName = HttpContext.Session.GetString("UserName");

            var supplier = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            var pos = _context.PurchaseOrderHeaders
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
                .Include(p => p.Supplier)
                .Where(p => p.SupplierId == supplier.ReferenceToId) // ✅ IMPORTANT
                .ToList();

            return View(pos);
        }


        // ACCEPT PO
        public IActionResult ApprovePO(int id)
        {
            var po = _context.PurchaseOrderHeaders.Find(id);

            if (po != null)
            {
                po.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("ViewPO");
        }

        // REJECT PO
        public IActionResult RejectPO(int id)
        {
            var po = _context.PurchaseOrderHeaders.Find(id);

            if (po != null)
            {
                po.Status = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("ViewPO");
        }
    }
}