using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Cuenta
    {
        [DisplayName("Número de tarjeta")]
        [Key]
        [Required]
        public string NumeroTarjeta { get; set; }

        [Required]
        [DisplayName("Nombre del titular")]
        public string NombreTitular { get; set; }

        [Required]
        [DisplayName("Saldo Actual")]
        public decimal SaldoActual { get; set; }

        [Required]
        public decimal SaldoDisponible { get; set; }

        [Required]
        public decimal LimiteCredito { get; set; }

        [Required]
        public decimal InteresBonificable { get; set; }

        [Required]
        public decimal CuotaMinima { get; set; }

        [Required]
        public decimal MontoPago { get; set; }

        [Required]
        public decimal MontoPagoIntereses { get; set; }
    }
}
