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
    public class ActividadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActividadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Actividades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actividad>>> GetActividades()
        {
            return await _context.Actividades.ToListAsync();
        }

        // GET: api/Actividades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actividad>> GetActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Actividades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, Actividad actividad)
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
                if (!ActividadExists(id))
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

        // POST: api/Actividades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actividad>> PostActividad(Actividad actividad)
        {
            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActividad", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Actividades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }

        // GET: api/Actividades/EnlacesByActividad/5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnlaceActividad>>> GetEnlacesByActividad(int id)
        {
            return await _context.EnlacesActividad.Where(t => t.ActividadId == id).ToListAsync();
        }

        // GET: api/Actividades/Enlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnlaceActividad>> GetEnlace(int id)
        {
            var enlaceActividad = await _context.EnlacesActividad.FindAsync(id);

            if (enlaceActividad == null)
            {
                return NotFound();
            }

            return enlaceActividad;
        }

        // PUT: api/Actividades/Enlaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnlace(int id, EnlaceActividad enlaceActividad)
        {
            if (id != enlaceActividad.Id)
            {
                return BadRequest();
            }

            _context.Entry(enlaceActividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnlaceActividadExists(id))
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

        // POST: api/Actividades/Enlaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EnlaceActividad>> PostEnlace(EnlaceActividad enlaceActividad)
        {
            _context.EnlacesActividad.Add(enlaceActividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnlace", new { id = enlaceActividad.Id }, enlaceActividad);
        }

        // DELETE: api/Actividades/Enlaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnlace(int id)
        {
            var enlaceActividad = await _context.EnlacesActividad.FindAsync(id);
            if (enlaceActividad == null)
            {
                return NotFound();
            }

            _context.EnlacesActividad.Remove(enlaceActividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EnlaceActividadExists(int id)
        {
            return _context.EnlacesActividad.Any(e => e.Id == id);
        }

    }
}
