using CalculadoraMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CalculadoraMVC.Controllers
{
   public class HomeController : Controller
   {
      public IActionResult Index()
      {
         return View();
      }

      [HttpGet("~/Views/Home/Error", Name = "Error")]
      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         return View();
      }

      public IActionResult Privacy()
      {
         return View();
      }
   }
}