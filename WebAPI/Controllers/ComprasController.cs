﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompra()
        {
          if (_context.Compra == null)
          {
              return NotFound();
          }
            return await _context.Compra.ToListAsync();
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

        //GET: api/compras/tarjeta/4390930039010979
        [HttpGet("tarjeta/{numerotarjeta}")]
        public async Task<ActionResult<Compra>> GetCompraByNumber(string numerotarjeta)
        {
            try 
            {
                if (_context.Compra == null)
                {
                    return NotFound();
                }
                //List<Compra> compra =  _context.Compra.Where(u => u.NumeroTarjeta == numerotarjeta).ToList();
                //var compra = await _context.Compra.FindAsync(id);
                var compra = _context.Compra.Where(u => u.NumeroTarjeta == numerotarjeta);

                if (compra == null)
                {
                    return BadRequest("No se encontró ninguna compra asociada a la tarjeta");
                }

                return Ok(compra);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/Compras/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompra(int id, Compra compra)
        {
            if (id != compra.id)
            {
                return BadRequest();
            }

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
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

        // POST: api/Compras
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compra>> PostCompra(Compra compra)
        {
          if (_context.Compra == null)
          {
              return Problem("Entity set 'APIContext.Compra'  is null.");
          }
            _context.Compra.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompra", new { id = compra.id }, compra);
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
}