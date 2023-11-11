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
    [Route("api/transacciones")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly APIContext _context;
        private readonly ILogger _logger;
        public decimal PorcentajeInteres { get; } = (5m/100);

        public decimal PorcentajeSaldoMinimo { get; } = (5m/100);

        public TransaccionesController(APIContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<TransaccionesController>();
        }

        // GET: api/Transacciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaccion>>> GetTransaccion()
        {
          if (_context.Transaccion == null)
          {
              return NotFound();
          }
            return await _context.Transaccion.ToListAsync();
        }

        [HttpGet("{numerotarjeta}")]
        public async Task<ActionResult<IEnumerable<Transaccion>>> GetTransaccionesByNumber(string numerotarjeta, [FromQuery] TransaccionFiltros filtros)
        {
            if(_context.Transaccion == null)
            { return NotFound(); }

            try
            {
                List<Transaccion> transacciones = new List<Transaccion>();

                if(filtros.tipo == "Todas")
                {
                    transacciones = _context.Transaccion.Where(u => u.NumeroTarjeta == numerotarjeta).ToList();
                }
                else
                {
                    transacciones = _context.Transaccion.Where(u => u.NumeroTarjeta == numerotarjeta && u.Tipo == filtros.tipo && u.Fecha.Month == filtros.mes).ToList();
                }
                
                if(transacciones == null)
                {
                    return BadRequest("No se encontraron transacciones asociadas a la tarjeta.");
                }

                return Ok(transacciones);

            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("reciente/{numerotarjeta}")]
        public async Task<ActionResult<decimal>> GetTransaccionesRecientes(string numerotarjeta, [FromQuery] TransaccionRecienteFiltros filtros)
        {
            if (_context.Transaccion == null)
            { return NotFound(); }

            try
            {
                decimal montoRecienteTotal = _context.Transaccion.Where(u => u.NumeroTarjeta == numerotarjeta && u.Fecha.Month >= filtros.mesanterior && u.Fecha.Month <= filtros.mes).Select(u => u.Monto).Sum();

                if(montoRecienteTotal == null)
                {
                    return BadRequest("No se encontrar transacciones recientes asociadas a la tarjeta.");
                }

                return Ok(montoRecienteTotal);
            }
            catch 
            {
                return StatusCode(500);
            }
        }

        // POST: api/transacciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaccion>> PostTransaccion(Transaccion transaccion)
        {
            try
            {
                var cuentaActual = await _context.Cuenta.FindAsync(transaccion.NumeroTarjeta);

                if(cuentaActual == null)
                {
                    return BadRequest("La cuenta asociada a la tarjeta no se encuentra.");
                }

                decimal saldo = 0;

                if (transaccion.Tipo == "Compra")
                {
                    saldo = cuentaActual.SaldoActual + transaccion.Monto;

                    if (Math.Abs(saldo) > cuentaActual.SaldoDisponible)
                    {
                        return BadRequest("Usted a alcanzado el límite de su credito, realizar un pago lo antes posible.");
                    }
                }

                _context.Transaccion.Add(transaccion);
                int rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected <= 0)
                {
                    return StatusCode(500, "Error al guardar la transacción.");
                }

                switch (transaccion.Tipo)
                {
                    case "Compra":
                        cuentaActual.SaldoActual += transaccion.Monto;
                        cuentaActual.SaldoDisponible -= transaccion.Monto;
                        break;

                    case "Pago":
                        cuentaActual.SaldoActual -= transaccion.Monto;
                        cuentaActual.SaldoDisponible += transaccion.Monto;
                        break;

                    default: return StatusCode(500, "Tipo de transacción no válido.");
                }
                cuentaActual.InteresBonificable = cuentaActual.SaldoActual*PorcentajeInteres;
                cuentaActual.CuotaMinima = cuentaActual.SaldoActual * (cuentaActual.SaldoActual*PorcentajeSaldoMinimo);
                cuentaActual.MontoPago = cuentaActual.SaldoActual;
                cuentaActual.MontoPagoIntereses = cuentaActual.SaldoActual + cuentaActual.InteresBonificable;
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTransaccion", new { id = transaccion.ID }, transaccion);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        private bool TransaccionExists(int id)
        {
            return (_context.Transaccion?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }

    public class TransaccionFiltros
    {
        public int mes { get; set; }

        public string tipo { get; set; }
    }

    public class TransaccionRecienteFiltros
    {
        public int mesanterior { get; set; }
        public int mes { get; set; }
    }
}
