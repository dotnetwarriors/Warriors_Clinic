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

        // ================= DASHBOARD =================
        public IActionResult Dashboard()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            var supplier = _context.Users
                .FirstOrDefault(u => u.Email == userEmail);

            if (supplier == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var supplierId = supplier.ReferenceToId;

            // ✅ FILTERED COUNTS  
            var totalPO = _context.PurchaseOrderHeaders
                .Count(p => p.SupplierId == supplierId && p.IsVisible == true);

            var pending = _context.PurchaseOrderHeaders
                .Count(p => p.SupplierId == supplierId && p.Status == "Pending" && p.IsVisible == true);

            var approved = _context.PurchaseOrderHeaders
                .Count(p => p.SupplierId == supplierId && p.Status == "Approved" && p.IsVisible == true);

            var rejected = _context.PurchaseOrderHeaders
                .Count(p => p.SupplierId == supplierId && p.Status == "Rejected" && p.IsVisible == true);

            ViewBag.TotalPO = totalPO;
            ViewBag.Pending = pending;
            ViewBag.Approved = approved;
            ViewBag.Rejected = rejected;

            return View();

        }


        // ================= VIEW PENDING PURCHASE ORDERS =================
        public IActionResult ViewPO()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var supplier = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (supplier == null)
                return RedirectToAction("Login", "Account");

            var pos = _context.PurchaseOrderHeaders
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
                .Include(p => p.Supplier)
                .Where(p =>
                    p.SupplierId == supplier.ReferenceToId &&
                    p.IsVisible == true)
                .AsEnumerable() // 🔥 Important
                .DistinctBy(p => p.Poid) // 🔥 Remove duplicates
                .OrderByDescending(p => p.Podate)
                .ToList();

            return View(pos);
        }




        // ================= ACCEPTED ORDERS =================
        public IActionResult AcceptedOrders()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var supplier = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (supplier == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.PurchaseOrderHeaders
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
               
                .Where(p => p.SupplierId == supplier.ReferenceToId &&
                            p.Status == "Approved" &&
                            p.IsVisible == true)
                .ToList();

            return View(orders);
        }

        // ================= REJECTED ORDERS =================
        public IActionResult RejectedOrders()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Account");

            var supplier = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (supplier == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.PurchaseOrderHeaders
                .Include(p => p.PurchaseOrderLines)
                    .ThenInclude(l => l.Drug)
               
                .Where(p => p.SupplierId == supplier.ReferenceToId &&
                            p.Status == "Rejected" &&
                            p.IsVisible == true)
                .ToList();

            return View(orders);
        }

        // ================= HIDE (UI DELETE ONLY) =================
        public IActionResult Hide(int id)
        {
            var po = _context.PurchaseOrderHeaders.Find(id);

            if (po != null)
            {
                po.IsVisible = false;
                _context.SaveChanges();
            }

            return RedirectToAction("AcceptedOrders");
        }


        [HttpPost]
        public IActionResult AddNote(int poId, string note)
        {
            if (string.IsNullOrEmpty(note))
                return RedirectToAction("ViewPO");

            var po = _context.PurchaseOrderHeaders
                .FirstOrDefault(p => p.Poid == poId);

            if (po != null)
            {
                po.SupplierNote = note;
                _context.SaveChanges();
            }

            return RedirectToAction("ViewPO");
        }
    }
}
