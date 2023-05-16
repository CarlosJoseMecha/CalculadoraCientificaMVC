using CalculadoraCientifica.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCientifica.Repositories
{
   public interface IOperacionRepository
   {
      IEnumerable<Operacion> GetAll(string UsuarioId);
      Operacion GetById(int id);
      void Add(Operacion operacion);
      void Update(Operacion operacion);
      void Delete(int id);
      void DeleteMultiple(int[] ids);
      void RemoveRange(IEnumerable<Operacion> operations);
      Task<int> SaveChangesAsync();

   }
}
