#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LogAnalytics/LogAnalytics.csproj", "LogAnalytics/"]
RUN dotnet restore "LogAnalytics/LogAnalytics.csproj"
COPY . .
WORKDIR "/src/LogAnalytics"
RUN dotnet build "LogAnalytics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogAnalytics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogAnalytics.dll"]