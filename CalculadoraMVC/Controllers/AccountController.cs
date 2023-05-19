using CalculadoraMVC.Core;
using CalculadoraMVC.Models;
using CalculadoraMVC.Models.ViewModels;
using CalculadoraMVC.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalculadoraMVC.Controllers
{
   public class AccountController : Controller
   {
      private readonly IUserEmailStore<ApplicationUser> _emailStore;
      private readonly IUnitOfWork _unitOfWork;

      private readonly SignInManager<ApplicationUser> _signInManager;

      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserStore<ApplicationUser> _userStore;

      public AccountController(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork)
      {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;
         _unitOfWork = unitOfWork;
      }

      [HttpPost]
      public async Task<IActionResult> DeleteAccount()
      {
         var user = await _userManager.GetUserAsync(HttpContext.User);
         var userId = user.Id;

         var userOperations = _unitOfWork.OperacionRepository.GetAll(userId);
         _unitOfWork.OperacionRepository.RemoveRange(userOperations);
         await _unitOfWork.OperacionRepository.SaveChangesAsync();

         var result = await _userManager.DeleteAsync(user);
         await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
         return RedirectToAction("Index", "Home");
      }

      public IActionResult Login()
      {
         if (this.User?.Identity?.IsAuthenticated == true)
         {
            return RedirectToAction("Index", "Home");
         }
         return View();
      }

      [HttpPost, ActionName("Login")]
      public async Task<IActionResult> LoginPost(LoginViewModel loginModel)
      {
         if (ModelState.IsValid)
         {
            var user = await _signInManager.UserManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
               ModelState.AddModelError(string.Empty, "Inicio de sesión incorrecto.");
               return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

            if (result.Succeeded)
            {
               var claims = new List<Claim>
               {
                  new Claim("amr", "pwd"),
               };

               var roles = await _signInManager.UserManager.GetRolesAsync(user);

               if (roles.Any())
               {
                  var roleClaim = string.Join(",", roles);
                  claims.Add(new Claim("Roles", roleClaim));
               }

               await _signInManager.SignInWithClaimsAsync(user, loginModel.RememberMe, claims); ;
               return RedirectToAction("Index", "Home");
            }
            else
            {
               ModelState.AddModelError(string.Empty, "Inicio de sesión incorrecto.");
               return View();
            }
         }

         return View();
      }

      [Authorize]
      public async Task<IActionResult> Logout()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction("Index", "Home");
      }

      [Authorize(Roles = $"{Constants.Roles.User}")]
      public async Task<IActionResult> Profile()
      {
         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         var user = await _userManager.FindByIdAsync(userId);
         if (user == null)
         {
            return RedirectToAction("Login", "Account");
         }
         ViewData["Email"] = user.Email;
         ViewData["FirstName"] = user.FirstName;
         ViewData["LastName"] = user.LastName;
         return View();
      }

      public IActionResult Register()
      {
         if (User?.Identity?.IsAuthenticated == true)
         {
            return RedirectToAction("Index", "Home");
         }
         return View();
      }

      [HttpPost, ActionName("Register")]
      public async Task<IActionResult> RegisterPost(RegisterViewModel registerModel)
      {
         if (ModelState.IsValid)
         {
            var user = CreateUser();

            user.FirstName = registerModel.FirstName;
            user.LastName = registerModel.LastName;

            await _userStore.SetUserNameAsync(user, registerModel.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerModel.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, registerModel.Password);

            if (result.Succeeded)
            {
               await _userManager.AddToRoleAsync(user, "User");
               var userId = await _userManager.GetUserIdAsync(user);

               await _signInManager.SignInAsync(user, isPersistent: false);
               return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
               ModelState.AddModelError(string.Empty, error.Description);
            }
         }

         return View();
      }

      private ApplicationUser CreateUser()
      {
         try
         {
            return Activator.CreateInstance<ApplicationUser>();
         }
         catch
         {
            throw new InvalidOperationException();
         }
      }

      private IUserEmailStore<ApplicationUser> GetEmailStore()
      {
         if (!_userManager.SupportsUserEmail)
         {
            throw new NotSupportedException("The default UI requires a user store with email support.");
         }
         return (IUserEmailStore<ApplicationUser>)_userStore;
      }
   }
}