using CalculadoraCientifica.Models;
using CalculadoraCientifica.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalculadoraCientifica.Controllers
{
   public class HistorialController : Controller
   {
      private readonly IOperacionRepository _repository;

      public HistorialController(IOperacionRepository repository)
      {
         _repository = repository;
      }

      [Authorize]
      public IActionResult Historial()
      {
         var UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         var data = _repository.GetAll(UsuarioId);
         return View("~/Views/Account/Historial.cshtml", data);
      }

      [HttpPost]
      public IActionResult Create([FromBody] Operacion operacion)
      {
         var UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         if (UsuarioId == null)
         {
            return Unauthorized();
         }
         operacion.UsuarioId = UsuarioId;
         _repository.Add(operacion);
         return Ok();
      }


      [HttpPost]
      [Route("Historial/DeleteMultiple")]
      public IActionResult DeleteMultiple(int[] ids)
      {
         _repository.DeleteMultiple(ids);
         return RedirectToAction("Historial");
      }

   }
}


