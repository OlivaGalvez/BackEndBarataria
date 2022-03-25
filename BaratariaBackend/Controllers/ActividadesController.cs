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
        public ActividadesController(ApplicationDbContext context)
        {
            _context = context;
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment) pathImagen = "C:\\repositorios\\imagenes\\";
        }

        // GET: api/Actividades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActividadVm>>> GetActividades()
        {
            if (isDevelopment)
            { 
            
            }
            List<ActividadVm> listVm = new List<ActividadVm>();
            List<Actividad> list = await _context.Actividades.Where(i=>i.Mostrar == true).ToListAsync();

            foreach (Actividad actividad in list)
            {

                byte[] imageArray = System.IO.File.ReadAllBytes(pathImagen + actividad.ImagenServidor);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                ActividadVm vm = new()
                {
                    Id = actividad.Id,
                    Titulo = actividad.Titulo,
                    Texto = actividad.Texto,
                    ImagenServidorBase64 = "data:image/png;base64," + base64ImageRepresentation
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
        public async Task<ActionResult<Actividad>> PostActividad([FromForm] string actividad, [FromForm] IFormFile file)
        {
            try
            {
                var folderName = "imagenes";
                var actividadModel = JsonConvert.DeserializeObject<Actividad>(actividad);

                if (file.Length > 0)
                {
                    var fullPath = Path.Combine(pathImagen, actividadModel.ImagenServidor);
                    var dbPath = Path.Combine(folderName, actividadModel.ImagenServidor);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    actividadModel.ImagenPeso = file.Length;
                    actividadModel.ImagenOriginal = file.FileName;
                }

                _context.Actividades.Add(actividadModel);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetActividad", new { id = actividadModel.Id }, actividadModel);
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
