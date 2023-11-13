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

        [Range(1, double.MaxValue, ErrorMessage = "Realice una transacción válida.")]
        [Required]
        public decimal Monto { get; set; }
    }
}
