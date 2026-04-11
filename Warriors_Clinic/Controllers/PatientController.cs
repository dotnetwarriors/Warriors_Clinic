using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Warriors_Clinic.Models;

namespace Warriors_Clinic.Controllers
{
    public class PatientController : Controller
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            // ✅ Get session email
            var email = HttpContext.Session.GetString("UserName");

            // ✅ FIX 1: Session check (MANDATORY)
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            // ✅ FIX 2: Safe query
            var patient = _context.Patients
                .FirstOrDefault(p => p.Email != null && p.Email == email);

            // ✅ FIX 3: Patient null check
            if (patient == null)
            {
                return Content("Patient not found. Check DB email mapping.");
            }

            return View(patient);
        }

        public IActionResult ViewPrescription()
        {
            var email = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.Email == email);

            if (patient == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var data = _context.PhysicianAdvices
                .Where(p => p.PatientId == patient.PatientId)
                .Include(p => p.PhysicianPrescriptions)
                    .ThenInclude(pp => pp.Drug)
                .ToList();

            return View(data);
        }

    }
}
