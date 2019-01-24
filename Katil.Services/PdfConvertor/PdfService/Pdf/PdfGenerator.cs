using System;
using System.IO;
using Katil.Messages.Pdf.Events;

namespace Katil.Services.PdfConvertor.PdfService.Pdf
{
    public class PdfGenerator
    {
        public static FileInfo GenerateFromHtmlString(string htmlBody, string outputFile, string pageHeader, string pageFooter, string disputeNumber, DateTime date)
        {
            var pdfDocument = new PdfDocumentGenerateIntegrationEvent
            {
                Html = htmlBody,
                PaperType = PaperTypes.A4,
                FooterFontName = "Arial",
                HeaderFontName = "Arial",
                HeaderRight = pageHeader,
                FooterRight = pageFooter,
                FooterFontSize = "10",
                HeaderFontSize = "10"
            };

            PdfOutput pdfOutput = new PdfOutput
            {
                OutputFilePath = outputFile
            };

            PdfConvert.ConvertHtmlToPdf(pdfDocument, pdfOutput);

            return new FileInfo(pdfOutput.OutputFilePath);
        }
    }
}
