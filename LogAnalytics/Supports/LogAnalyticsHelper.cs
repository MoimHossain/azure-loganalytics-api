using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalytics.Supports
{
    public class LogAnalyticsHelper
    {
        //Find lots of info here https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-data-collector-api

        private readonly string LogAnalyticsWorkspaceID;
        private readonly string LogAnalyticsPrimaryKey;


        public LogAnalyticsHelper(string logAnalyticsWorkspaceID, string logAnalyticsPrimaryKey)
        {
            LogAnalyticsWorkspaceID = logAnalyticsWorkspaceID;
            LogAnalyticsPrimaryKey = logAnalyticsPrimaryKey;
        }

        public async Task AddCustomLogJsonAsync(string logName, string json, string timefield)
        {
            // Create a hash for the API signature
            var datestring = DateTime.UtcNow.ToString("r");
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
            var hashedString = BuildSignature(stringToHash, LogAnalyticsPrimaryKey);
            var signature = "SharedKey " + LogAnalyticsWorkspaceID + ":" + hashedString;

            await PostData(logName, signature, datestring, json, timefield);
        }


        // Build the API signature
        private string BuildSignature(string message, string secret)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using(var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }

        // Send a request to the POST API endpoint
        private async Task PostData(string logname, string signature, string date, string json, string timefield)
        {
            try
            {

                string url = "https://" + LogAnalyticsWorkspaceID + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Log-Type", logname);
                client.DefaultRequestHeaders.Add("Authorization", signature);
                client.DefaultRequestHeaders.Add("x-ms-date", date);
                client.DefaultRequestHeaders.Add("time-generated-field", timefield);


                var httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(new Uri(url), httpContent);

                var responseContent = response.Result.Content;
                string result = await responseContent.ReadAsStringAsync();
            }
            catch(Exception excep)
            {
                Console.WriteLine("API Post Exception: " + excep.Message);
            }
        }
    }
}