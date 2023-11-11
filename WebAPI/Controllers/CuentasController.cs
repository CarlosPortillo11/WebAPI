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

        // GET: api/cuentas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            return await _context.Cuenta.ToListAsync();
        }

        // GET: api/cuentas/################
        [HttpGet("{numerotarjeta}")]
        public async Task<ActionResult<Cuenta>> GetCuenta(string numerotarjeta)
        {
          if (_context.Cuenta == null)
          {
              return NotFound();
          }
            var cuenta = await _context.Cuenta.FindAsync(numerotarjeta);

            if (cuenta == null)
            {
                return NotFound();
            }

            return cuenta;
        }

        // POST: api/cuentas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta nuevacuenta)
        {
          if (_context.Cuenta == null)
          {
              return Problem("El contexto de la base de datos: 'APIContext.Cuentas' devolvió nulo.");
          }
            _context.Cuenta.Add(nuevacuenta);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CuentaExists(nuevacuenta.NumeroTarjeta))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCuentas", new { id = nuevacuenta.NumeroTarjeta }, nuevacuenta);
        }

        private bool CuentaExists(string id)
        {
            return (_context.Cuenta?.Any(e => e.NumeroTarjeta == id)).GetValueOrDefault();
        }
    }
}
