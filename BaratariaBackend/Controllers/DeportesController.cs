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
    public class DeportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Deportes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deporte>>> GetDeportes()
        {
            return await _context.Deportes.ToListAsync();
        }

        // GET: api/Deportes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Deporte>> GetDeporte(int id)
        {
            var actividad = await _context.Deportes.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Deportes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeporte(int id, Deporte actividad)
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
                if (!DeporteExists(id))
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

        // POST: api/Deportes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Deporte>> PostDeporte(Deporte actividad)
        {
            _context.Deportes.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeporte", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Deportes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeporte(int id)
        {
            var actividad = await _context.Deportes.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Deportes.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeporteExists(int id)
        {
            return _context.Deportes.Any(e => e.Id == id);
        }

        // GET: api/Deportes/EnlacesByDeporte/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnlaceDeporte>>> GetEnlacesByDeporte(int id)
        {
            return await _context.EnlacesDeporte.Where(t => t.DeporteId == id).ToListAsync();
        }

        // GET: api/Deportes/Enlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnlaceDeporte>> GetEnlace(int id)
        {
            var enlaceDeporte = await _context.EnlacesDeporte.FindAsync(id);

            if (enlaceDeporte == null)
            {
                return NotFound();
            }

            return enlaceDeporte;
        }

        // PUT: api/Deportes/Enlaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnlace(int id, EnlaceDeporte enlaceDeporte)
        {
            if (id != enlaceDeporte.Id)
            {
                return BadRequest();
            }

            _context.Entry(enlaceDeporte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnlaceDeporteExists(id))
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

        // POST: api/Deportes/Enlaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EnlaceDeporte>> PostEnlace(EnlaceDeporte enlaceDeporte)
        {
            _context.EnlacesDeporte.Add(enlaceDeporte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnlace", new { id = enlaceDeporte.Id }, enlaceDeporte);
        }

        // DELETE: api/Deportes/Enlaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnlace(int id)
        {
            var enlaceDeporte = await _context.EnlacesDeporte.FindAsync(id);
            if (enlaceDeporte == null)
            {
                return NotFound();
            }

            _context.EnlacesDeporte.Remove(enlaceDeporte);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EnlaceDeporteExists(int id)
        {
            return _context.EnlacesDeporte.Any(e => e.Id == id);
        }
    }
}
