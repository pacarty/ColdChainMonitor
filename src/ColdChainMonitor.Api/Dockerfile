FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ColdChainMonitor.slnx", "."]
COPY ["src/ColdChainMonitor.Api/ColdChainMonitor.Api.csproj", "src/ColdChainMonitor.Api/"]
COPY ["src/ColdChainMonitor.Application/ColdChainMonitor.Application.csproj", "src/ColdChainMonitor.Application/"]
COPY ["src/ColdChainMonitor.Infrastructure/ColdChainMonitor.Infrastructure.csproj", "src/ColdChainMonitor.Infrastructure/"]
COPY ["src/ColdChainMonitor.Domain/ColdChainMonitor.Domain.csproj", "src/ColdChainMonitor.Domain/"]
RUN dotnet restore "src/ColdChainMonitor.Api/ColdChainMonitor.Api.csproj"
COPY . .
WORKDIR "/src/src/ColdChainMonitor.Api"
RUN dotnet build "ColdChainMonitor.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ColdChainMonitor.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ColdChainMonitor.Api.dll"]
