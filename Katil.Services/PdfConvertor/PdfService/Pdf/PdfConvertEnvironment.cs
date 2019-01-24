using System;

namespace Katil.Services.PdfConvertor.PdfService.Pdf
{
    public class PdfConvertEnvironment
    {
        public string TempFolderPath { get; set; }

        public string WkHtmlToPdfPath { get; set; }

        public int Timeout { get; set; }

        public bool Debug { get; set; }
    }
}
