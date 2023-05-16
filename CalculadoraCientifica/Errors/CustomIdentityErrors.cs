namespace CalculadoraCientifica.Errors
{
   using Microsoft.AspNetCore.Identity;

   public class CustomIdentityErrorDescriber : IdentityErrorDescriber
   {
      public override IdentityError PasswordRequiresNonAlphanumeric()
      {
         return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contraseña deben tener al menos un carácter no alfanumérico." };
      }

      public override IdentityError PasswordRequiresUpper()
      {
         return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "La contraseña deben tener al menos un carácter en mayúsculas." };
      }

      public override IdentityError DuplicateEmail(string email)
      {
         return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Ese email ya está registrado." };
      }

      public override IdentityError DuplicateUserName(string userName)
      {
         return new IdentityError { Code = nameof(DuplicateUserName), Description = $"El email '{userName}' ya está registrado." };
      }

   }

}
