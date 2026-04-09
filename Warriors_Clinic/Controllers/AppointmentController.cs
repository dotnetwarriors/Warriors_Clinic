using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;

namespace Warriors_Clinic.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ PATIENT: View only their appointments
        public IActionResult MyAppointments()
        {
            var userName = HttpContext.Session.GetString("UserName");

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            var appointments = _context.Appointments
                .Where(a => a.PatientId == user.ReferenceToId)
                .Include(a => a.Physician)
                .ToList();

            return View(appointments);
        }
        public IActionResult ViewSchedule()
        {
            var userName = HttpContext.Session.GetString("UserName");

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            var approvedAppointments = _context.Appointments
                .Where(a => a.PatientId == user.ReferenceToId
                         && a.ScheduleStatus == "Approved")
                .Include(a => a.Physician)
                .ToList();

            return View(approvedAppointments);
        }

        // ✅ GET: Appointment/Create
        public IActionResult Create()
        {
            ViewData["PhysicianId"] = new SelectList(_context.Physicians, "PhysicianId", "Name");
            return View();
        }

        // ✅ POST: Appointment/Create (ONLY THIS ONE)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var userName = HttpContext.Session.GetString("UserName");

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);

            if (user != null)
            {
                // ✅ USE CORRECT PROPERTY NAME
                appointment.PatientId = user.ReferenceToId;
            }

            appointment.ScheduleStatus = "Pending";

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyAppointments"); // 👈 important
            }

            ViewData["PhysicianId"] = new SelectList(_context.Physicians, "PhysicianId", "Name", appointment.PhysicianId);
            return View(appointment);
        }
    }
    
}