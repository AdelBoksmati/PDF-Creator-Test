using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDF_Creator_Test.Models;
using PDF_Creator_Test.Documents;
using QuestPDF.Fluent;
using System.Diagnostics;


namespace PDF_Creator_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        private readonly ILogger<PlaneController> _logger;

        public PlaneController(ILogger<PlaneController> logger)
        {
            _logger = logger;
        }
        [HttpPost("CheckDetails")]
        public async Task<IActionResult> Details([FromBody] Plane plane)
        {
            // Validate the model
            if (plane == null ||
                        string.IsNullOrEmpty(plane.Manufacturer) ||
                        string.IsNullOrEmpty(plane.Model) ||
                        plane.MaxSpeed <= 0 ||
                        plane.MaxAltitude <= 0 ||
                        plane.MaxRange <= 0 ||
                        plane.Wingspan <= 0 ||
                        plane.Length <= 0 ||
                        plane.Weight <= 0 ||
                        string.IsNullOrEmpty(plane.Picture)

                        )

            {
                return BadRequest("Missing required fields or invalid data");
            }
            try
            {
                var filePath = "PlaneDocument.pdf";

                // This creates the PlaneDocument, passing the plane object that came through the validation
                var document = new PlaneDocument(plane);
                document.GeneratePdf(filePath);

                // Read the PDF file as a byte array
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                // Delete the PDF file
                System.IO.File.Delete(filePath);

                // Return the PDF file as a FileContentResult
                return File(fileBytes, "application/pdf", "PlaneDocument.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }


        }

    }
}
