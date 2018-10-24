

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalytics.Supports
{
    public class HttpClientFactory
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly TokenHelper tokenHelper;

        public HttpClientFactory(TokenHelper tokenHelper)
        {
            this.tokenHelper = tokenHelper;
        }

        public async Task<string> GetStringAsync(Uri uri)
        {
            httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer",
                await this.tokenHelper.GetAccessToken());

            var response = await httpClient.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<TPayload> PostAsync<TPayload>(string uri, object payload)
        {
            httpClient.DefaultRequestHeaders.Authorization
              = new AuthenticationHeaderValue("Bearer",
              await this.tokenHelper.GetAccessToken());

            var response = await httpClient.PostAsync(uri, ToJsonContent(payload));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return FromJson<TPayload>(await response.Content.ReadAsStringAsync());
        }

        private StringContent ToJsonContent(object payload)
        {
            var content = new StringContent(ToJson(payload), Encoding.UTF8, "application/json");
            return content;
        }

        private string ToJson(object payload)
        {
            return JsonConvert.SerializeObject(payload,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        private TPayload FromJson<TPayload>(string payload)
        {
            return JsonConvert.DeserializeObject<TPayload>(payload,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}
