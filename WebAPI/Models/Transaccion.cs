using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Transaccion
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string NumeroTarjeta { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        [Required]
        public decimal Monto { get; set; }
    }
}
