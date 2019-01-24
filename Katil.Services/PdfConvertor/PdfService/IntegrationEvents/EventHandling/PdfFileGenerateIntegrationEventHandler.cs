using System;
using System.IO;
using Katil.Messages.Pdf.Events;
using Katil.Services.PdfConvertor.PdfService.Pdf;

namespace Katil.Services.PdfConvertor.PdfService.IntegrationEvents.EventHandling
{
    public class PdfFileGenerateIntegrationEventHandler
    {
        public static PdfFileGeneratedIntegrationEvent ConsumeAsync(PdfDocumentGenerateIntegrationEvent document)
        {
            Console.WriteLine("Pdf File Generate Integration Event Received: {0}", document.DisputeGuid);

            PdfOutput pdfOutput = new PdfOutput()
            {
                OutputStream = new MemoryStream()
            };

            PdfConvert.ConvertHtmlToPdf(document, pdfOutput);

            var bytes = ((MemoryStream)pdfOutput.OutputStream).ToArray();

            if (bytes != null)
            {
                pdfOutput.OutputStream.Close();

                var base64File = Convert.ToBase64String(bytes);

                var pdfFile = new PdfFileGeneratedIntegrationEvent
                {
                    CreatedDateTime = document.CreatedDateTime,
                    DisputeGuid = document.DisputeGuid,
                    FileContentBase64 = base64File
                };

                return pdfFile;
            }

            return null;
        }
    }
}
