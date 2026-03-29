FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AIIntegratedCRM.API/AIIntegratedCRM.API.csproj", "src/AIIntegratedCRM.API/"]
COPY ["src/AIIntegratedCRM.Application/AIIntegratedCRM.Application.csproj", "src/AIIntegratedCRM.Application/"]
COPY ["src/AIIntegratedCRM.Domain/AIIntegratedCRM.Domain.csproj", "src/AIIntegratedCRM.Domain/"]
COPY ["src/AIIntegratedCRM.Infrastructure/AIIntegratedCRM.Infrastructure.csproj", "src/AIIntegratedCRM.Infrastructure/"]
RUN dotnet restore "src/AIIntegratedCRM.API/AIIntegratedCRM.API.csproj"
COPY . .
WORKDIR "/src/src/AIIntegratedCRM.API"
RUN dotnet build "AIIntegratedCRM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AIIntegratedCRM.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AIIntegratedCRM.API.dll"]
