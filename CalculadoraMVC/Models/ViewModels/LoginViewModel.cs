using System.ComponentModel.DataAnnotations;

namespace CalculadoraMVC.Models.ViewModels
{
   public class LoginViewModel
   {
      [Required(ErrorMessage = "Por favor introduce un email.")]
      [EmailAddress]
      public string? Email { get; set; }

      [Required(ErrorMessage = "Por favor introduce la contraseña.")]
      [DataType(DataType.Password)]
      public string? Password { get; set; }

      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
   }
}