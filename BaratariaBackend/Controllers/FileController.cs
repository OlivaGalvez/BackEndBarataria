using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BaratariaBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly bool isDevelopment;
        private readonly string pathImagen;

        public FileController() 
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
                        url = fullPath
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

        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> Download([FromQuery] string fileUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);
            if (!System.IO.File.Exists(filePath))
                return NotFound();
            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
