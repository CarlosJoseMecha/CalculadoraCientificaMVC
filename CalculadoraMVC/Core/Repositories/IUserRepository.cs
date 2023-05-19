using CalculadoraMVC.Models;

namespace CalculadoraMVC.Repositories
{
   public interface IUserRepository
   {
      ICollection<ApplicationUser> GetAll();
      ApplicationUser GetById(string id);
      ApplicationUser UpdateUser(ApplicationUser user);
      ApplicationUser DeleteUser(ApplicationUser user);
      Task<int> SaveChangesAsync();
   }
}