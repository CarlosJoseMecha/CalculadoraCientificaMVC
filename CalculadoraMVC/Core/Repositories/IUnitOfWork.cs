namespace CalculadoraMVC.Repositories
{
   public interface IUnitOfWork
   {
      IOperacionRepository OperacionRepository { get; }
      IUserRepository User { get; }
      IRoleRepository Role { get; }
      Task<int> SaveChangesAsync();
   }
}