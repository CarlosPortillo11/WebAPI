using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Login
    {
        [Required]
        public required string NumeroTarjeta { get; set; }
        [Required]
        public int PinTarjeta { get; set; }
    }
}
