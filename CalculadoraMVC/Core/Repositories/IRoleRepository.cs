using Microsoft.AspNetCore.Identity;

namespace CalculadoraMVC.Repositories
{
   public interface IRoleRepository
   {
      ICollection<IdentityRole> GetRoles();
      Task<int> SaveChangesAsync();
   }
}