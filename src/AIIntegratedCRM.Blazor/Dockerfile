FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/AIIntegratedCRM.Blazor/AIIntegratedCRM.Blazor.csproj", "src/AIIntegratedCRM.Blazor/"]
RUN dotnet restore "src/AIIntegratedCRM.Blazor/AIIntegratedCRM.Blazor.csproj"
COPY . .
WORKDIR "/src/src/AIIntegratedCRM.Blazor"
RUN dotnet publish "AIIntegratedCRM.Blazor.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY src/AIIntegratedCRM.Blazor/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
