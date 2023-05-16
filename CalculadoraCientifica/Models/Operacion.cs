using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculadoraCientifica.Models
{
#nullable disable
    public class Operacion
   {
      [Key]
      public int Id { get; set; }
      public string Expresion { get; set; }
      public string Resultado { get; set; }
      [Column("FechaHora")]
      public string FechaHora { get; set; }

      public string UsuarioId { get; set; }
      public ApplicationUser ApplicationUser { get; set; }

      public Operacion()
      {
         Expresion = string.Empty;
         Resultado = string.Empty;
         UsuarioId = string.Empty;
      }
   }
#nullable restore
}