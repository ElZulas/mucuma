# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar archivos de proyecto primero (para optimizar caché)
COPY ["src/PresupuestoFamiliarMensual.API/PresupuestoFamiliarMensual.API.csproj", "src/PresupuestoFamiliarMensual.API/"]
COPY ["src/PresupuestoFamiliarMensual.Core/PresupuestoFamiliarMensual.Core.csproj", "src/PresupuestoFamiliarMensual.Core/"]
COPY ["src/PresupuestoFamiliarMensual.Application/PresupuestoFamiliarMensual.Application.csproj", "src/PresupuestoFamiliarMensual.Application/"]
COPY ["src/PresupuestoFamiliarMensual.Infrastructure/PresupuestoFamiliarMensual.Infrastructure.csproj", "src/PresupuestoFamiliarMensual.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "src/PresupuestoFamiliarMensual.API/PresupuestoFamiliarMensual.API.csproj"

# 2. Copiar el resto de los archivos
COPY . .

# Publicar en modo Release
RUN dotnet publish "src/PresupuestoFamiliarMensual.API/PresupuestoFamiliarMensual.API.csproj" -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Exponer puerto
EXPOSE 8080

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "PresupuestoFamiliarMensual.API.dll"]