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
   
    public async Task<IActionResult> Create(Appointment appointment)
    {
        // ✅ Get logged-in patient
        var patientId = HttpContext.Session.GetInt32("PatientId");

        if (patientId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // ✅ Assign patient automatically
        appointment.PatientId = patientId.Value;

        // ❌ Remove doctor selection (Admin will assign)
        appointment.PhysicianId = null; // make sure DB allows NULL

        // ✅ Default status
        appointment.ScheduleStatus = "Pending";

        // ✅ Prevent past date booking
        if (appointment.AppointmentDateTime < DateTime.Now)
        {
            ModelState.AddModelError("", "Past date/time is not allowed.");
        }

        // ✅ Final save
        if (ModelState.IsValid)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAppointments");
        }
        if (string.IsNullOrEmpty(appointment.Criticality))
        {
            appointment.Criticality = "Routine";
        }

        return View(appointment);
    }

}