using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly APIContext _context;

        public LoginController(APIContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin([FromBody] Login login)
        {
            try 
            {
                if (_context.Cuenta == null)
                {
                    return NotFound();
                }

                var usuario = _context.Cuenta.Where(u => u.NumeroTarjeta == login.NumeroTarjeta && u.PinTarjeta == login.PinTarjeta).FirstOrDefault();

                if (usuario == null)
                {
                    return BadRequest("El número de la tarjeta o el pin son incorrectos, intente de nuevo.");
                };

                return Ok(usuario);
                
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
