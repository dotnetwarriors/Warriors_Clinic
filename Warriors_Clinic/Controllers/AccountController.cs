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
                Summary = string.IsNullOrEmpty(Summary)?"No summary provided":Summary
            };

            _context.Patients.Add(patient);
            _context.SaveChanges();

            user.ReferenceToId = patient.PatientId;

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ✅ LOGIN (GET)
        public IActionResult Login()
        {
            return View();
        }

        // ✅ LOGIN (POST)
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                    return RedirectToAction("Dashboard", "Admin");


                if (user.Role == "Patient")
                    return RedirectToAction("Dashboard", "Patient");


                return RedirectToAction("Index", "Home");
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