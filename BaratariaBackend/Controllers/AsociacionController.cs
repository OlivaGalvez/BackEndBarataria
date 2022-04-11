using BaratariaBackend.Models.Context;
using BaratariaBackend.Models.Entities;
using BaratariaBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<AsociacionVm>> GetAsociacion()
        {
            try
            {
                var asociacion = await _context.Asociacions.OrderByDescending(i => i.Id).FirstAsync();

                if (asociacion == null)
                {
                    return NotFound();
                }

                List<Documento> listDocumentos = _context.Documentos.Where(i => i.ActividadId == asociacion.Id).ToList();

                AsociacionVm vm = new()
                {
                    Id = asociacion.Id,
                    FechaAlta = asociacion.FechaAlta,
                    Titulo = asociacion.Titulo,
                    Texto = asociacion.Texto,
                    ListDocumentos = listDocumentos
                };

                return vm;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

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

        private bool AsociacionExists(int id)
        {
            return _context.Asociacions.Any(e => e.Id == id);
        }
    }
}
