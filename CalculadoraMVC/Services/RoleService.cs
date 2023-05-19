using CalculadoraMVC.Models.ViewModels;
using CalculadoraMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace CalculadoraMVC.Services
{
   public class RoleService
   {
      private readonly SignInManager<ApplicationUser> _signInManager;

      public RoleService(SignInManager<ApplicationUser> signInManager)
      {
         _signInManager = signInManager;
      }

      public async Task UpdateUserRoles(ApplicationUser user, IEnumerable<RoleViewModel> roles)
      {
         var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

         foreach (var role in roles)
         {
            var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Name);

            if (role.Selected)
            {
               if (assignedInDb == null)
               {
                  await _signInManager.UserManager.AddToRoleAsync(user, role.Name);
               }
            }
            else
            {
               if (assignedInDb != null)
               {
                  await _signInManager.UserManager.RemoveFromRoleAsync(user, role.Name);
               }
            }
         }

         var currentRole = userRolesInDb.FirstOrDefault();
         if (!roles.Any(role => role.Selected))
         {
            if (currentRole != null)
            {
               await _signInManager.UserManager.AddToRoleAsync(user, currentRole);
            }
         }
      }
   }
}