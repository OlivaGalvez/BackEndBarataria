using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using BaratariaBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsociacionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathDoc;
        public AsociacionController(ApplicationDbContext context)
        {
            _context = context;
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                pathDoc = "C:\\repositorios\\documentos\\";
            }
            else
            {
                pathDoc = "/etc/repositorios/documentos/";
            }
        }

        // GET: api/Asociacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asociacion>>> GetAsociaciones(bool portal = true)
        {
            try
            {
                List<Asociacion> list = new();

                //if (portal == true)
                //{
                //    list = await _context.Actividades.Where(i => i.Mostrar == true).OrderByDescending(i => i.FechaInicio).ToListAsync();
                //}
                //else
                //{
                //    list = await _context.Actividades.OrderByDescending(i => i.FechaAlta).ToListAsync();
                //}

                //foreach (Actividad actividad in list)
                //{

                //    byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
                //    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                //    List<DireccionWeb> listEnlaces = _context.DireccionWebs.Where(i => i.ActividadId == actividad.Id).ToList();
                //    List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == actividad.Id).ToList();

                //    ActividadVm vm = new()
                //    {
                //        Id = actividad.Id,
                //        FechaAlta = actividad.FechaAlta,
                //        FechaInicio = actividad.FechaInicio,
                //        FechaFin = actividad.FechaFin,
                //        Titulo = actividad.Titulo,
                //        Texto = actividad.Texto,
                //        Mostrar = actividad.Mostrar,
                //        ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                //        ListEnlaces = listEnlaces,
                //        ListDocumentos = listDocumentos
                //    };

                //    listVm.Add(vm);
                //}
                return list;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // GET: api/Asociacion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asociacion>> GetAsociacion(int id)
        {
            try
            {
                //var actividad = await _context.Actividades.FindAsync(id);

                //if (actividad == null)
                //{
                //    return NotFound();
                //}

                //byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
                //string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                //List<DireccionWeb> listEnlaces = _context.DireccionWebs.Where(i => i.ActividadId == actividad.Id).ToList();
                //List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == actividad.Id).ToList();

                //ActividadVm vm = new()
                //{
                //    Id = actividad.Id,
                //    FechaAlta = actividad.FechaAlta,
                //    FechaInicio = actividad.FechaInicio,
                //    FechaFin = actividad.FechaFin,
                //    Titulo = actividad.Titulo,
                //    Texto = actividad.Texto,
                //    Mostrar = actividad.Mostrar,
                //    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                //    ListEnlaces = listEnlaces,
                //    ListDocumentos = listDocumentos
                //};

                //return vm;
                return null;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }

        // PUT: api/Asociacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsociacion(int id, [FromForm] string asociacion)
        {
            //var folderName = "imagenes";
            //var actividadVewModel = JsonConvert.DeserializeObject<ActividadVm>(actividad);
            //if (actividadVewModel.Id != id)
            //{
            //    return BadRequest();
            //}

            //Actividad act = _context.Actividades.Find(id);
            //if (act == null)
            //{
            //    return BadRequest();
            //}

            //if (imagen != null && imagen.Length > 0)
            //{
            //    var fullPath = Path.Combine(pathImagen, actividadVewModel.ImagenServidor);
            //    var dbPath = Path.Combine(folderName, actividadVewModel.ImagenServidor);
            //    using (var stream = new FileStream(fullPath, FileMode.Create))
            //    {
            //        imagen.CopyTo(stream);
            //    }
            //}
            //act.Titulo = actividadVewModel.Titulo;
            //act.FechaAlta = actividadVewModel.FechaAlta;
            //act.FechaInicio = actividadVewModel.FechaInicio;
            //act.FechaFin = actividadVewModel.FechaFin;
            //act.Mostrar = actividadVewModel.Mostrar;
            //act.Texto = actividadVewModel.Texto;
            //if (imagen != null)
            //{
            //    act.ImagenServidor = actividadVewModel.ImagenServidor;
            //    act.ImagenPeso = imagen.Length;
            //    act.ImagenOriginal = imagen.FileName;
            //}


            //_context.Entry(act).State = EntityState.Modified;

            //List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ActividadId == id).ToList();
            //if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
            //await _context.SaveChangesAsync();

            //if (actividadVewModel.ListDocumentos.Count() > 0)
            //{
            //    List<Documento> listDocumentos = new();
            //    foreach (Documento documento in actividadVewModel.ListDocumentos)
            //    {
            //        Documento doc = new()
            //        {
            //            ActividadId = act.Id,
            //            Nombre = documento.Nombre,
            //            Original = documento.Original,
            //            Servidor = documento.Servidor,
            //            Fecha = DateTime.Now,
            //            Url = pathDoc + documento.Servidor,
            //            Tamanio = documento.Tamanio
            //        };
            //        listDocumentos.Add(doc);
            //    }
            //    _context.Documentos.AddRange(listDocumentos);
            //    await _context.SaveChangesAsync();
            //}

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ActividadExists(actividadVewModel.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Asociacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Actividad>> PostAsociacion([FromForm] string asociacion)
        {
            try
            {
                Asociacion asoc = null;
                var asociacionVewModel = JsonConvert.DeserializeObject<AsociacionVm>(asociacion);

                asoc = new Asociacion
                {
                    Titulo = asociacionVewModel.Titulo,
                    FechaAlta = asociacionVewModel.FechaAlta,
                    Texto = asociacionVewModel.Texto
                };

                _context.Asociacions.Add(asoc);
                await _context.SaveChangesAsync();

                if (asociacionVewModel.ListDocumentos.Count() > 0)
                {
                    List<Documento> listDocumentos = new();
                    foreach (Documento documento in asociacionVewModel.ListDocumentos)
                    {
                        Documento doc = new()
                        {
                            AsociacionId = asoc.Id,
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

                return CreatedAtAction("GetAsociacion", new { id = asoc.Id }, asoc);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // DELETE: api/Asociacion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsociacion(int id)
        {
            try
            {
                var asociacion = await _context.Asociacions.FindAsync(id);
                if (asociacion == null)
                {
                    return NotFound();
                }

                List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ActividadId == id).ToList();
                if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);

                await _context.SaveChangesAsync();

                _context.Asociacions.Remove(asociacion);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private bool AsociacionExists(int id)
        {
            return _context.Asociacions.Any(e => e.Id == id);
        }
    }
}
