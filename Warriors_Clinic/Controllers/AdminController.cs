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
        var data = _context.Physicians
            .Select(p => new Warriors_Clinic.Models.ViewModels.PhysicianVM
            {
                PhysicianId = p.PhysicianId,
                Name = p.Name,
                Email = p.Email,
                Phone = p.Phone,
                Specialization = p.Specialization,

                Status = _context.Users
                    .Where(u => u.ReferenceToId == p.PhysicianId && u.Role == "Physician")
                    .Select(u => u.Status)
                    .FirstOrDefault()
            })
            .ToList();

        return View(data);
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

    
 
    public IActionResult EnablePhysician(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Physician");

        if (user != null)
        {
            user.Status = "Active";
            _context.SaveChanges();
        }

        return RedirectToAction("PhysicianList");
    }

    public IActionResult DisablePhysician(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Physician");

        if (user != null)
        {
            user.Status = "Disabled";
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
        var data = _context.Chemists
            .Select(c => new Warriors_Clinic.Models.ViewModels.ChemistVM
            {
                ChemistId = c.ChemistId,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,

                Status = _context.Users
                    .Where(u => u.ReferenceToId == c.ChemistId && u.Role == "Chemist")
                    .Select(u => u.Status)
                    .FirstOrDefault()
            })
            .ToList();

        return View(data);
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

    
    public IActionResult EnableChemist(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Chemist");

        if (user != null)
        {
            user.Status = "Active";
            _context.SaveChanges();
        }

        return RedirectToAction("ChemistList");
    }

    public IActionResult DisableChemist(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Chemist");

        if (user != null)
        {
            user.Status = "Disabled";
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
        var data = _context.Suppliers
            .Select(s => new Warriors_Clinic.Models.ViewModels.SupplierVM
            {
                SupplierId = s.SupplierId,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,

                Status = _context.Users
                    .Where(u => u.ReferenceToId == s.SupplierId && u.Role == "Supplier")
                    .Select(u => u.Status)
                    .FirstOrDefault()
            })
            .ToList();

        return View(data);
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

   
    public IActionResult EnableSupplier(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Supplier");

        if (user != null)
        {
            user.Status = "Active";
            _context.SaveChanges();
        }

        return RedirectToAction("SupplierList");
    }

    public IActionResult DisableSupplier(int id)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.ReferenceToId == id && u.Role == "Supplier");

        if (user != null)
        {
            user.Status = "Disabled";
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
        // ✅ Get all appointments with related data
        var data = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Physician)
            .ToList();

        // ✅ Send ACTIVE physicians for dropdown
        ViewBag.Physicians = _context.Physicians
            .Where(p => p.Status == "Active")
            .ToList();

        return View(data);
    }

    [HttpPost]
    public IActionResult ApproveAppointment(int appointmentId, int physicianId, DateTime appointmentDateTime)
    {
        var appointment = _context.Appointments
            .FirstOrDefault(a => a.AppointmentId == appointmentId);

        if (appointment == null)
            return NotFound();

        // ✅ Assign doctor
        appointment.PhysicianId = physicianId;

        // ✅ Update date/time
        appointment.AppointmentDateTime = appointmentDateTime;

        // ✅ Approve
        appointment.ScheduleStatus = "Approved";

        _context.SaveChanges();

        return RedirectToAction("AppointmentRequests");
    }

    public IActionResult RejectAppointment(int id)
    {
        var appointment = _context.Appointments.Find(id);

        if (appointment != null)
        {
            appointment.ScheduleStatus = "Rejected";
            _context.SaveChanges();
        }

        return RedirectToAction("AppointmentRequests");
    }


    [HttpPost]
    public IActionResult Reschedule(int id, DateTime newDate)
    {
        var appointment = _context.Appointments.Find(id);

        if (appointment != null)
        {
            // ✅ ONLY update date
            appointment.AppointmentDateTime = newDate;

           

            _context.SaveChanges();
        }

        return RedirectToAction("AppointmentRequests");
    }

}