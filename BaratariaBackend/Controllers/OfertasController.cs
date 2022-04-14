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
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathImagen;
        private readonly string pathDoc;
        public OfertasController(ApplicationDbContext context)
        {
            _context = context;
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                pathImagen = "C:\\repositorios\\imagenes\\";
                pathDoc = "C:\\repositorios\\documentos\\";
            }
            else
            {
                pathImagen = "/etc/repositorios/imagenes/";
                pathDoc = "/etc/repositorios/documentos/";
            }
        }

        // GET: api/Ofertas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertaVm>>> GetOfertas(bool portal = true)
        {
            try
            {
                List<OfertaVm> listVm = new();
                List<Oferta> list = new();

                if (portal == true)
                {
                    list = await _context.Ofertas.Where(i => i.Mostrar == true).OrderByDescending(i => i.FechaInicio).ToListAsync();
                }
                else
                {
                    list = await _context.Ofertas.OrderByDescending(i => i.FechaAlta).ToListAsync();
                }

                foreach (Oferta oferta in list)
                {

                    byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + oferta.ImagenServidor);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    List<Documento> listDocumentos = _context.Documentos.Where(i => i.OfertaId == oferta.Id).ToList();

                    OfertaVm vm = new()
                    {
                        Id = oferta.Id,
                        FechaAlta = oferta.FechaAlta,
                        FechaInicio = oferta.FechaInicio,
                        FechaFin = oferta.FechaFin,
                        Titulo = oferta.Titulo,
                        Mostrar = oferta.Mostrar,
                        ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                        ListDocumentos = listDocumentos
                    };

                    listVm.Add(vm);
                }
                return listVm;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // GET: api/Ofertas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfertaVm>> GetOferta(int id)
        {
            try
            {
                var oferta = await _context.Ofertas.FindAsync(id);

                if (oferta == null)
                {
                    return NotFound();
                }

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + oferta.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                List<Documento> listDocumentos = _context.Documentos.Where(i => i.OfertaId == oferta.Id).ToList();

                OfertaVm vm = new()
                {
                    Id = oferta.Id,
                    FechaAlta = oferta.FechaAlta,
                    FechaInicio = oferta.FechaInicio,
                    FechaFin = oferta.FechaFin,
                    Titulo = oferta.Titulo,
                    Mostrar = oferta.Mostrar,
                    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                    ListDocumentos = listDocumentos
                };

                return vm;
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }

        // PUT: api/Ofertas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOfertas(int id, [FromForm] string oferta, [FromForm] IFormFile imagen)
        {
            var folderName = "imagenes";
            var ofertaVewModel = JsonConvert.DeserializeObject<OfertaVm>(oferta);
            if (ofertaVewModel.Id != id)
            {
                return BadRequest();
            }

            Oferta ofer = _context.Ofertas.Find(id);
            if (ofer == null)
            {
                return BadRequest();
            }

            if (imagen != null && imagen.Length > 0)
            {
                var fullPath = Path.Combine(pathImagen, ofertaVewModel.ImagenServidor);
                var dbPath = Path.Combine(folderName, ofertaVewModel.ImagenServidor);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
            }
            ofer.Titulo = ofertaVewModel.Titulo;
            ofer.FechaAlta = ofertaVewModel.FechaAlta;
            ofer.FechaInicio = ofertaVewModel.FechaInicio;
            ofer.FechaFin = ofertaVewModel.FechaFin;
            ofer.Mostrar = ofertaVewModel.Mostrar;
           
            if (imagen != null)
            {
                ofer.ImagenServidor = ofertaVewModel.ImagenServidor;
                ofer.ImagenPeso = imagen.Length;
                ofer.ImagenOriginal = imagen.FileName;
            }
            _context.Entry(ofer).State = EntityState.Modified;
            
            List<Documento> listDocBorrado = _context.Documentos.Where(i => i.OfertaId == id).ToList();
            if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
            await _context.SaveChangesAsync();
          

            if (ofertaVewModel.ListDocumentos.Count() > 0)
            {
                List<Documento> listDocumentos = new();
                foreach (Documento documento in ofertaVewModel.ListDocumentos)
                {
                    Documento doc = new()
                    {
                        OfertaId = ofer.Id,
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
                if (!OfertaExists(ofertaVewModel.Id))
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

        // POST: api/Ofertas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Oferta>> PostOfertas([FromForm] string oferta, [FromForm] IFormFile imagen)
        {
            try
            {
                Oferta ofer = null;
                var folderName = "imagenes";
                var ofertaVewModel = JsonConvert.DeserializeObject<OfertaVm>(oferta);

                if (imagen.Length > 0)
                {
                    var fullPath = Path.Combine(pathImagen, ofertaVewModel.ImagenServidor);
                    var dbPath = Path.Combine(folderName, ofertaVewModel.ImagenServidor);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        imagen.CopyTo(stream);
                    }

                    ofer = new Oferta
                    {
                        Titulo = ofertaVewModel.Titulo,
                        FechaAlta = ofertaVewModel.FechaAlta,
                        FechaInicio = ofertaVewModel.FechaInicio,
                        FechaFin = ofertaVewModel.FechaFin,
                        Mostrar = ofertaVewModel.Mostrar,
                        ImagenServidor = ofertaVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName
                    };
                }

                _context.Ofertas.Add(ofer);
                await _context.SaveChangesAsync();
               

                if (ofertaVewModel.ListDocumentos.Count() > 0)
                {
                    List<Documento> listDocumentos = new();
                    foreach (Documento documento in ofertaVewModel.ListDocumentos)
                    {
                        Documento doc = new()
                        {
                            OfertaId = ofer.Id,
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

                return CreatedAtAction("GetOfertas", new { id = ofer.Id }, ofer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // DELETE: api/Ofertas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfertas(int id)
        {
            try
            {
                var oferta = await _context.Ofertas.FindAsync(id);
                if (oferta == null)
                {
                    return NotFound();
                }

                List<Documento> listDocBorrado = _context.Documentos.Where(i => i.OfertaId == id).ToList();
                if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
                await _context.SaveChangesAsync();

                _context.Ofertas.Remove(oferta);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private bool OfertaExists(int id)
        {
            return _context.Ofertas.Any(e => e.Id == id);
        }
    }
}
