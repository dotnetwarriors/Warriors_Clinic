using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.NetworkInformation;
using Warriors_Clinic.Models;
using Microsoft.EntityFrameworkCore;

public class AppointmentController : Controller
{
    private readonly AppDbContext _context;

    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ PATIENT: View their appointments
    public IActionResult MyAppointments()
    {
        var patientId = HttpContext.Session.GetInt32("PatientId");

        if (patientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var appointments = _context.Appointments
            .Include(a => a.Physician)
            .Where(a => a.PatientId == patientId)
            .ToList();

        return View(appointments);
    }

    // ✅ PATIENT: View approved schedule
    public IActionResult ViewSchedule()
    {
        var patientId = HttpContext.Session.GetInt32("PatientId");

        if (patientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var approvedAppointments = _context.Appointments
            .Include(a => a.Physician)
            .Where(a => a.PatientId == patientId &&
                        a.ScheduleStatus == "Approved")
            .ToList();

        return View(approvedAppointments);
    }

    // ✅ GET: Appointment/Create
    public IActionResult Create()
    {
        ViewData["PhysicianId"] = new SelectList(_context.Physicians, "PhysicianId", "Name");
        return View();
    }

    // ✅ POST: Appointment/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Appointment appointment)
    {
        var patientId = HttpContext.Session.GetInt32("PatientId");

        if (patientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // ✅ CORRECT LINKING
        appointment.PatientId = patientId.Value;

        appointment.ScheduleStatus = "Pending";

        if (ModelState.IsValid)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAppointments");
        }

        ViewData["PhysicianId"] = new SelectList(_context.Physicians, "PhysicianId", "Name", appointment.PhysicianId);
        return View(appointment);
    }
}