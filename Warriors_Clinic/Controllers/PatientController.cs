using Microsoft.AspNetCore.Mvc;
using Warriors_Clinic.Models;
using System.Linq;

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
            var userName = HttpContext.Session.GetString("UserName");

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            var patient = _context.Patients
                .FirstOrDefault(p => p.PatientId == user.ReferenceToId);

            ViewBag.Name = patient?.Name;

            return View();
        }
    }
}