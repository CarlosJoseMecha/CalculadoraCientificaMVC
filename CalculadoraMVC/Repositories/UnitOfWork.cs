using CalculadoraMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraMVC.Repositories
{
   public class UnitOfWork : IUnitOfWork
   {
      private readonly ApplicationDbContext _dbContext;
      public IOperacionRepository OperacionRepository { get; set; }
      public IUserRepository User { get; set; }
      public IRoleRepository Role { get; set; }

      public UnitOfWork(ApplicationDbContext dbContext, IUserRepository user, IRoleRepository role, IOperacionRepository operacionRepository)
      {
         _dbContext = dbContext;
         OperacionRepository = operacionRepository;
         User = user;
         Role = role;

      }

      public async Task<int> SaveChangesAsync()
      {
         await OperacionRepository.SaveChangesAsync();
         await User.SaveChangesAsync();
         await Role.SaveChangesAsync();

         return await _dbContext.SaveChangesAsync();
      }

   }
}


