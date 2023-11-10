using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/pagos")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly APIContext _context;

        public PagosController(APIContext context)
        {
            _context = context;
        }

        /*
            // GET: api/Pagos
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Pago>>> GetPago()
            {
              if (_context.Pago == null)
              {
                  return NotFound();
              }
                return await _context.Pago.ToListAsync();
            }
         */

        // GET: api/compras/tarjeta/4390930039010978
        [HttpGet("tarjeta/{numerotarjeta}")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagosByNumber(string numerotarjeta)
        {
            try
            {
                if (_context.Pago == null)
                {
                    return NotFound();
                }

                List<Pago> pago = new List<Pago>();

                pago = _context.Pago.Where(u => u.NumeroTarjeta == numerotarjeta).ToList();

                if (pago == null)
                {
                    return BadRequest("No se encontró ninguna compra asociada a la tarjeta");
                }

                return Ok(pago);

            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> GetPago(int id)
        {
            if (_context.Pago == null)
            {
                return NotFound();
            }
            var pago = await _context.Pago.FindAsync(id);

            if (pago == null)
            {
                return NotFound();
            }

            return pago;
        }

        // POST: api/Pagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pago>> PostPago(Pago pago)
        {
          if (_context.Pago == null)
          {
              return Problem("Entity set 'APIContext.Pago'  is null.");
          }
            _context.Pago.Add(pago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPago", new { id = pago.id }, pago);
        }

        private bool PagoExists(int id)
        {
            return (_context.Pago?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
