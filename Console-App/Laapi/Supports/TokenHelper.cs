using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laapi.Supports
{
    public class TokenHelper
    {
        private static AuthenticationResult CachedToken = default(AuthenticationResult);
        private readonly string tenantId;
        private readonly string resource;
        private readonly string clientId;
        private readonly string clientSecret;

        public TokenHelper(string tenantId, string clientId, string clientSecret, string resource)
        {
            this.tenantId = tenantId;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.resource = resource;
        }

        public async Task<string> GetAccessToken()
        {
            if (CachedToken != null)
            {
                var safeNow = DateTime.Now.Subtract(new TimeSpan(0, 0, 5));
                var expiresOn = CachedToken.ExpiresOn.ToLocalTime();
                if (safeNow <= expiresOn)
                {
                    return CachedToken.AccessToken;
                }
            }

            CachedToken = await GetAccessTokenCore();
            return CachedToken.AccessToken;
        }

        private async Task<AuthenticationResult> GetAccessTokenCore()
        {
            var authContext = new AuthenticationContext($"https://login.windows.net/{tenantId}");
            var creadential = new ClientCredential(this.clientId, this.clientSecret);
            var result = await authContext.AcquireTokenAsync(resource, creadential);
            return result;
        }
    }
}
