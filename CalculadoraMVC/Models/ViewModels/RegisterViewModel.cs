using System.ComponentModel.DataAnnotations;

namespace CalculadoraMVC.Models.ViewModels
{
   public class RegisterViewModel
   {
      [Required(ErrorMessage = "Por favor introduce un nombre de usuario.")]
      [Display(Name = "FirstName")]
      [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El nombre solo puede contener letras y números.")]
      public string? FirstName { get; set; }

      [Required(ErrorMessage = "Por favor introduce un apellido.")]
      [Display(Name = "LastName")]
      [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El apellido solo puede contener letras.")]
      public string? LastName { get; set; }

      [Required(ErrorMessage = "Por favor introduce un email.")]
      [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
      [Display(Name = "Email")]
      public string? Email { get; set; }

      [Required(ErrorMessage = "Por favor introduce una contraseña.")]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string? Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "Las contraseñas deben coincidir.")]
      public string? ConfirmPassword { get; set; }
   }
}