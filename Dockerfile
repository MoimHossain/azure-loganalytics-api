FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 62172
EXPOSE 44352

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["LogAnalytics/LogAnalytics.csproj", "LogAnalytics/"]
RUN dotnet restore "LogAnalytics/LogAnalytics.csproj"
COPY . .
WORKDIR "/src/LogAnalytics"
RUN dotnet build "LogAnalytics.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LogAnalytics.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "LogAnalytics.dll"]