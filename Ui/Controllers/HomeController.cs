using Microsoft.AspNetCore.Mvc;

namespace Ui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
