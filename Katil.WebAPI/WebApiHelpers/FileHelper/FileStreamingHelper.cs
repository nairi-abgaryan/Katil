using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

#pragma warning disable SA1009 // Closing parenthesis must be spaced correctly

namespace Katil.WebAPI.WebApiHelpers.FileHelper
{
    public static class FileStreamingHelper
    {
        private static readonly int BondaryLengthLimit = 1024;

        private static readonly int BufferSize = 81920;

        public static async Task<IActionResult> GetFile(string filePath, string fileMimeType)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return new FileStreamResult(memory, fileMimeType);
        }

        public static async Task<(T model, List<LocalMultipartFileInfo> file)>
            ParseRequestForm<T>(
            this Controller controller, string tempFolder, T model)
            where T : class
        {
            (var forms, var files) = await ParseRequest(controller.Request, tempFolder);
            await UpdateAndValidateForm(controller, model, forms);
            return (model, files);
        }

        public static async Task<T> ParseRequestForm<T>(
            this Controller controller,
            Func<MultipartSection, Task> fileHandler,
            T model)
            where T : class
        {
            var forms = await ParseRequest(controller.Request, fileHandler);
            await UpdateAndValidateForm(controller, model, forms);
            return model;
        }

        public static async Task<(Dictionary<string, StringValues> forms, List<LocalMultipartFileInfo> files)> ParseRequest(HttpRequest request, string tempFolder)
        {
            if (tempFolder == null)
            {
                throw new ApplicationException("Request is not a multipart request");
            }

            return await ParseRequest(request, tempFolder, null);
        }

        public static async Task<Dictionary<string, StringValues>>
                ParseRequest(HttpRequest request, Func<MultipartSection, Task> fileHandler)
        {
            return (await ParseRequest(request, string.Empty, fileHandler)).Item1;
        }

        private static async Task UpdateAndValidateForm<T>(Controller controller, T model, Dictionary<string, StringValues> forms)
            where T : class
        {
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(forms),
                CultureInfo.CurrentCulture);

            var bindingSuccessful = await controller.TryUpdateModelAsync(
                model,
                prefix: string.Empty,
                valueProvider: formValueProvider);

            controller.TryValidateModel(model);
        }

        private static async Task<(Dictionary<string, StringValues>, List<LocalMultipartFileInfo>)>
            ParseRequest(HttpRequest request, string tempLoc, Func<MultipartSection, Task> fileHandler = null)
        {
            var files = new List<LocalMultipartFileInfo>();

            if (fileHandler == null)
            {
                fileHandler = HandleFileSection;
            }

            if (!MultipartRequestHelper.IsMultipartContentType(request.ContentType))
            {
                throw new InvalidDataException("Request is not a multipart request");
            }

            var formAccumulator = default(KeyValueAccumulator);

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(request.ContentType),
                BondaryLengthLimit);

            var reader = new MultipartReader(boundary, request.Body);

            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition,
                        out ContentDispositionHeaderValue contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        await fileHandler(section);
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        formAccumulator = await AccumulateForm(formAccumulator, section, contentDisposition);
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return (formAccumulator.GetResults(), files);

            async Task HandleFileSection(MultipartSection fileSection)
            {
                string targetFilePath;
                var guid = Guid.NewGuid();

                targetFilePath = Path.Combine(tempLoc, guid.ToString());

                using (var targetStream = File.Create(targetFilePath))
                {
                    await fileSection.Body.CopyToAsync(targetStream);
                    targetStream.Position = 0;
                }

                if (fileSection.Body.Length == 0)
                {
                    throw new InvalidDataException("Trying to upload empty file");
                }

                var formFile = new LocalMultipartFileInfo
                {
                    Name = fileSection.AsFileSection().FileName,
                    FileName = fileSection.AsFileSection().Name,
                    OriginalFileName = fileSection.AsFileSection().FileName,
                    ContentType = fileSection.ContentType,
                    Length = fileSection.Body.Length,
                    TemporaryLocation = targetFilePath,
                    ////AddedBy = int.Parse(fileSection.Headers["AddedBy"])
                };

                files.Add(formFile);
            }
        }

        private static async Task<KeyValueAccumulator> AccumulateForm(
            KeyValueAccumulator formAccumulator,
            MultipartSection section,
            ContentDispositionHeaderValue contentDisposition)
        {
            var key = MultipartRequestHelper.RemoveQuotes(contentDisposition.Name.Value);
            var encoding = MultipartRequestHelper.GetEncoding(section);
            using (var streamReader = new StreamReader(
                section.Body,
                encoding,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: BufferSize,
                leaveOpen: true))
            {
                var value = await streamReader.ReadToEndAsync();
                if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                {
                    value = string.Empty;
                }

                formAccumulator.Append(key, value);

                if (formAccumulator.ValueCount > FormReader.DefaultValueCountLimit)
                {
                    throw new InvalidDataException(
                        $"Form key count limit {FormReader.DefaultValueCountLimit} exceeded.");
                }
            }

            return formAccumulator;
        }
    }

#pragma warning restore SA1009 // Closing parenthesis must be spaced correctly

    public class MultipartFileInfo
    {
        public long Length { get; set; }

        public string FileName { get; set; }

        public string OriginalFileName { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }
    }

    public class LocalMultipartFileInfo : MultipartFileInfo
    {
        public string TemporaryLocation { get; set; }

        public int AddedBy { get; set; }
    }
}