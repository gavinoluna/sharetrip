using Microsoft.AspNetCore.Mvc;

namespace SharedTrip.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}