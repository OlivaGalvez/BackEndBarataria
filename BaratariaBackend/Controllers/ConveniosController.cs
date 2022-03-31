using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConveniosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConveniosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Convenioss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Convenio>>> GetConvenios()
        {
            return await _context.Convenios.ToListAsync();
        }

        // GET: api/Convenioss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Convenio>> GetConvenio(int id)
        {
            var actividad = await _context.Convenios.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // PUT: api/Convenioss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConvenio(int id, Convenio actividad)
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
                if (!ConveniosExists(id))
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

        // POST: api/Convenioss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Convenio>> PostConvenio(Convenio actividad)
        {
            _context.Convenios.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConvenios", new { id = actividad.Id }, actividad);
        }

        // DELETE: api/Convenioss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConvenio(int id)
        {
            var actividad = await _context.Convenios.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.Convenios.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConveniosExists(int id)
        {
            return _context.Convenios.Any(e => e.Id == id);
        }
       
    }
}
