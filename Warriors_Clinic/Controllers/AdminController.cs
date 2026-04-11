using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warriors_Clinic.Models;
using System.Linq;
 //heyy
public class AdminController : Controller
{
    private readonly AppDbContext _context;
 
    public AdminController(AppDbContext context)
    {
        _context = context;
    }
 
    // ================= DASHBOARD =================
    public IActionResult Dashboard()
    {
        return View();
    }
 
    public IActionResult Workforce()
    {
        return View();
    }
 
    // ================= PENDING USERS =================
    public IActionResult PendingUsers()
    {
        var users = _context.Users.Where(u => !u.IsApproved).ToList();
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
 
    // ================= PHYSICIAN =================
    public IActionResult PhysicianMenu()
    {
        return View();
    }
 
    public IActionResult AddPhysician()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddPhysician(Physician model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.Physicians.Add(model);
        _context.SaveChanges();

        var namePart = model.Name.Replace(" ", "").ToLower();
        var phonePart = model.Phone.Length >= 4
            ? model.Phone.Substring(model.Phone.Length - 4)
            : "0000";

        var generatedPassword = namePart + phonePart;

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            Password = generatedPassword,
            Role = "Physician",
            IsApproved = true,
            ReferenceToId = model.PhysicianId
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // ✅ PASS DATA TO VIEW
        ViewBag.Username = user.UserName;
        ViewBag.Password = user.Password;
        ViewBag.Role = "Physician";

        return View("ShowCredentials",user);

        
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
 
    public IActionResult DeletePhysician(int id)
    {
        var data = _context.Physicians.Find(id);
 
        if (data != null)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Physician");
 
            if (user != null)
                _context.Users.Remove(user);
 
            _context.Physicians.Remove(data);
            _context.SaveChanges();
        }
 
        return RedirectToAction("PhysicianList");
    }
 
    // ================= CHEMIST =================
    public IActionResult ChemistMenu()
    {
        return View();
    }
 
    public IActionResult AddChemist()
    {
        return View();
    }

    [HttpPost]   // ✅ ONLY ONE
    public IActionResult AddChemist(Chemist model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _context.Chemists.Add(model);
        _context.SaveChanges();

        var namePart = model.Name.Replace(" ", "").ToLower();
        var phonePart = model.Phone.Length >= 4
            ? model.Phone.Substring(model.Phone.Length - 4)
            : "0000";

        var generatedPassword = namePart + phonePart;

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            Password = generatedPassword,
            Role = "Chemist",
            IsApproved = true,
            ReferenceToId = model.ChemistId
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // ✅ PASS DATA TO VIEW
        ViewBag.Username = user.UserName;
        ViewBag.Password = user.Password;
        ViewBag.Role = "Chemist";

        return View("ShowCredentials", user);


    }
    public IActionResult ChemistList()
    {
        return View(_context.Chemists.ToList());
    }
 
    public IActionResult EditChemist(int id)
    {
        return View(_context.Chemists.Find(id));
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
        var data = _context.Chemists.Find(id);
 
        if (data != null)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Chemist");
 
            if (user != null)
                _context.Users.Remove(user);
 
            _context.Chemists.Remove(data);
            _context.SaveChanges();
        }
 
        return RedirectToAction("ChemistList");
    }
 
    // ================= SUPPLIER =================
    public IActionResult SupplierMenu()
    {
        return View();
    }
 
    public IActionResult AddSupplier()
    {
        return View();
    }
 
    [HttpPost]
    public IActionResult AddSupplier(Supplier model)
    {
        if (ModelState.IsValid)
        {
            _context.Suppliers.Add(model);
            _context.SaveChanges();
 
            var namePart = model.Name.Replace(" ", "").ToLower();
            var phonePart = model.Phone.Length >= 4
                ? model.Phone.Substring(model.Phone.Length - 4)
                : "0000";
 
            var generatedPassword = namePart + phonePart;
 
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Password = generatedPassword,
                Role = "Supplier",
                IsApproved = true,
                ReferenceToId = model.SupplierId
            };
 
            _context.Users.Add(user);
            _context.SaveChanges();

            // ✅ PASS DATA TO VIEW
            ViewBag.Username = user.UserName;
            ViewBag.Password = user.Password;
            ViewBag.Role = "Supplier";

            return View("ShowCredentials", user);
        }
 
        return View(model);
    }
 
    public IActionResult SupplierList()
    {
        return View(_context.Suppliers.ToList());
    }
 
    public IActionResult EditSupplier(int id)
    {
        return View(_context.Suppliers.Find(id));
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
        var data = _context.Suppliers.Find(id);
 
        if (data != null)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Supplier");
 
            if (user != null)
                _context.Users.Remove(user);
 
            _context.Suppliers.Remove(data);
            _context.SaveChanges();
        }
 
        return RedirectToAction("SupplierList");
    }

    //================== SHOW CREDENTIALS =============
    public IActionResult ShowCredentials()
    {
        return View();
    }
 
    // ================= APPOINTMENTS =================
    public IActionResult AppointmentRequests()
    {
        var data = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Physician)
            .ToList();
 
        return View(data);
    }

    public IActionResult ApproveAppointment(int id)
    {
        var appointment = _context.Appointments.Find(id);

        if (appointment != null)
        {
            appointment.ScheduleStatus = "Approved";
            
            _context.SaveChanges();
        }

        return RedirectToAction("AppointmentRequests");
    }

    public IActionResult RejectAppointment(int id)
    {
        var appt = _context.Appointments.Find(id);
 
        if (appt == null) return NotFound();
 
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