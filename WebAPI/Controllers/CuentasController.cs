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
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly APIContext _context;

        public CuentasController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Cuentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            return await _context.Cuenta.ToListAsync();
        }

        // GET: api/Cuentas/5
        [HttpGet("{numerotarjeta}")]
        public async Task<ActionResult<Cuenta>> GetCuenta(string id)
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            var cuenta = await _context.Cuenta.FindAsync(id);

            if (cuenta == null)
            {
                return NotFound();
            }

            return cuenta;
        }

        // PUT: api/Cuentas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(string id, Cuenta cuenta)
        {
            if (id != cuenta.NumeroTarjeta)
            {
                return BadRequest();
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cuentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta)
        {
          if (_context.Cuenta == null)
          {
              return Problem("Entity set 'APIContext.Cuentas'  is null.");
          }
            _context.Cuenta.Add(cuenta);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CuentaExists(cuenta.NumeroTarjeta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCuenta", new { id = cuenta.NumeroTarjeta }, cuenta);
        }

        /*
        // DELETE: api/Cuentas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(string id)
        {
            if (_context.Cuenta == null)
            {
                return NotFound();
            }
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            _context.Cuenta.Remove(cuenta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool CuentaExists(string id)
        {
            return (_context.Cuenta?.Any(e => e.NumeroTarjeta == id)).GetValueOrDefault();
        }
    }
}
