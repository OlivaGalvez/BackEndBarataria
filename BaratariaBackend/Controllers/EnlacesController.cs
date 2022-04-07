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
    public class EnlacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathImagen;

        public EnlacesController(ApplicationDbContext context)
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

        // GET: api/Enlaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnlaceVm>>> GetEnlaces(bool portal = true)
        {
            List<EnlaceVm> listVm = new();
            List<Enlace> list = new();

            if (portal == true)
            {
                list = await _context.Enlaces.Where(i => i.Mostrar == true).OrderByDescending(i => i.FechaAlta).ToListAsync();
            }
            else
            {
                list = await _context.Enlaces.OrderByDescending(i => i.FechaAlta).ToListAsync();
            }

            foreach (Enlace convenio in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + convenio.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                EnlaceVm vm = new()
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

        // GET: api/Enlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnlaceVm>> GetEnlaces(int id)
        {
            var enlace = await _context.Enlaces.FindAsync(id);

            if (enlace == null)
            {
                return NotFound();
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + enlace.ImagenServidor);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            EnlaceVm vm = new()
            {
                Id = enlace.Id,
                FechaAlta = enlace.FechaAlta,
                Titulo = enlace.Titulo,
                Mostrar = enlace.Mostrar,
                ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                Url = enlace.Url
            };

            return vm;
        }

        // PUT: api/Enlaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnlaces(int id, [FromForm] string enlace, [FromForm] IFormFile imagen)
        {
            var folderName = "imagenes";
            var enlaceVewModel = JsonConvert.DeserializeObject<EnlaceVm>(enlace);
            if (enlaceVewModel.Id != id)
            {
                return BadRequest();
            }

            Enlace enl = _context.Enlaces.Find(id);
            if (enl == null)
            {
                return BadRequest();
            }

            if (imagen != null && imagen.Length > 0)
            {
                var fullPath = Path.Combine(pathImagen, enlaceVewModel.ImagenServidor);
                var dbPath = Path.Combine(folderName, enlaceVewModel.ImagenServidor);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
            }
            enl.Titulo = enlaceVewModel.Titulo;
            enl.FechaAlta = enlaceVewModel.FechaAlta;
            enl.Mostrar = enlaceVewModel.Mostrar;
            enl.Url = enlaceVewModel.Url;
            if (imagen != null)
            {
                enl.ImagenServidor = enlaceVewModel.ImagenServidor;
                enl.ImagenPeso = imagen.Length;
                enl.ImagenOriginal = imagen.FileName;
            }

            _context.Entry(enl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnlacesExists(enlaceVewModel.Id))
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

        // POST: api/Enlaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Enlace>> PostEnlaces([FromForm] string enlace, [FromForm] IFormFile imagen)
        {
            try
            {
                Enlace con = null;
                var folderName = "imagenes";
                var enlaceVewModel = JsonConvert.DeserializeObject<EnlaceVm>(enlace);

                if (imagen.Length > 0)
                {
                    var fullPath = Path.Combine(pathImagen, enlaceVewModel.ImagenServidor);
                    var dbPath = Path.Combine(folderName, enlaceVewModel.ImagenServidor);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    con = new Enlace
                    {
                        Titulo = enlaceVewModel.Titulo,
                        FechaAlta = enlaceVewModel.FechaAlta,
                        Mostrar = enlaceVewModel.Mostrar,
                        ImagenServidor = enlaceVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName,
                        Url = enlaceVewModel.Url
                    };
                }

                _context.Enlaces.Add(con);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEnlaces", new { id = con.Id }, con);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // DELETE: api/Enlaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnlaces(int id)
        {
            var enlace = await _context.Enlaces.FindAsync(id);
            if (enlace == null)
            {
                return NotFound();
            }

            _context.Enlaces.Remove(enlace);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnlacesExists(int id)
        {
            return _context.Enlaces.Any(e => e.Id == id);
        }
       
    }
}
