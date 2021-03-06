﻿using System;
using System.IO;
using EasyNetQ;
using Katil.Common.Utilities;
using Katil.Messages.Pdf.Events;

namespace Katil.Business.Services.IntegrationEvents.Pdf.EventHandling
{
    public class PdfDocumentGenrateIntegrationEventHandler
    {
        public PdfDocumentGenrateIntegrationEventHandler(IBus bus)
        {
            MessageBus = bus;
        }

        private IBus MessageBus { get; set; }

        public FileInfo GetPdfFromServiceAsync(string pdfServiceUrl, string htmlBody, string outputFile, string pageHeader, string pageFooter)
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

            var pdfFile = MessageBus.Request<PdfDocumentGenerateIntegrationEvent, PdfFileGeneratedIntegrationEvent>(pdfDocument);
            if (pdfFile != null)
            {
                var responce = pdfFile.FileContentBase64;
                var bytes = Convert.FromBase64String(responce);

                FileUtils.CheckIfNotExistsCreate(Path.GetDirectoryName(outputFile));
                File.WriteAllBytes(outputFile, bytes);
                return new FileInfo(outputFile);
            }

            return null;
        }
    }

    public class PaperTypes
    {
        public const string A0 = "A0";
        public const string A1 = "A1";
        public const string A2 = "A2";
        public const string A3 = "A3";
        public const string A4 = "A4";
        public const string A5 = "A5";
        public const string A6 = "A6";
        public const string A7 = "A7";
        public const string A8 = "A8";
        public const string A9 = "A9";
        public const string B0 = "B0";
        public const string B1 = "B1";
        public const string B10 = "B10";
        public const string B2 = "B2";
        public const string B3 = "B3";
        public const string B4 = "B4";
        public const string B5 = "B5";
        public const string B6 = "B6";
        public const string B7 = "B7";
        public const string B8 = "B8";
        public const string B9 = "B9";
        public const string C5E = "C5E";
        public const string Comm10E = "Comm10E";
        public const string DLE = "DLE";
        public const string Executive = "Executive";
        public const string Folio = "Folio";
        public const string Ledger = "Ledger";
        public const string Legal = "Legal";
        public const string Letter = "Letter";
        public const string Tabloid = "Tabloid";
    }
}