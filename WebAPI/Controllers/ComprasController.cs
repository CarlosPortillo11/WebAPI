using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/compras")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly APIContext _context;

        public ComprasController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Compras
        /*
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Compra>>> GetCompra()
            {
              if (_context.Compra == null)
              {
                  return NotFound();
              }
                return await _context.Compra.ToListAsync();
            }
         */

        //GET: api/compras/tarjeta/4390930039010979
        [HttpGet("tarjeta/{numerotarjeta}")]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompraByNumber(string numerotarjeta,[FromQuery] GetCompraFilters filtros)
        {
            try 
            {
                if (_context.Compra == null)
                {
                    return NotFound();
                }

                List<Compra> compra = new List<Compra>();

                if (filtros.mes == null)
                {
                    compra =  _context.Compra.Where(u => u.NumeroTarjeta == numerotarjeta).ToList();
                }
                else
                {
                    compra = _context.Compra.Where(u => u.NumeroTarjeta == numerotarjeta && u.Fecha.Month == filtros.mes).ToList();
                }

                if (compra == null)
                {
                    return BadRequest("No se encontró ninguna compra asociada a la tarjeta");
                }

                return Ok(compra);

            }catch
            {
                return StatusCode(500);
            }
        }

        //GET: api/compras/montoreciente/4390930039010979
        [HttpGet("montoreciente/{numerotarjeta}")]
        public async Task<ActionResult<decimal>> GetMontoReciente(string numerotarjeta, [FromQuery] GetCompraFilters filtros)
        {
            try
            {
                if (_context.Compra == null)
                {
                    return NotFound();
                }

                
                decimal montoRecienteTotal = _context.Compra.Where(u => u.NumeroTarjeta == numerotarjeta && u.Fecha.Month >= filtros.mesanterior && u.Fecha.Month <= filtros.mes).Select(u => u.Monto).Sum();

                if (montoRecienteTotal == null)
                {
                    return BadRequest("No se encontró ninguna compra asociada a la tarjeta");
                }

                return Ok(montoRecienteTotal);

            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
          if (_context.Compra == null)
          {
              return NotFound();
          }
            var compra = await _context.Compra.FindAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            return compra;
        }


        // POST: api/Compras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compra>> PostCompra(Compra compra)
        {
          if (_context.Compra == null)
          {
              return Problem("Entity set 'APIContext.Compra'  is null.");
          }

            using (var transactionScope = new TransactionScope()) 
            {
                try
                {
                    var cuentaActual = await _context.Cuenta.FindAsync(compra.NumeroTarjeta);
                    decimal nuevoSaldo = cuentaActual.SaldoActual - compra.Monto;

                    if(cuentaActual.LimiteCredito <= nuevoSaldo)
                    {
                        return StatusCode(400);
                    }

                    _context.Compra.Add(compra);
                    int rowAffected = await _context.SaveChangesAsync();

                    if (rowAffected > 0)
                    {
                        /*
                        var cuenta = await _context.Cuenta.FindAsync(compra.NumeroTarjeta);
                        cuenta.SaldoActual += compra.Monto;
                        cuenta.SaldoDisponible -= compra.Monto;
                        cuenta.InteresBonificable = cuenta.SaldoActual;
                        await _context.SaveChangesAsync();
                        */

                        transactionScope.Complete();

                        return CreatedAtAction("GetCompra", new { id = compra.id }, compra);
                    }

                    return StatusCode(500);


                }catch (Exception ex) 
                {
                    return StatusCode(500);
                }
            }

        }

        /*
        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompra(int id)
        {
            if (_context.Compra == null)
            {
                return NotFound();
            }
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }

            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }
         */

        private bool CompraExists(int id)
        {
            return (_context.Compra?.Any(e => e.id == id)).GetValueOrDefault();
        }

    }


    public class GetCompraFilters
    {
        public int? mesanterior {  get; set; }
        public int? mes { get; set; }

        public string tipo { get; set; }
    }
}
