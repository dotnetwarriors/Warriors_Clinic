using Microsoft.AspNetCore.Mvc;

namespace Warriors_Clinic.Controllers
{
    public class ClinicInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
