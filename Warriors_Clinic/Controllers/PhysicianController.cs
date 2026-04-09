using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;

namespace Warriors_Clinic.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly AppDbContext _context;

        public PhysicianController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Physician")
            {
                return RedirectToAction("Login", "Account");
            }

            var email = HttpContext.Session.GetString("UserName");

            var physician = _context.Physicians
                .FirstOrDefault(p => p.Email == email);

            ViewBag.Name = physician != null ? physician.Name : "Doctor";

            return View();
        }

        public IActionResult Appointments()
        {
            var email = HttpContext.Session.GetString("UserName");

            var physician = _context.Physicians
                .FirstOrDefault(p => p.Email == email);

            var data = _context.Appointments
                .Where(a => a.PhysicianId == physician.PhysicianId
                            && a.ScheduleStatus == "Approved")
                .Include(a => a.Patient)   // ✅ VERY IMPORTANT
                .ToList();

            return View(data);
        }
        [HttpPost]
        public IActionResult MarkConsulted(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment != null)
            {
                appointment.ScheduleStatus = "Consulted";
                _context.SaveChanges();
            }

            return RedirectToAction("Appointments");
        }

        public IActionResult Prescriptions()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult DrugRequests()
        {
            return View();
        }
    }
}
