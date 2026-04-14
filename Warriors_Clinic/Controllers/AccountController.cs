using Microsoft.AspNetCore.Mvc;
using Warriors_Clinic.Models;
using System.Linq;

namespace Warriors_Clinic.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        //REGISTER
        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }

        // ✅ REGISTER
        [HttpPost]
        public IActionResult Register(User user, string DOB, string Gender, string Address, string Phone, string Summary)
        {
            user.Role = "Patient";
            user.IsApproved = false;

            var patient = new Patient
            {
                Name = user.UserName,
                DOB = DateTime.Parse(DOB),
                Gender = Gender,
                Address = Address,
                Phone = Phone,
                Email = user.Email,
                Summary = string.IsNullOrEmpty(Summary) ? "No summary provided" : Summary
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            user.ReferenceToId = patient.PatientId;

            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.Success = "Thank you for registering! Wait for Admin Approval.";

            return View();
        }

        // ✅ LOGIN (GET)
        public IActionResult Login()
        {
            return View();
        }

        // ✅ LOGIN (POST)
        [HttpPost]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                // ✅ STORE SESSION CORRECTLY
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                // ✅ PATIENT MAPPING
                if (user.Role == "Patient")
                {
                    var patient = _context.Patients
                        .FirstOrDefault(p => p.Email == user.Email);

                    if (patient == null)
                        return Content("Patient record not found");

                    HttpContext.Session.SetInt32("PatientId", patient.PatientId);
                }

                // ✅ REDIRECT
                switch (user.Role)
                {
                    case "Admin":
                        return RedirectToAction("Dashboard", "Admin");

                    case "Patient":
                        return RedirectToAction("Dashboard", "Patient");

                    case "Physician":
                        return RedirectToAction("Dashboard", "Physician");

                    case "Chemist":
                        return RedirectToAction("Dashboard", "Chemist");

                    case "Supplier":
                        return RedirectToAction("Dashboard", "Supplier");

                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Invalid login";
            return View();
        }


        // ✅ LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}