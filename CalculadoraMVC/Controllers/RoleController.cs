using CalculadoraMVC.Core;
using CalculadoraMVC.Models;
using CalculadoraMVC.Models.ViewModels;
using CalculadoraMVC.Repositories;
using CalculadoraMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraMVC.Controllers
{
   public class RoleController : Controller
   {
      private readonly IUnitOfWork _unitOfWork;
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly UserService _userService;
      private readonly RoleService _roleService;

      public RoleController(IUnitOfWork unitOfWork,
                            SignInManager<ApplicationUser> signInManager,
                            UserManager<ApplicationUser> userManager,
                            UserService userService,
                            RoleService roleService)
      {
         _unitOfWork = unitOfWork;
         _signInManager = signInManager;
         _userManager = userManager;
         _userService = userService;
         _roleService = roleService;
      }

      public IActionResult AccessDenied()
      {
         return View();
      }

      [Authorize(Roles = $"{Constants.Roles.Administrator}")]
      public async Task<IActionResult> Admin()
      {
         var users = _unitOfWork.User.GetAll();
         var userRoles = new Dictionary<string, List<string>>();

         foreach (var user in users)
         {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles.Add(user.Id, roles.ToList());
         }

         ViewBag.UserRoles = userRoles;
         return View(users);
      }

      public async Task<IActionResult> Edit(string id)
      {
         var user = _unitOfWork.User.GetById(id);
         var roles = _unitOfWork.Role.GetRoles();

         var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

         var roleItems = roles.Select(role => new RoleViewModel
         {
            Id = role.Id,
            Name = role.Name,
            Selected = userRoles.Any(ur => ur.Contains(role.Name))
         }).ToList();

         var vm = new EditUserViewModel
         {
            User = user,
            Roles = roleItems,
         };
         return View(vm);
      }

      [HttpPost]
      [Authorize(Roles = $"{Constants.Roles.Administrator}")]
      public async Task<IActionResult> EditOnPost(EditUserViewModel data)
      {
         var user = _unitOfWork.User.GetById(data.User.Id);

         if (user == null)
         {
            return NotFound();
         }

         await _roleService.UpdateUserRoles(user, data.Roles);

         var editUser = new EditUserViewModel
         {
            User = new ApplicationUser
            {
               FirstName = data.User.FirstName,
               LastName = data.User.LastName,
               Email = data.User.Email
            },
            Roles = data.Roles
         };

         _userService.UpdateUser(user, editUser);

         return RedirectToAction("Admin");
      }

      [Authorize(Roles = Constants.Roles.Administrator)]
      public async Task<IActionResult> Delete(string id)
      {
         var user = _unitOfWork.User.GetById(id);

         if (user == null)
         {
            return NotFound();
         }

         _unitOfWork.User.DeleteUser(user);
         await _unitOfWork.SaveChangesAsync();

         return RedirectToAction("Admin");
      }
   }
}