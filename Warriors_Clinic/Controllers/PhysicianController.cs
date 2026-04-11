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
        
        public IActionResult Consulted(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment != null)
            {
                
                var schedule = new Schedule
                {
                    AppointmentId = appointment.AppointmentId,
                    ScheduleDate = DateTime.Now,
                    ScheduleStatus = ScheduleStatusEnum.Consulted
                };

                _context.Schedule.Add(schedule);
                _context.SaveChanges();
            }

            return RedirectToAction("Appointments"); // stay here
        }

        public IActionResult Prescriptions()
        {
            var data = _context.Schedule
                .Include(s => s.Appointment)
                    .ThenInclude(a => a.Patient)
                .Where(s => s.ScheduleStatus == ScheduleStatusEnum.Consulted)
                .ToList();

            return View(data);
        }

       public IActionResult AddPrescription(int id)
{
    var schedule = _context.Schedule
        .Include(s => s.Appointment)
        .ThenInclude(a => a.Patient)
        .FirstOrDefault(s => s.ScheduleId == id);
 
    // ✅ ADD THIS (VERY IMPORTANT)
    ViewBag.Drugs = _context.Drugs.ToList();
 
    // ✅ ALSO FIX THIS (important for POST)
    ViewBag.PatientId = schedule?.Appointment?.PatientId;
 
    return View(schedule);
}

        [HttpPost]
        public IActionResult AddPrescription(
            int patientId,
            string advice,
            List<int> Drug,          // ✅ NOW using DrugId
            List<string> Dosage,
            List<string> Timing,
            List<string> Duration)
        {
            // Save Advice
            var adviceEntry = new PhysicianAdvice
            {
                PatientId = patientId,
                Advice = advice
            };

            _context.PhysicianAdvices.Add(adviceEntry);
            _context.SaveChanges();

            // Save Prescription
            for (int i = 0; i < Drug.Count; i++)
            {
                if (Drug[i] != 0) // skip empty selection
                {
                    var prescription = new PhysicianPrescription
                    {
                        PhysicianAdviceId = adviceEntry.PhysicianAdviceId,
                        DrugId = Drug[i],   // ✅ DIRECT SAVE (NO LOOKUP)
                        Dosage = Dosage[i],
                        Timing = Timing[i],
                        Duration = Duration[i]
                    };

                    _context.PhysicianPrescriptions.Add(prescription);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Prescriptions");
        }


        public IActionResult SendToPatient(int id)
        {
            var schedule = _context.Schedule.Find(id);

            if (schedule != null)
            {
                schedule.IsSent = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Appointment");
        }

        public IActionResult DrugRequests()
        {
            var userName = HttpContext.Session.GetString("UserName");

            var physician = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            var requests = _context.DrugRequests
            .Where(r => r.PhysicianId == physician.ReferenceToId
              && r.IsDeletedByPhysician == false)
             .ToList();
            return View(requests);
        }

        [HttpPost]
        public IActionResult CreateDrugRequest(string DrugInfoText)
        {
            var userName = HttpContext.Session.GetString("UserName");

            var physician = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            if (physician == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var request = new DrugRequest
            {
                PhysicianId = physician.ReferenceToId,
                DrugInfoText = DrugInfoText,
                RequestDate = DateTime.Now,
                RequestStatus = "Pending"
            };

            _context.DrugRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("DrugRequests");
        }
        public IActionResult DeleteDrugRequest(int id)
        {
            var req = _context.DrugRequests.Find(id);

            if (req != null)
            {
                req.IsDeletedByPhysician = true; // 👈 SOFT DELETE
                _context.SaveChanges();
            }

            return RedirectToAction("DrugRequests");
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
