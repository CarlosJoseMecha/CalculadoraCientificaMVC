using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CalculadoraMVC.Models.ViewModels
{
   public class EditUserViewModel
   {
      public ApplicationUser User { get; set; }
      public List<RoleViewModel> Roles { get; set; }
   }
}