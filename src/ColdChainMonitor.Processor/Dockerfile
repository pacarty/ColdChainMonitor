FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ColdChainMonitor.slnx", "."]
COPY ["src/ColdChainMonitor.Processor/ColdChainMonitor.Processor.csproj", "src/ColdChainMonitor.Processor/"]
COPY ["src/ColdChainMonitor.Application/ColdChainMonitor.Application.csproj", "src/ColdChainMonitor.Application/"]
COPY ["src/ColdChainMonitor.Infrastructure/ColdChainMonitor.Infrastructure.csproj", "src/ColdChainMonitor.Infrastructure/"]
COPY ["src/ColdChainMonitor.Domain/ColdChainMonitor.Domain.csproj", "src/ColdChainMonitor.Domain/"]
RUN dotnet restore "src/ColdChainMonitor.Processor/ColdChainMonitor.Processor.csproj"
COPY . .
WORKDIR "/src/src/ColdChainMonitor.Processor"
RUN dotnet build "ColdChainMonitor.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ColdChainMonitor.Processor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ColdChainMonitor.Processor.dll"]
