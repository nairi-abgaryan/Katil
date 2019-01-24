using System;
using System.Collections.Generic;
using Katil.Messages.Pdf.Events;

namespace Katil.Generators.Entities
{
    public class PdfDocument
    {
        public string PaperType { get; set; }

        public string Url { get; set; }

        public string Html { get; set; }

        public string HeaderUrl { get; set; }

        public string FooterUrl { get; set; }

        public string HeaderLeft { get; set; }

        public string HeaderCenter { get; set; }

        public string HeaderRight { get; set; }

        public string FooterLeft { get; set; }

        public string FooterCenter { get; set; }

        public string FooterRight { get; set; }

        public object State { get; set; }

        public Dictionary<string, string> Cookies { get; set; }

        public Dictionary<string, string> ExtraParams { get; set; }

        public string HeaderFontSize { get; set; }

        public string FooterFontSize { get; set; }

        public string HeaderFontName { get; set; }

        public string FooterFontName { get; set; }

        public PdfDocumentGenerateIntegrationEvent Convert()
        {
            return new PdfDocumentGenerateIntegrationEvent
            {
                Html = this.Html,
                HeaderRight = this.HeaderRight,
                FooterRight = this.FooterRight
            };
        }
    }
}
