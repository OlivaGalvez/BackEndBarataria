using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using BaratariaBackend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    //[Authorize(Policy = "Administrador")]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathImagen;
        private readonly string pathDoc;
        public ActividadesController(ApplicationDbContext context)
        {
            _context = context;
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                pathImagen = "C:\\repositorios\\imagenes\\";
                pathDoc = "C:\\repositorios\\documentos\\";
            }
        }

        // GET: api/Actividades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActividadVm>>> GetActividades()
        {
            List<ActividadVm> listVm = new List<ActividadVm>();
            List<Actividad> list = await _context.Actividades.Where(i=>i.Mostrar == true).ToListAsync();

            foreach (Actividad actividad in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                List<EnlaceActividad> listEnlaces = _context.EnlacesActividad.Where(i => i.ActividadId == actividad.Id).ToList();
                List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == actividad.Id).ToList();

                ActividadVm vm = new()
                {
                    Id = actividad.Id,
                    Titulo = actividad.Titulo,
                    Texto = actividad.Texto,
                    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                    ListEnlaces = listEnlaces,
                    ListDocumentos = listDocumentos
                };

                listVm.Add(vm);
            }

            return listVm;
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
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Actividad>> PostActividad([FromForm] string actividad, [FromForm] IFormFile imagen, [FromForm] string documentos)
        {
            try
            {
                Actividad act = null;
                var folderName = "imagenes";
                var actividadVewModel = JsonConvert.DeserializeObject<ActividadVm>(actividad);

                if (imagen.Length > 0)
                {
                    var fullPath = Path.Combine(pathImagen, actividadVewModel.ImagenServidor);
                    var dbPath = Path.Combine(folderName, actividadVewModel.ImagenServidor);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    act = new Actividad
                    {
                        Titulo = actividadVewModel.Titulo,
                        FechaAlta = actividadVewModel.FechaAlta,
                        FechaBaja = actividadVewModel.FechaBaja,
                        Mostrar = actividadVewModel.Mostrar,
                        Texto = actividadVewModel.Texto,
                        ImagenServidor = actividadVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName
                    };
                }

                _context.Actividades.Add(act);
                await _context.SaveChangesAsync();

                if (actividadVewModel.ListEnlaces != null)
                {
                    List<EnlaceActividad> listEnlaces = new List<EnlaceActividad>();
                    foreach (EnlaceActividad enlace in actividadVewModel.ListEnlaces)
                    {
                        EnlaceActividad en = new()
                        {
                            ActividadId = act.Id,
                            Nombre = enlace.Nombre,
                            Url = enlace.Url
                        };
                        listEnlaces.Add(en);
                    }
                    _context.EnlacesActividad.AddRange(listEnlaces);
                    await _context.SaveChangesAsync();
                }

                if (actividadVewModel.ListDocumentos != null)
                {
                    List<Documento> listDocumentos = new List<Documento>();
                    foreach (Documento documento in actividadVewModel.ListDocumentos)
                    {
                        Documento doc = new()
                        {
                            ActividadId = act.Id,
                            Nombre = documento.Nombre,
                            Original = documento.Original,
                            Servidor = documento.Servidor,
                            Fecha = DateTime.Now,
                            Url = pathDoc + documento.Servidor,
                            Tamanio = documento.Tamanio
                        };
                        listDocumentos.Add(doc);
                    }
                    _context.Documentos.AddRange(listDocumentos);
                    await _context.SaveChangesAsync();
                }

                return CreatedAtAction("GetActividad", new { id = act.Id }, act);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
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
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EnlaceActividad>>> GetEnlacesByActividad(int id)
        //{
        //    return await _context.EnlacesActividad.Where(t => t.ActividadId == id).ToListAsync();
        //}

        //// GET: api/Actividades/Enlaces/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<EnlaceActividad>> GetEnlace(int id)
        //{
        //    var enlaceActividad = await _context.EnlacesActividad.FindAsync(id);

        //    if (enlaceActividad == null)
        //    {
        //        return NotFound();
        //    }

        //    return enlaceActividad;
        //}

        //// PUT: api/Actividades/Enlaces/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEnlace(int id, EnlaceActividad enlaceActividad)
        //{
        //    if (id != enlaceActividad.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(enlaceActividad).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EnlaceActividadExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Actividades/Enlaces
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<EnlaceActividad>> PostEnlace(EnlaceActividad enlaceActividad)
        //{
        //    _context.EnlacesActividad.Add(enlaceActividad);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEnlace", new { id = enlaceActividad.Id }, enlaceActividad);
        //}

        //// DELETE: api/Actividades/Enlaces/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEnlace(int id)
        //{
        //    var enlaceActividad = await _context.EnlacesActividad.FindAsync(id);
        //    if (enlaceActividad == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.EnlacesActividad.Remove(enlaceActividad);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        //private bool EnlaceActividadExists(int id)
        //{
        //    return _context.EnlacesActividad.Any(e => e.Id == id);
        //}

    }
}
