using CalculadoraCientifica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CalculadoraCientifica.Models.ViewModels;
using System.Security.Claims;
using CalculadoraCientifica.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace CalculadoraCientifica.Controllers
{
   public class AccountController : Controller
   {
      private readonly SignInManager<ApplicationUser> _signInManager;
      private readonly UserManager<ApplicationUser> _userManager;
      private readonly IUserStore<ApplicationUser> _userStore;
      private readonly IUserEmailStore<ApplicationUser> _emailStore;
      private readonly IOperacionRepository _repository;

      public AccountController(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, SignInManager<ApplicationUser> signInManager, IOperacionRepository repository)
      {
         _userManager = userManager;
         _userStore = userStore;
         _emailStore = GetEmailStore();
         _signInManager = signInManager;

         _repository = repository;
      }

      public IActionResult Login()
      {
         if (this.User?.Identity?.IsAuthenticated == true)
         {
            return RedirectToAction("Profile", "Account");
         }
         return View();

      }

      [HttpPost, ActionName("Login")]
      public async Task<IActionResult> LoginPost(LoginViewModel loginModel)
      {
         if (ModelState.IsValid)
         {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
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

      public async Task<IActionResult> Profile()
      {
         var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
         var user = await _userManager.FindByIdAsync(userId);
         if (user == null)
         {
            return RedirectToAction("Index", "Home");
         }
         ViewData["Email"] = user.Email;
         ViewData["FirstName"] = user.FirstName;
         ViewData["LastName"] = user.LastName;
         return View();
      }


      [HttpPost]
      public async Task<IActionResult> DeleteAccount()
      {
         var user = await _userManager.GetUserAsync(HttpContext.User);
         var userId = user.Id;

         var userOperations = _repository.GetAll(userId);
         _repository.RemoveRange(userOperations);
         await _repository.SaveChangesAsync();

         var result = await _userManager.DeleteAsync(user);
         await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
         return RedirectToAction("Index", "Home");
      }


      public IActionResult AccessDenied()
      {
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
