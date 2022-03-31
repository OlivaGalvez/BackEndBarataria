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
        public async Task<ActionResult<IEnumerable<ActividadVm>>> GetActividades(bool portal = true)
        {
            List<ActividadVm> listVm = new();
            List<Actividad> list = new();

            if (portal == true)
            {
                list = await _context.Actividades.Where(i => i.Mostrar == true).OrderByDescending(i=>i.FechaAlta).ToListAsync();
            }
            else {
                list = await _context.Actividades.OrderByDescending(i => i.FechaAlta).ToListAsync();
            }

            foreach (Actividad actividad in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                List<EnlaceActividad> listEnlaces = _context.EnlacesActividad.Where(i => i.ActividadId == actividad.Id).ToList();
                List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == actividad.Id).ToList();

                ActividadVm vm = new()
                {
                    Id = actividad.Id,
                    FechaAlta = actividad.FechaAlta,
                   // FechaBaja = actividad.FechaBaja,
                    Titulo = actividad.Titulo,
                    Texto = actividad.Texto,
                    Mostrar = actividad.Mostrar,
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
        public async Task<ActionResult<ActividadVm>> GetActividad(int id)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            List<EnlaceActividad> listEnlaces = _context.EnlacesActividad.Where(i => i.ActividadId == actividad.Id).ToList();
            List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == actividad.Id).ToList();

            ActividadVm vm = new()
            {
                Id = actividad.Id,
                FechaAlta = actividad.FechaAlta,
                //FechaBaja = actividad.FechaBaja,
                Titulo = actividad.Titulo,
                Texto = actividad.Texto,
                Mostrar = actividad.Mostrar,
                ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                ListEnlaces = listEnlaces,
                ListDocumentos = listDocumentos
            };

            return vm;
        }

        // PUT: api/Actividades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, [FromForm] string actividad, [FromForm] IFormFile imagen, [FromForm] string documentos)
        {
            var folderName = "imagenes";
            var actividadVewModel = JsonConvert.DeserializeObject<ActividadVm>(actividad);
            if (actividadVewModel.Id != id)
            {
                return BadRequest();
            }

            Actividad act = _context.Actividades.Find(id);
            if (act == null)
            {
                return BadRequest();
            }

            if (imagen != null && imagen.Length > 0)
            {
                var fullPath = Path.Combine(pathImagen, actividadVewModel.ImagenServidor);
                var dbPath = Path.Combine(folderName, actividadVewModel.ImagenServidor);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
            }
            act.Titulo = actividadVewModel.Titulo;
            act.FechaAlta = actividadVewModel.FechaAlta;
           // act.FechaBaja = actividadVewModel.FechaBaja;
            act.Mostrar = actividadVewModel.Mostrar;
            act.Texto = actividadVewModel.Texto;
            if (imagen != null)
            {
                act.ImagenServidor = actividadVewModel.ImagenServidor;
                act.ImagenPeso = imagen.Length;
                act.ImagenOriginal = imagen.FileName;
            }
          

            _context.Entry(act).State = EntityState.Modified;

            List<EnlaceActividad> listActBorrado = _context.EnlacesActividad.Where(i => i.ActividadId == id).ToList();
            if (listActBorrado != null) _context.RemoveRange(listActBorrado);
            List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ActividadId == id).ToList();
            if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
            await _context.SaveChangesAsync();

            if (actividadVewModel.ListEnlaces.Count() > 0)
            {
                List<EnlaceActividad> listEnlaces = new();
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
            }

            if (actividadVewModel.ListDocumentos.Count() > 0)
            {
                List<Documento> listDocumentos = new();
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(actividadVewModel.Id))
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
                      //  FechaBaja = actividadVewModel.FechaBaja,
                        Mostrar = actividadVewModel.Mostrar,
                        Texto = actividadVewModel.Texto,
                        ImagenServidor = actividadVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName
                    };
                }

                _context.Actividades.Add(act);
                await _context.SaveChangesAsync();

                if (actividadVewModel.ListEnlaces.Count() > 0)
                {
                    List<EnlaceActividad> listEnlaces = new ();
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

                if (actividadVewModel.ListDocumentos.Count() > 0)
                {
                    List<Documento> listDocumentos = new ();
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

            List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ActividadId == id).ToList();
            if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
            await _context.SaveChangesAsync();

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return _context.Actividades.Any(e => e.Id == id);
        }
       
    }
}
