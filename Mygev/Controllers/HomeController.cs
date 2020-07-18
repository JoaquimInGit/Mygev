using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mygev.Data;
using Mygev.Models;

namespace Mygev.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly MygevDB _context;

        public HomeController(ILogger<HomeController> logger, MygevDB context) {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync() {
            return View(await _context.Evento.ToListAsync());
        }

        public IActionResult Privacy() {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
