using System;

namespace Katil.Services.PdfConvertor.PdfService.Pdf
{
    public class PdfConvertException : Exception
    {
        public PdfConvertException(string msg)
            : base(msg)
        {
        }
    }
}
