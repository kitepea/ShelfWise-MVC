using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
