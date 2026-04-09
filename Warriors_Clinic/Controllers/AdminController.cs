using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult PendingUsers()
    {
        var users = _context.Users
            .Where(u => !u.IsApproved)
            .ToList();

        return View(users);
    }

    public IActionResult Approve(int id)
    {
        var user = _context.Users.Find(id);

        if (user != null)
        {
            user.IsApproved = true;
            _context.SaveChanges();
        }

        return RedirectToAction("PendingUsers");
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Workforce()
    {
        return View();
    }


    // ================== PHYSICIAN ==================
    public IActionResult PhysicianMenu()
    {
        return View();
    }
    // GET
    public IActionResult AddPhysician()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult AddPhysician(Physician physician)
    {
        if (ModelState.IsValid)
        {
            _context.Physicians.Add(physician);
            _context.SaveChanges();

            // create login
            var user = new User
            {
                UserName = physician.Name,
                Password = "1234",
                Role = "Physician",
                IsApproved = true,
                ReferenceToId = physician.PhysicianId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("PhysicianList");
        }

        return View(physician);
    }
    public IActionResult PhysicianList()
    {
        var list = _context.Physicians.ToList();
        return View(list);
    }
    public IActionResult EditPhysician(int id)
    {
        var data = _context.Physicians.Find(id);
        return View(data);
    }

    [HttpPost]
    [HttpPost]
    public IActionResult EditPhysician(Physician model)
    {
        if (ModelState.IsValid)
        {
            var existing = _context.Physicians.Find(model.PhysicianId);

            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Address = model.Address;
                existing.Specialization = model.Specialization;
                existing.Summary = model.Summary;

                _context.SaveChanges();
            }

            return RedirectToAction("PhysicianList");
        }

        return View(model);
    }


    //=====================Chemist=====================================
    public IActionResult ChemistMenu()
    {
        return View();
    }
    // GET
    public IActionResult AddChemist()
    {
        return View();
    }
    [HttpPost]
    public IActionResult AddChemist(Chemist model)
    {
        if (ModelState.IsValid)
        {
            _context.Chemists.Add(model);
            _context.SaveChanges();

            // Create login
            var user = new User
            {
                UserName = model.Email,
                Password = "123456",
                Role = "Chemist",
                IsApproved = true,
                ReferenceToId = model.ChemistId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("ChemistList");
        }

        return View(model);
    }
    public IActionResult ChemistList()
    {
        var data = _context.Chemists.ToList();
        return View(data);
    }
    public IActionResult EditChemist(int id)
    {
        var chemist = _context.Chemists.Find(id);
        return View(chemist);
    }
    [HttpPost]
    public IActionResult EditChemist(Chemist model)
    {
        if (ModelState.IsValid)
        {
            var existing = _context.Chemists.Find(model.ChemistId);

            if (existing != null)
            {
                existing.Name = model.Name;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Address = model.Address;
                existing.Summary = model.Summary;

                _context.SaveChanges();
            }

            return RedirectToAction("ChemistList");
        }

        return View(model);
    }
    public IActionResult DeleteChemist(int id)
    {
        var chemist = _context.Chemists.Find(id);

        if (chemist != null)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Chemist");

            if (user != null)
            {
                _context.Users.Remove(user);
            }

            _context.Chemists.Remove(chemist);
            _context.SaveChanges();
        }

        return RedirectToAction("ChemistList");
    }

    //==============================Supplier==================================

    public IActionResult SupplierMenu()
    {
        return View();
    }

    public IActionResult AddSupplier()
    {
        return View("AddSupplier");
    }

    [HttpPost]
    public IActionResult AddSupplier(Supplier model)
    {
        if (ModelState.IsValid)
        {
            _context.Suppliers.Add(model);
            _context.SaveChanges();

            var user = new User
            {
                UserName = model.Email,
                Password = "123456",
                Role = "Supplier",
                IsApproved = true,
                ReferenceToId = model.SupplierId
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("SupplierList");
        }

        return View(model);
    }

    public IActionResult SupplierList()
    {
        var data = _context.Suppliers.ToList();
        return View(data);
    }

    public IActionResult EditSupplier(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        return View(supplier);
    }
    [HttpPost]
public IActionResult EditSupplier(Supplier model)
{
    if (ModelState.IsValid)
    {
        var existing = _context.Suppliers.Find(model.SupplierId);
 
        if (existing != null)
        {
            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.Address = model.Address;
 
            _context.SaveChanges();
        }
 
        return RedirectToAction("SupplierList");
    }
 
    return View(model);
}
 
public IActionResult DeleteSupplier(int id)
{
    var supplier = _context.Suppliers.Find(id);
 
    if (supplier != null)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Supplier");
 
        if (user != null)
        {
            _context.Users.Remove(user);
        }
 
        _context.Suppliers.Remove(supplier);
        _context.SaveChanges();
    }
 
    return RedirectToAction("SupplierList");
}
 



    //Appointment requests
    public IActionResult AppointmentRequests()
    {
        var data = _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Physician)
        .ToList();

        return View(data);
    }

    // ✅ Approve
    public IActionResult ApproveAppointment(int id)
    {
        var appt = _context.Appointments.Find(id);

        if (appt == null)
            return NotFound();

        appt.ScheduleStatus = "Approved";
        _context.SaveChanges();

        return RedirectToAction("AppointmentRequests");
    }

    // ❌ Reject
    public IActionResult RejectAppointment(int id)
    {
        var appt = _context.Appointments.Find(id);

        if (appt == null)
            return NotFound();

        appt.ScheduleStatus = "Rejected";
        _context.SaveChanges();

        return RedirectToAction("AppointmentRequests");
    }
    [HttpPost]
    public IActionResult Reschedule(int id, DateTime newDate)
    {
        var appt = _context.Appointments.Find(id);

        if (appt != null)
        {
            appt.AppointmentDateTime = newDate;
            appt.ScheduleStatus = "Approved";

            _context.SaveChanges();
        }

        return RedirectToAction("AppointmentRequests");
    }
}