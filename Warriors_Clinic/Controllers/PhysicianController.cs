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
                     && ( a.ScheduleStatus == "Approved" || a.ScheduleStatus == "Consulted")
                     && a.IsVisibleToDoctor == true)        
                .Include(a => a.Patient)   // ✅ VERY IMPORTANT
                .ToList();

            return View(data);
        }

        public IActionResult Consulted(int id)
        {
            var appointment = _context.Appointments
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // ✅ Prevent multiple clicks
            if (appointment.ScheduleStatus == "Consulted")
            {
                return RedirectToAction("Appointments");
            }

            // ✅ Update Appointment table (MAIN FIX)
            appointment.ScheduleStatus = "Consulted";

            // ✅ Force EF to detect change
            _context.Entry(appointment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            // ✅ Check if already exists in Schedule table
            var existingSchedule = _context.Schedule
                .FirstOrDefault(s => s.AppointmentId == id);

            if (existingSchedule == null)
            {
                var schedule = new Schedule
                {
                    AppointmentId = appointment.AppointmentId,
                    ScheduleDate = DateTime.Now,
                    ScheduleStatus = ScheduleStatusEnum.Consulted
                };

                _context.Schedule.Add(schedule);
            }

            _context.SaveChanges();

            return RedirectToAction("Appointments");
        }

        public IActionResult HideFromDashboard(int id)
        {
            var appointment = _context.Appointments
                .FirstOrDefault(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.IsVisibleToDoctor = false;

            _context.Entry(appointment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();

            return RedirectToAction("Appointments");
        }

        public IActionResult Prescriptions()
        {
            var data = _context.Schedule
                .Include(s => s.Appointment)
                .ThenInclude(a => a.Patient)
                .Where(s => (s.ScheduleStatus == ScheduleStatusEnum.Consulted
                          || s.ScheduleStatus == ScheduleStatusEnum.Added)
                         && s.Appointment.IsVisibleToDoctor == true)
                .ToList();

            return View(data);
        }

        public IActionResult AddPrescription(int id)
        {
            var schedule = _context.Schedule
                .Include(s => s.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null)
                return NotFound();

            // ✅ IMPORTANT
            ViewBag.AppointmentId = schedule.Appointment.AppointmentId;
            ViewBag.PatientId = schedule.Appointment.PatientId;

            ViewBag.Drugs = _context.Drugs.ToList();

            return View(schedule);
        }
        [HttpPost]
        public IActionResult AddPrescription(
            int patientId,
            int appointmentId, // ✅ ADD THIS (IMPORTANT)
            string advice,
            List<int> Drug,
            List<string> Dosage,
            List<string> Timing,
            List<string> Duration)
        {
            // ✅ STEP 1: Check appointment exists
            var appointment = _context.Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            // ✅ STEP 2: Prevent duplicate prescription
            if (appointment.ScheduleStatus == "Added")
            {
                return RedirectToAction("Prescriptions");
            }

            // ✅ STEP 3: Save Advice
            var adviceEntry = new PhysicianAdvice
            {
                PatientId = patientId,
                Advice = advice
            };

            _context.PhysicianAdvices.Add(adviceEntry);
            _context.SaveChanges(); // needed to get AdviceId

            // ✅ STEP 4: Save Prescription (Multiple drugs)
            for (int i = 0; i < Drug.Count; i++)
            {
                if (Drug[i] != 0)
                {
                    var prescription = new PhysicianPrescription
                    {
                        PhysicianAdviceId = adviceEntry.PhysicianAdviceId,
                        DrugId = Drug[i],
                        Dosage = Dosage[i],
                        Timing = Timing[i],
                        Duration = Duration[i]
                    };

                    _context.PhysicianPrescriptions.Add(prescription);
                }
            }

            // ✅ STEP 5: UPDATE STATUS → MAIN REQUIREMENT
            appointment.ScheduleStatus = ScheduleStatusEnum.Added.ToString();

            // ✅ ALSO UPDATE SCHEDULE TABLE (CRITICAL FIX)
            var schedule = _context.Schedule
                .FirstOrDefault(s => s.AppointmentId == appointmentId);

            if (schedule != null)
            {
                schedule.ScheduleStatus = ScheduleStatusEnum.Added;

                _context.Entry(schedule).State =
                    Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            _context.Entry(appointment).State =
                Microsoft.EntityFrameworkCore.EntityState.Modified;

            // ✅ STEP 6: SAVE ALL
            _context.SaveChanges();

            // ✅ STEP 7: REDIRECT BACK
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
        public IActionResult DeleteFromPrescription(int id)
        {
            var schedule = _context.Schedule
                .Include(s => s.Appointment)
                .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule != null)
            {
                // ✅ Hide from Prescription dashboard ONLY
                schedule.Appointment.IsVisibleToDoctor = false;

                _context.SaveChanges();
            }

            // ✅ Stay on SAME PAGE
            return RedirectToAction("Prescriptions");
        }

        public IActionResult DrugRequests()
        {
            var userName = HttpContext.Session.GetString("UserName");

            var physician = _context.Users
                .FirstOrDefault(u => u.UserName == userName);

            if (physician == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = _context.DrugRequests
                .Include(r => r.Chemist) // ✅ FIXED
                .Where(r => r.PhysicianId == physician.ReferenceToId
                         && r.IsDeletedByPhysician == false)
                .ToList();

            ViewBag.Chemists = _context.Chemists.ToList();

            return View(requests);
        }

        [HttpPost]
        public IActionResult CreateDrugRequest(string DrugInfoText, int ChemistId)
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
                DrugInfoText = string.IsNullOrEmpty(DrugInfoText) ? "Unknown Drug" : DrugInfoText,
                ChemistId = ChemistId, // ✅ FIXED FORMAT
                RequestDate = DateTime.Now,
                RequestStatus = "Pending",
                IsDeletedByPhysician = false // ✅ GOOD PRACTICE
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
                req.IsDeletedByPhysician = true; // ✅ SOFT DELETE
                _context.SaveChanges();
            }

            return RedirectToAction("DrugRequests"); // ✅ stay on same page
        }


        public IActionResult Chat()
        {
            return View();
        }
    }
}
