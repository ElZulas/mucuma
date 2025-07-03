#!/bin/bash

echo "🚀 Iniciando migración de base de datos en Railway..."

# Navegar al directorio de la API
cd src/PresupuestoFamiliarMensual.API

# Verificar si las variables de entorno están configuradas
if [ -z "$DB_HOST" ] || [ -z "$DB_PORT" ] || [ -z "$DB_NAME" ] || [ -z "$DB_USER" ] || [ -z "$DB_PASSWORD" ]; then
    echo "❌ Error: Variables de entorno de base de datos no configuradas"
    echo "Variables requeridas: DB_HOST, DB_PORT, DB_NAME, DB_USER, DB_PASSWORD"
    exit 1
fi

echo "✅ Variables de entorno configuradas correctamente"
echo "Host: $DB_HOST"
echo "Puerto: $DB_PORT"
echo "Base de datos: $DB_NAME"
echo "Usuario: $DB_USER"

# Restaurar dependencias
echo "📦 Restaurando dependencias..."
dotnet restore

# Ejecutar migraciones
echo "🗄️ Ejecutando migraciones..."
dotnet ef database update --verbose

if [ $? -eq 0 ]; then
    echo "✅ Migraciones ejecutadas exitosamente"
else
    echo "❌ Error al ejecutar migraciones"
    exit 1
fi

echo "🎉 Base de datos configurada correctamente en Railway!" 