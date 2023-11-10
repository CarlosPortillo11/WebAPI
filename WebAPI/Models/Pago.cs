using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Pago
    {
        [Key]
        public int id { get; set; }

        public string? NumeroTarjeta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }

    }
}
