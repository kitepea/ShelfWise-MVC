using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScopedService _scoped1;
        private readonly IScopedService _scoped2;

        private readonly ITransientService _transient1;
        private readonly ITransientService _transient2;

        private readonly ISingletonService _singleton1;
        private readonly ISingletonService _singleton2;

        public HomeController(
            IScopedService scoped1, IScopedService scoped2,
            ITransientService transient1, ITransientService transient2,
            ISingletonService singleton1, ISingletonService singleton2)
        {
            _scoped1 = scoped1;
            _scoped2 = scoped2;

            _transient1 = transient1;
            _transient2 = transient2;

            _singleton1 = singleton1;
            _singleton2 = singleton2;
        }

        public IActionResult Index()
        {
            StringBuilder messages = new StringBuilder();
            messages.AppendLine($"Transient 1: {_transient1.GetGuid()}\n");
            messages.AppendLine($"Transient 2: {_transient2.GetGuid()}\n");
            messages.AppendLine("Transient - A different instance of a resource, everytime it's requested (i.e. someone requests an ITransientService).\n\n");

            messages.AppendLine($"Scoped 1: {_scoped1.GetGuid()}\n");
            messages.AppendLine($"Scoped 2: {_scoped2.GetGuid()}\n");
            messages.AppendLine("Scoped - One instance of a resource, but only for the current request. New request (i.e. hit an API endpoint again) = new instance\n\n");

            messages.AppendLine($"Singleton 1: {_singleton1.GetGuid()}\n");
            messages.AppendLine($"Singleton 2: {_singleton2.GetGuid()}\n");
            messages.AppendLine("Singleton - One instance of a resource, for the lifetime of the application\n\n");
            return Ok(messages.ToString());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
