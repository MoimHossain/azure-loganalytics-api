

using Microsoft.AspNetCore.Http;

namespace LogAnalytics.Supports
{
    public static class SignatureSupport
    {
        public static bool HasValidApiKey(this HttpRequest request, string sharedAccessSignature)
        {
            var sas = request.Headers["x-api-key"];
            return sharedAccessSignature.Equals(sas);
        }
    }
}
