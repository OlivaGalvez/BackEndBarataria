using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Authorize(Policy = "Administrador")]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SorteosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SorteosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Sorteos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sorteo>>> GetSorteos()
        {
            return await _context.Sorteos.ToListAsync();
        }

        // GET: api/Sorteos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sorteo>> GetSorteo(int id)
        {
            var actividad = await _context.Sorteos.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Sorteos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSorteo(int id, Sorteo actividad)
        {
            if (id != actividad.Id)
            {
                return BadRequest();
            }

            _context.Entry(actividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SorteoExists(id))
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

        // POST: api/Sorteos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sorteo>> PostSorteo(Sorteo actividad)
        {
            _context.Sorteos.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSorteo", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Sorteos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSorteo(int id)
        {
            var actividad = await _context.Sorteos.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Sorteos.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SorteoExists(int id)
        {
            return _context.Sorteos.Any(e => e.Id == id);
        }

        // GET: api/Sorteos/EnlacesBySorteo/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnlaceSorteo>>> GetEnlacesBySorteo(int id)
        {
            return await _context.EnlacesSorteo.Where(t => t.SorteoId == id).ToListAsync();
        }

        // GET: api/Sorteos/Enlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnlaceSorteo>> GetEnlace(int id)
        {
            var enlaceSorteo = await _context.EnlacesSorteo.FindAsync(id);

            if (enlaceSorteo == null)
            {
                return NotFound();
            }

            return enlaceSorteo;
        }

        // PUT: api/Sorteos/Enlaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnlace(int id, EnlaceSorteo enlaceSorteo)
        {
            if (id != enlaceSorteo.Id)
            {
                return BadRequest();
            }

            _context.Entry(enlaceSorteo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnlaceSorteoExists(id))
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

        // POST: api/Sorteos/Enlaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EnlaceSorteo>> PostEnlace(EnlaceSorteo enlaceSorteo)
        {
            _context.EnlacesSorteo.Add(enlaceSorteo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnlace", new { id = enlaceSorteo.Id }, enlaceSorteo);
        }

        // DELETE: api/Sorteos/Enlaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnlace(int id)
        {
            var enlaceSorteo = await _context.EnlacesSorteo.FindAsync(id);
            if (enlaceSorteo == null)
            {
                return NotFound();
            }

            _context.EnlacesSorteo.Remove(enlaceSorteo);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EnlaceSorteoExists(int id)
        {
            return _context.EnlacesSorteo.Any(e => e.Id == id);
        }

    }
}
