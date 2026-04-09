using Microsoft.AspNetCore.Mvc;

namespace Warriors_Clinic.Controllers
{
    public class ChemistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
