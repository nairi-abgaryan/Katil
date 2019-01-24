using System;
using System.IO;
using Katil.Generators.Entities;
using Katil.Services.PdfConvertor.PdfService.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace Katil.Generators.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfGeneratorController : ControllerBase
    {
        [HttpGet]
        public ActionResult<bool> Get()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] PdfDocument document)
        {
            PdfOutput pdfOutput = new PdfOutput()
            {
                OutputStream = new MemoryStream()
            };

            PdfConvert.ConvertHtmlToPdf(document.Convert(), pdfOutput);

            var bytes = ((MemoryStream)pdfOutput.OutputStream).ToArray();

            if (bytes != null)
            {
                pdfOutput.OutputStream.Close();

                return Ok(Convert.ToBase64String(bytes));
            }

            return StatusCode(404);
        }
    }
}
