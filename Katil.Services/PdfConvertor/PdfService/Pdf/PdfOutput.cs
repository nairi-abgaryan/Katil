using System;
using System.IO;
using Katil.Messages.Pdf.Events;

namespace Katil.Services.PdfConvertor.PdfService.Pdf
{
    public class PdfOutput
    {
        public string OutputFilePath { get; set; }

        public Stream OutputStream { get; set; }

        public Action<PdfDocumentGenerateIntegrationEvent, byte[]> OutputCallback { get; set; }
    }
}
