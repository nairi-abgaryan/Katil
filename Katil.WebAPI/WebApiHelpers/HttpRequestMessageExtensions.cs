using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Katil.WebAPI.WebApiHelpers
{
    public static class HttpRequestMessageExtensions
    {
        private const string Token = "Token";

        public static string GetToken(this HttpRequest request)
        {
            StringValues headerValue;

            if (request.Headers.TryGetValue(Token, out headerValue))
            {
                var valueString = headerValue.FirstOrDefault();
                if (valueString != null)
                {
                    return valueString;
                }
            }

            return string.Empty;
        }

        public static string GetTokenFromResponse(this HttpResponse response)
        {
            StringValues headerValue;

            if (response.Headers.TryGetValue(Token, out headerValue))
            {
                var valueString = headerValue.FirstOrDefault();
                if (valueString != null)
                {
                    return valueString;
                }
            }

            return string.Empty;
        }
    }
}
