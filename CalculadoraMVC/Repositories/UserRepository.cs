using CalculadoraMVC.Models;

namespace CalculadoraMVC.Repositories
{
   public class UserRepository : IUserRepository
   {
      private readonly ApplicationDbContext _context;

      public UserRepository(ApplicationDbContext context)
      {
         _context = context;
      }

      public ApplicationUser DeleteUser(ApplicationUser user)
      {
         _context.Users.Remove(user);
         _context.SaveChanges();
         return user;
      }

      public ICollection<ApplicationUser> GetAll()
      {
         return _context.Users.ToList();
      }

      public ApplicationUser GetById(string id) => _context.Users.FirstOrDefault(u => u.Id == id);

      public ApplicationUser UpdateUser(ApplicationUser user)
      {
         _context.Users.Update(user);
         _context.SaveChanges();
         return user;
      }

      public Task<int> SaveChangesAsync()
      {
         return _context.SaveChangesAsync();
      }
   }
}