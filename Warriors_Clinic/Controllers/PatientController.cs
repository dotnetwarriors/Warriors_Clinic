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
            var email = HttpContext.Session.GetString("UserEmail");

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
            var email = HttpContext.Session.GetString("UserEmail");

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
                .AsEnumerable() // 🔥 important
                .Where(p => p.PhysicianPrescriptions != null)
                .ToList();
            return View(data);
        }

        public IActionResult Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return Content("User not found");
            }

            if (user.ReferenceToId == null)
            {
                return Content("User not linked to Patient");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == user.ReferenceToId);

            if (patient == null)
            {
                return Content("Patient record not found");
            }

            return View(patient);
        }

        public IActionResult EditProfile()
        {
            var email = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return Content("User not found");
            }

            if (user.ReferenceToId == null)
            {
                return Content("User not linked to Patient");
            }

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == user.ReferenceToId);

            if (patient == null)
            {
                return Content("Patient not found");
            }

            return View(patient);
        }

        [HttpPost]
        public IActionResult Edit(Patient model, string NewPassword)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.PatientId == model.PatientId);

            if (patient == null)
                return NotFound();

            // ✅ Update patient details
            patient.Name = model.Name;
            patient.Email = model.Email;
            patient.Phone = model.Phone;
            patient.Address = model.Address;

            // ✅ FIXED: Get user using session email
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            // ✅ Update password
            if (!string.IsNullOrEmpty(NewPassword) && user != null)
            {
                user.Password = NewPassword;
            }

            _context.SaveChanges();

            return RedirectToAction("EditProfile");
        }
    }
}
