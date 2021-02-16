using BOT.DATA.Helper;
using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using BOT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BOT.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotificacionService _notificacionService;

        public HomeController(ILogger<HomeController> logger,
            INotificacionService notificacionService
            )
        {
            _logger = logger;
            _notificacionService = notificacionService;
        }

        public IActionResult Index()
        {
            var result = _notificacionService.List(new NotificacionModel(), new PagerModel());
            return View();
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
