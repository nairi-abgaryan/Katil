using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Katil.WebAPI.WebApiHelpers.FileHelper
{
    public static class MultipartRequestHelper
    {
        private const string BOUNDARY = "boundary";

        public static string RemoveQuotes(string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length >= 2 && input[0] == '"' && input[input.Length - 1] == '"')
            {
                input = input.Substring(1, input.Length - 2);
            }

            return input;
        }

        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = RemoveQuotes(contentType.Boundary());
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(
                section.ContentType,
                out MediaTypeHeaderValue mediaType);

            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }

        public static string Boundary(this MediaTypeHeaderValue media)
        {
            foreach (var value in media.Parameters)
            {
                if (string.Equals(value.Name.Value, BOUNDARY, StringComparison.OrdinalIgnoreCase))
                {
                    return value.Value.Value;
                }
            }

            return null;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && StringSegment.IsNullOrEmpty(contentDisposition.FileName)
                   && StringSegment.IsNullOrEmpty(contentDisposition.FileNameStar);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                   && contentDisposition.DispositionType.Equals("form-data")
                   && (!StringSegment.IsNullOrEmpty(contentDisposition.FileName)
                       || !StringSegment.IsNullOrEmpty(contentDisposition.FileNameStar));
        }
    }
}
