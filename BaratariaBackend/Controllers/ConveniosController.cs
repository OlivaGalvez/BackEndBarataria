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
    //[Authorize(Policy = "Administrador")]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConveniosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly bool isDevelopment;
        private readonly string pathImagen;
        private readonly string pathDoc;
        public ConveniosController(ApplicationDbContext context)
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

        // GET: api/Convenios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConvenioVm>>> GetConvenios(bool portal = true)
        {
            List<ConvenioVm> listVm = new();
            List<Convenio> list = new();

            list = portal ? await _context.Convenios.Where(i => i.Mostrar == true).ToListAsync() : await _context.Convenios.ToListAsync();

            foreach (Convenio convenio in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + convenio.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                List<DireccionWeb> listEnlaces = _context.DireccionWebs.Where(i => i.ConvenioId == convenio.Id).ToList();
                List<Documento> listDocumentos = _context.Documentos.Where(i => i.ConvenioId == convenio.Id).ToList();

                ConvenioVm vm = new()
                {
                    Id = convenio.Id,
                    FechaAlta = convenio.FechaAlta,
                    Titulo = convenio.Titulo,
                    Texto = convenio.Texto,
                    Mostrar = convenio.Mostrar,
                    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                    ListEnlaces = listEnlaces,
                    ListDocumentos = listDocumentos
                };

                listVm.Add(vm);
            }

            return listVm.OrderByDescending(i => i.FechaAlta).ToList();
        }

        // GET: api/Convenios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConvenioVm>> GetConvenio(int id)
        {
            var convenio = await _context.Convenios.FindAsync(id);

            if (convenio == null)
            {
                return NotFound();
            }

            byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + convenio.ImagenServidor);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            List<DireccionWeb> listEnlaces = _context.DireccionWebs.Where(i => i.ConvenioId == convenio.Id).ToList();
            List<Documento> listDocumentos = _context.Documentos.Where(i => i.ConvenioId == convenio.Id).ToList();

            ConvenioVm vm = new()
            {
                Id = convenio.Id,
                FechaAlta = convenio.FechaAlta,
                Titulo = convenio.Titulo,
                Texto = convenio.Texto,
                Mostrar = convenio.Mostrar,
                ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation,
                ListEnlaces = listEnlaces,
                ListDocumentos = listDocumentos
            };

            return vm;
        }

        // PUT: api/Convenios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConvenio(int id, [FromForm] string convenio, [FromForm] IFormFile imagen)
        {
            var folderName = "imagenes";
            var convenioVewModel = JsonConvert.DeserializeObject<ConvenioVm>(convenio);
            if (convenioVewModel.Id != id)
            {
                return BadRequest();
            }

            Convenio conv = _context.Convenios.Find(id);
            if (conv == null)
            {
                return BadRequest();
            }

            if (imagen != null && imagen.Length > 0)
            {
                var fullPath = Path.Combine(pathImagen, convenioVewModel.ImagenServidor);
                var dbPath = Path.Combine(folderName, convenioVewModel.ImagenServidor);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
            }
            conv.Titulo = convenioVewModel.Titulo;
            conv.FechaAlta = convenioVewModel.FechaAlta;
            conv.Mostrar = convenioVewModel.Mostrar;
            conv.Texto = convenioVewModel.Texto;
            if (imagen != null)
            {
                conv.ImagenServidor = convenioVewModel.ImagenServidor;
                conv.ImagenPeso = imagen.Length;
                conv.ImagenOriginal = imagen.FileName;
            }


            _context.Entry(conv).State = EntityState.Modified;

            List<DireccionWeb> listActBorrado = _context.DireccionWebs.Where(i => i.ConvenioId == id).ToList();
            if (listActBorrado != null) _context.RemoveRange(listActBorrado);
            List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ConvenioId == id).ToList();
            if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);
            await _context.SaveChangesAsync();

            if (convenioVewModel.ListEnlaces.Count() > 0)
            {
                List<DireccionWeb> listEnlaces = new();
                foreach (DireccionWeb enlace in convenioVewModel.ListEnlaces)
                {
                    DireccionWeb en = new()
                    {
                        ConvenioId = conv.Id,
                        Nombre = enlace.Nombre,
                        Url = enlace.Url
                    };
                    listEnlaces.Add(en);
                }
                _context.DireccionWebs.AddRange(listEnlaces);
            }

            if (convenioVewModel.ListDocumentos.Count() > 0)
            {
                List<Documento> listDocumentos = new();
                foreach (Documento documento in convenioVewModel.ListDocumentos)
                {
                    Documento doc = new()
                    {
                        ConvenioId = conv.Id,
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
                if (!ConvenioExists(convenioVewModel.Id))
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

        // POST: api/Convenios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<Convenio>> PostConvenio([FromForm] string convenio, [FromForm] IFormFile imagen)
        {
            try
            {
                Convenio conv = null;
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

                    conv = new Convenio
                    {
                        Titulo = convenioVewModel.Titulo,
                        FechaAlta = convenioVewModel.FechaAlta,
                        Mostrar = convenioVewModel.Mostrar,
                        Texto = convenioVewModel.Texto,
                        ImagenServidor = convenioVewModel.ImagenServidor,
                        ImagenPeso = imagen.Length,
                        ImagenOriginal = imagen.FileName
                    };
                }

                _context.Convenios.Add(conv);
                await _context.SaveChangesAsync();

                if (convenioVewModel.ListEnlaces.Count() > 0)
                {
                    List<DireccionWeb> listEnlaces = new();
                    foreach (DireccionWeb enlace in convenioVewModel.ListEnlaces)
                    {
                        DireccionWeb en = new()
                        {
                            ConvenioId = conv.Id,
                            Nombre = enlace.Nombre,
                            Url = enlace.Url
                        };
                        listEnlaces.Add(en);
                    }
                    _context.DireccionWebs.AddRange(listEnlaces);
                    await _context.SaveChangesAsync();
                }

                if (convenioVewModel.ListDocumentos.Count() > 0)
                {
                    List<Documento> listDocumentos = new();
                    foreach (Documento documento in convenioVewModel.ListDocumentos)
                    {
                        Documento doc = new()
                        {
                            ConvenioId = conv.Id,
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

                return CreatedAtAction("GetConvenio", new { id = conv.Id }, conv);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // DELETE: api/Convenios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConvenio(int id)
        {
            try
            {
                var convenio = await _context.Convenios.FindAsync(id);
                if (convenio == null)
                {
                    return NotFound();
                }

                List<Documento> listDocBorrado = _context.Documentos.Where(i => i.ConvenioId == id).ToList();
                if (listDocBorrado != null) _context.RemoveRange(listDocBorrado);

                List<DireccionWeb> listEnlaces = _context.DireccionWebs.Where(i => i.ConvenioId == id).ToList();
                if (listEnlaces != null) _context.RemoveRange(listEnlaces);

                await _context.SaveChangesAsync();

                _context.Convenios.Remove(convenio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        private bool ConvenioExists(int id)
        {
            return _context.Convenios.Any(e => e.Id == id);
        }
    }
}
