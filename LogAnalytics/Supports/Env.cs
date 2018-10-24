

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogAnalytics.Supports
{
    public class Env
    {
        public class Keys
        {
            public const string SharedAccessSignature = "SharedAccessSignature";
            public const string TenantId = "TenantId";
            public const string ClientID = "ClientID";
            public const string ClientSecret = "ClientSecret";
            public const string LogAnalyticsWorkspaceID = "LogAnalyticsWorkspaceID";
        }

        public static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
