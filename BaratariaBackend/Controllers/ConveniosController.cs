using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using BaratariaBackend.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConveniosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathImagen;

        public ConveniosController(ApplicationDbContext context)
        {
            _context = context;
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                pathImagen = "C:\\repositorios\\imagenes\\";
            }
            else
            {
                pathImagen = "/etc/repositorios/imagenes/";
            }
        }

        // GET: api/Convenioss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConvenioVm>>> GetConvenios(bool portal = true)
        {
            List<ConvenioVm> listVm = new();
            List<Convenio> list = new();

            if (portal == true)
            {
                list = await _context.Convenios.Where(i => i.Mostrar == true).OrderByDescending(i => i.FechaAlta).ToListAsync();
            }
            else
            {
                list = await _context.Convenios.OrderByDescending(i => i.FechaAlta).ToListAsync();
            }

            foreach (Convenio convenio in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + convenio.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                ConvenioVm vm = new()
                {
                    Id = convenio.Id,
                    FechaAlta = convenio.FechaAlta,
                    Titulo = convenio.Titulo,
                    Mostrar = convenio.Mostrar,
                    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                    Url = convenio.Url
                };

                listVm.Add(vm);
            }

            return listVm;
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
        public async Task<ActionResult<Convenio>> PostConvenio([FromForm] string convenio, [FromForm] IFormFile imagen)
        {
            try
            {
                Convenio con = null;
                var folderName = "imagenes";
                var convenioVewModel = JsonConvert.DeserializeObject<ConvenioVm>(convenio);

                if (imagen.Length > 0)
                {
                    var fullPath = Path.Combine(pathImagen, convenioVewModel.ImagenServidor);
                    var dbPath = Path.Combine(folderName, convenioVewModel.ImagenServidor);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    con = new Convenio
                    {
                        Titulo = convenioVewModel.Titulo,
                        FechaAlta = convenioVewModel.FechaAlta,
                        Mostrar = convenioVewModel.Mostrar,
                        ImagenServidor = convenioVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName,
                        Url = convenioVewModel.Url
                    };
                }

                _context.Convenios.Add(con);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetConvenios", new { id = con.Id }, con);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
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
