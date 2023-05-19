using CalculadoraMVC.Models;
using CalculadoraMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalculadoraMVC.Controllers
{
   public class HistorialController : Controller
   {
      private readonly IUnitOfWork _unitOfWork;

      public HistorialController(IUnitOfWork unitOfWork)
      {
         _unitOfWork = unitOfWork;
      }

      [Authorize]
      public IActionResult Historial()
      {
         var UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         var data = _unitOfWork.OperacionRepository.GetAll(UsuarioId);
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
         _unitOfWork.OperacionRepository.Add(operacion);
         return Ok();
      }

      [HttpPost]
      [Route("Historial/DeleteMultiple")]
      public IActionResult DeleteMultiple(int[] ids)
      {
         _unitOfWork.OperacionRepository.DeleteMultiple(ids);
         return RedirectToAction("Historial");
      }
   }
}