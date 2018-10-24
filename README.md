
# What is it?
A simple API over Azure Log Analytics. The idea is, you can run this as a Docker container anywhere with some basic configuration, and consume log analytics queries via this API.

This is implemented for a very specific reason that fits a very specific setup. You might consider using Log Analytics APIs directly for less specific scenarios.

# How to run?

You need following variables to deliver during the container startup:

- SharedAccessSignature - An API Key that you can define on your own
- TenantId - The Azure AD tenant ID - GUID
- ClientID - A service principal ID that has RBAC access to OMS workspace
- ClientSecret - The service principal Secret
- LogAnalyticsWorkspaceID - The OMS workspace ID - GUID (Get it from Azure Portal)

Once you have these secrets:

```bash
docker run -d -p 80:80 -e "SharedAccessSignature=<VALUE>" -e "TenantId=<KEY>" -e "ClientID=<KEY>" 
-e "ClientSecret=<KEY>" -e "LogAnalyticsWorkspaceID=<KEY>" moimhossain/azure-loganalytics:latest
```


Voila!



## Thanks

Feel free to use it!