
# What is it?
A simple API over Azure Log Analytics. The idea is, you can run this as a Docker container anywhere with some basic configuration, and consume log analytics queries via this API.

This is implemented for a very specific reason that fits a very specific setup. You might consider using Log Analytics APIs directly for less specific scenarios.

# How to build
The solution uses a docker container to build the solution. Execute this command in the root to build the solution

```
docker build -t repo/name .
```

# How to run the container?

You need following variables to deliver during the container startup:

- SharedAccessSignature - An API Key that you can define on your own. You send this in the header of the call
- TenantId - The Azure AD tenant ID - GUID
- ClientID - A service principal ID that has RBAC access to OMS workspace
- ClientSecret - The service principal Secret
- LogAnalyticsWorkspaceID - The OMS workspace ID - GUID (Get it from Azure Portal)
- LogAnalyticsPrimaryKey - The OMS workspace Primary Key (Get it from Azure Portal)

Once you have these secrets:

```bash
docker run -d -p 80:80 -e "SharedAccessSignature=<VALUE>" -e "TenantId=<KEY>" -e "ClientID=<KEY>" 
-e "ClientSecret=<KEY>" -e "LogAnalyticsWorkspaceID=<KEY>" -e  "LogAnalyticsPrimaryKey=<KEY>" repository/namecontainer:latest
```


# How to post data to Log Analytics
Start the container and then execute a POST request to the following url

```
POST https://url/api/logs
Content-type:application/json
x-api-key: <SharedAccessSignature>

BODY
{
	"customlogType":"Name of the Custom Log Type e.g.",
	"json":{
		"demo":"demo"
	}
}
```

# How to query Log Analytics
Start the container and then execute a GET request to the following url
```
POST https://url/api/logs?query=AzureDiagnostics
Content-type:application/json
x-api-key: <SharedAccessSignature>
```


## Thanks

Forked from https://github.com/MoimHossain/azure-loganalytics-api