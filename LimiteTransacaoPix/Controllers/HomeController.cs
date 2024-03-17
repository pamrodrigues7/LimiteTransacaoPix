using LimiteTransacaoPix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LimiteTransacaoPix.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
