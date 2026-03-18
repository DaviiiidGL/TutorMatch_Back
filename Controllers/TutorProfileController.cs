using Microsoft.AspNetCore.Mvc;

namespace TutorMatch.Controllers
{
    public class TutorProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
