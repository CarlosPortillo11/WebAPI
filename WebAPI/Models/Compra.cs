using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Compra
    {
        [Key]
        public int id { get; set; }
        public string? NumeroTarjeta { get; set; }

        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
