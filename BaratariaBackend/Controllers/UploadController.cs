using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : Controller
    {
        private readonly bool isDevelopment;
        private readonly string pathImagen;

        public UploadController() 
        {
            isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                pathImagen = "C:\\repositorios\\";
            }
            else {
                pathImagen = "/etc/repositorios/";
            }
            
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string carpeta = "imagenes")
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = carpeta;
               
                if (file.Length > 0)
                {
                    string fileName = DateTime.Now.Day.ToString("00") +
                    DateTime.Now.Month.ToString("00") + DateTime.Now.Year + DateTime.Now.Hour.ToString("00") +
                    DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") +
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var fullPath = Path.Combine(pathImagen + carpeta, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var result = new {
                        dbPath = dbPath,
                        fileName = fileName,
                        fullPath = fullPath
                    };

                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
