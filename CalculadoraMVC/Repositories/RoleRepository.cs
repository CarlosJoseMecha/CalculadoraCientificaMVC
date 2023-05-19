using CalculadoraMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace CalculadoraMVC.Repositories
{
   public class RoleRepository : IRoleRepository
   {
      private readonly ApplicationDbContext _context;

      public RoleRepository(ApplicationDbContext context)
      {
         _context = context;
      }

      public ICollection<IdentityRole> GetRoles()
      {
         return _context.Roles.ToList();
      }

      public Task<int> SaveChangesAsync()
      {
         return _context.SaveChangesAsync();
      }
   }
}