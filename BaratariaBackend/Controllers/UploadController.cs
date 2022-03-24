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
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = "imagenes";
                var pathToSave = "C:\\tmp\\imagenes";
               
                if (file.Length > 0)
                {
                    string fileName = DateTime.Now.Day.ToString("00") +
                    DateTime.Now.Month.ToString("00") + DateTime.Now.Year + DateTime.Now.Hour.ToString("00") +
                    DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") +
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var result = new {
                        dbPath = dbPath,
                        fileName = fileName
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
