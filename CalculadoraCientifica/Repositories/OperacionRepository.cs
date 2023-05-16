using CalculadoraCientifica.Models;
using System.Security.Claims;

namespace CalculadoraCientifica.Repositories
{
   public class OperacionRepository : IOperacionRepository
   {
      private ApplicationDbContext _context;
      public OperacionRepository(ApplicationDbContext context)
      {
         _context = context;
      }

      public void Add(Operacion operacion)
      {
         _context.Add(operacion);
         _context.SaveChanges();

      }

      public void Delete(int id)
      {
         var entidad = _context.Operaciones.FirstOrDefault(e => e.Id == id);

         if (entidad != null)
         {
            _context.Remove(entidad);
            _context.SaveChanges();
         }
      }
      public void DeleteMultiple(int[] ids)
      {
         var entities = _context.Operaciones.Where(e => ids.Contains(e.Id));
         _context.Operaciones.RemoveRange(entities);
         _context.SaveChanges();
      }

      public IEnumerable<Operacion> GetAll(string UsuarioId)
      {
         return _context.Operaciones.Where(o => o.UsuarioId == UsuarioId).ToList();
      }

      public Operacion GetById(int id)
      {
         var operacion = _context.Operaciones.Where(o => o.Id == id).FirstOrDefault();
         if (operacion == null)
         {
            throw new InvalidOperationException($"No se ha encontrado la operacion con id: {id}");
         }
         else
         {
            return operacion;
         }
      }

      public void RemoveRange(IEnumerable<Operacion> operations)
      {
         _context.Operaciones.RemoveRange(operations);

      }

      public async Task<int> SaveChangesAsync()
      {
         return await _context.SaveChangesAsync();
      }


      public void Update(Operacion operacion)
      {
         throw new NotImplementedException();
      }
   }
}
