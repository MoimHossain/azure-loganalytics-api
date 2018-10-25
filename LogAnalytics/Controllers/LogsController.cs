

using LogAnalytics.Models;
using LogAnalytics.Supports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace LogAnalytics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly string uri;

        public HttpClientFactory httpFactory { get; }

        private readonly string LogAnalyticsResource;
        private readonly string SharedAccessSignature;
        private readonly string TenantId;
        private readonly string ClientID;
        private readonly string ClientSecret;
        private readonly string LogAnalyticsWorkspaceID;
        private readonly string LogAnalyticsPrimaryKey;

        public LogsController()
        {
            LogAnalyticsResource = "https://api.loganalytics.io";

            SharedAccessSignature = Env.GetEnvironmentVariable(Env.Keys.SharedAccessSignature);
            TenantId = Env.GetEnvironmentVariable(Env.Keys.TenantId);
            ClientID = Env.GetEnvironmentVariable(Env.Keys.ClientID);
            ClientSecret = Env.GetEnvironmentVariable(Env.Keys.ClientSecret);
            LogAnalyticsWorkspaceID = Env.GetEnvironmentVariable(Env.Keys.LogAnalyticsWorkspaceID);
            LogAnalyticsPrimaryKey = Env.GetEnvironmentVariable(Env.Keys.LogAnalyticsPrimaryKey);


            uri = $"https://api.loganalytics.io/v1/workspaces/{this.LogAnalyticsWorkspaceID}/query";
            this.httpFactory = new HttpClientFactory(new TokenHelper(this.TenantId, this.ClientID, this.ClientSecret, this.LogAnalyticsResource));
        }


        [HttpGet("")]
        public async Task<ActionResult<Logs>> GetLogsAsync(string query, string timespan)
        {
            if (!Request.HasValidApiKey(SharedAccessSignature))
            {
                return new UnauthorizedResult();
            }
            var logs = await httpFactory.PostAsync<Logs>(this.uri, new
            {
                Query = query,
                Timespan = timespan
            });
            return logs;
        }

        [HttpPost("")]
        public async Task<ActionResult> PostCustomLogType([FromBody]JObject body)
        {
            if (!Request.HasValidApiKey(SharedAccessSignature))
            {
                return new UnauthorizedResult();
            }

            var customlogType = body.GetValue("customlogType").Value<string>();
            var json = body.GetValue("json").ToString(Formatting.None);

            if (!string.IsNullOrEmpty(json))
            {

                var logAnalytics = new LogAnalyticsHelper(LogAnalyticsWorkspaceID, LogAnalyticsPrimaryKey);
                await logAnalytics.AddCustomLogJsonAsync(customlogType, json, "");
                return new OkResult();
            }
            return new UnprocessableEntityResult();
        }
    }
}