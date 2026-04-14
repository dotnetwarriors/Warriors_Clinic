using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;
using Warriors_Clinic.Models.ViewModels;

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
            var userName = HttpContext.Session.GetString("UserEmail");

            var user = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            var chemist = _context.Chemists
                .FirstOrDefault(c => c.ChemistId == user.ReferenceToId);

            if (chemist == null)
            {
                return Content("Chemist not found"); // debug safety
            }

            var requests = _context.DrugRequests
                .Include(r => r.Chemist)
                .Include(r => r.Physician) // ✅ ADD THIS
                .AsNoTracking()
                .Where(r => r.ChemistId == chemist.ChemistId
                            && r.IsDeletedByPhysician == false)
                .ToList();

            return View(requests);
        }

        public IActionResult UpdateStatus(int id, string status)
        {
            var request = _context.DrugRequests.Find(id);

            if (request != null)
            {
                request.RequestStatus = status;
                _context.SaveChanges();
            }

            return RedirectToAction("DrugRequests");
        }
        // ✅ APPROVE REQUEST
        public IActionResult ApproveRequest(int id)
        {
            var request = _context.DrugRequests.Find(id);

            if (request != null)
            {
                request.RequestStatus = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("DrugRequests");
        }


        // ❌ REJECT REQUEST
        public IActionResult RejectRequest(int id)
        {
            var request = _context.DrugRequests.Find(id);

            if (request != null)
            {
                request.RequestStatus = "Rejected";
                _context.SaveChanges();
            }

            return RedirectToAction("DrugRequests");
        }

        //==================Purchase order =============================

        public IActionResult POHome()
        {
            return View();
        }

        public IActionResult POList()
        {
            var pos = _context.PurchaseOrderHeaders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
                .Where(p => p.Status != "Deleted") // ✅ soft delete
                .OrderByDescending(p => p.Poid)
                .ToList();

            return View(pos);
        }


        public IActionResult CreatePO()
        {
            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.Drugs = _context.Drugs.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult CreatePO(PurchaseOrderVM model)
        {
            if (model == null || model.Lines.Count == 0)
                return View(model);

            // HEADER
            var header = new PurchaseOrderHeader
            {
                SupplierId = model.SupplierId,
                Podate = DateTime.Now,
                Status = "Pending"
            };

            _context.PurchaseOrderHeaders.Add(header);
            _context.SaveChanges();

            // LINES
            foreach (var line in model.Lines)
            {
                var poLine = new PurchaseOrderLine
                {
                    Poid = header.Poid,
                    DrugId = line.DrugId,
                    Quantity = line.Quantity,
                    Note = line.Note
                };

                _context.PurchaseOrderLines.Add(poLine);
            }

            _context.SaveChanges();

            return RedirectToAction("POList");
        }
        public IActionResult ViewPO()
        {
            var pos = _context.PurchaseOrderHeaders
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
                .OrderByDescending(p => p.Poid)
                .ToList();

            return View(ViewPO);

        }
        public IActionResult DeletePO(int id)
        {
            var po = _context.PurchaseOrderHeaders.Find(id);

            if (po != null)
            {
                po.Status = "Deleted"; // ✅ NOT removing from DB
                _context.SaveChanges();
            }

            return RedirectToAction("POList");
        }
    }
}