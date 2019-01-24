using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Katil.Messages.Pdf.Events;

namespace Katil.Services.PdfConvertor.PdfService.Pdf
{
    public class PdfConvert
    {
        private static PdfConvertEnvironment _environment;

        public static PdfConvertEnvironment Environment
        {
            get
            {
                if (_environment == null)
                {
                    _environment = new PdfConvertEnvironment
                    {
                        TempFolderPath = Path.GetTempPath(),
                        WkHtmlToPdfPath = GetWkHtmlToPdfExeLocation(),
                        Timeout = 60000
                    };
                }

                return _environment;
            }
        }

        public static void ConvertHtmlToPdf(PdfDocumentGenerateIntegrationEvent document, PdfOutput output)
        {
            ConvertHtmlToPdf(document, null, output);
        }

        public static void ConvertHtmlToPdf(PdfDocumentGenerateIntegrationEvent document, PdfConvertEnvironment environment, PdfOutput woutput)
        {
            if (environment == null)
            {
                environment = Environment;
            }

            if (document.Html != null)
            {
                document.Url = "-";
            }

            string outputPdfFilePath;
            bool delete;
            if (woutput.OutputFilePath != null)
            {
                outputPdfFilePath = woutput.OutputFilePath;
                delete = false;
            }
            else
            {
                outputPdfFilePath = Path.Combine(environment.TempFolderPath, string.Format("{0}.pdf", Guid.NewGuid()));
                delete = true;
            }

            StringBuilder paramsBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(document.PaperType))
            {
                document.PaperType = PaperTypes.A4;
            }

            paramsBuilder.AppendFormat("--page-size {0} ", document.PaperType);

            if (!string.IsNullOrEmpty(document.HeaderUrl))
            {
                paramsBuilder.AppendFormat("--header-html {0} ", document.HeaderUrl);
                paramsBuilder.Append("--margin-top 25 ");
                paramsBuilder.Append("--header-spacing 5 ");
            }

            if (!string.IsNullOrEmpty(document.FooterUrl))
            {
                paramsBuilder.AppendFormat("--footer-html {0} ", document.FooterUrl);
                paramsBuilder.Append("--margin-bottom 25 ");
                paramsBuilder.Append("--footer-spacing 5 ");
            }

            if (!string.IsNullOrEmpty(document.HeaderLeft))
            {
                paramsBuilder.AppendFormat("--header-left \"{0}\" ", document.HeaderLeft);
            }

            if (!string.IsNullOrEmpty(document.HeaderCenter))
            {
                paramsBuilder.AppendFormat("--header-center \"{0}\" ", document.HeaderCenter);
            }

            if (!string.IsNullOrEmpty(document.HeaderRight))
            {
                paramsBuilder.AppendFormat("--header-right \"{0}\" ", document.HeaderRight);
                paramsBuilder.Append("--margin-top 25 ");
                paramsBuilder.Append("--header-spacing 5 ");
            }

            if (!string.IsNullOrEmpty(document.FooterLeft))
            {
                paramsBuilder.AppendFormat("--footer-left \"{0}\" ", document.FooterLeft);
            }

            if (!string.IsNullOrEmpty(document.FooterCenter))
            {
                paramsBuilder.AppendFormat("--footer-center \"{0}\" ", document.FooterCenter);
            }

            if (!string.IsNullOrEmpty(document.FooterRight))
            {
                paramsBuilder.AppendFormat("--footer-right \"{0}\" ", document.FooterRight);
                paramsBuilder.Append("--margin-bottom 25 ");
                paramsBuilder.Append("--footer-spacing 5 ");
            }

            if (!string.IsNullOrEmpty(document.HeaderFontSize))
            {
                paramsBuilder.AppendFormat("--header-font-size \"{0}\" ", document.HeaderFontSize);
            }

            if (!string.IsNullOrEmpty(document.FooterFontSize))
            {
                paramsBuilder.AppendFormat("--footer-font-size \"{0}\" ", document.FooterFontSize);
            }

            if (!string.IsNullOrEmpty(document.HeaderFontName))
            {
                paramsBuilder.AppendFormat("--header-font-name \"{0}\" ", document.HeaderFontName);
            }

            if (!string.IsNullOrEmpty(document.FooterFontName))
            {
                paramsBuilder.AppendFormat("--footer-font-name \"{0}\" ", document.FooterFontName);
            }

            if (document.ExtraParams != null)
            {
                foreach (var extraParam in document.ExtraParams)
                {
                    paramsBuilder.AppendFormat("--{0} {1} ", extraParam.Key, extraParam.Value);
                }
            }

            if (document.Cookies != null)
            {
                foreach (var cookie in document.Cookies)
                {
                    paramsBuilder.AppendFormat("--cookie {0} {1} ", cookie.Key, cookie.Value);
                }
            }

            paramsBuilder.AppendFormat("\"{0}\" \"{1}\"", document.Url, outputPdfFilePath);

            try
            {
                StringBuilder output = new StringBuilder();
                StringBuilder error = new StringBuilder();

                using (Process process = new Process())
                {
                    process.StartInfo.FileName = environment.WkHtmlToPdfPath;
                    process.StartInfo.Arguments = paramsBuilder.ToString();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardInput = true;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        void outputHandler(object sender, DataReceivedEventArgs e)
                        {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(e.Data);
                            }
                        }

                        void errorHandler(object sender, DataReceivedEventArgs e)
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        }

                        process.OutputDataReceived += outputHandler;
                        process.ErrorDataReceived += errorHandler;

                        try
                        {
                            process.Start();

                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();

                            if (document.Html != null)
                            {
                                using (var stream = process.StandardInput)
                                {
                                    byte[] buffer = Encoding.UTF8.GetBytes(document.Html);
                                    stream.BaseStream.Write(buffer, 0, buffer.Length);
                                    stream.WriteLine();
                                }
                            }

                            if (process.WaitForExit(environment.Timeout) && outputWaitHandle.WaitOne(environment.Timeout) && errorWaitHandle.WaitOne(environment.Timeout))
                            {
                                if (process.ExitCode != 0 && !File.Exists(outputPdfFilePath))
                                {
                                    throw new PdfConvertException(string.Format("Html to PDF conversion of '{0}' failed. Wkhtmltopdf output: \r\n{1}", document.Url, error));
                                }
                            }
                            else
                            {
                                if (!process.HasExited)
                                {
                                    process.Kill();
                                }

                                throw new PdfConvertTimeoutException();
                            }
                        }
                        finally
                        {
                            process.OutputDataReceived -= outputHandler;
                            process.ErrorDataReceived -= errorHandler;
                        }
                    }
                }

                if (woutput.OutputStream != null)
                {
                    using (Stream fs = new FileStream(outputPdfFilePath, FileMode.Open))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int read;

                        while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            woutput.OutputStream.Write(buffer, 0, read);
                        }
                    }
                }

                if (woutput.OutputCallback != null)
                {
                    byte[] pdfFileBytes = File.ReadAllBytes(outputPdfFilePath);
                    woutput.OutputCallback(document, pdfFileBytes);
                }
            }
            finally
            {
                if (delete && File.Exists(outputPdfFilePath))
                {
                    File.Delete(outputPdfFilePath);
                }
            }
        }

        private static string GetWkHtmlToPdfExeLocation()
        {
            return @"wkhtmltopdf";
        }
    }
}
