#!/bin/bash

# Script de migración para Railway
echo "Iniciando migración de base de datos..."

# Esperar a que la base de datos esté lista
echo "Esperando conexión a la base de datos..."
sleep 10

# Ejecutar migraciones
echo "Ejecutando migraciones..."
dotnet ef database update --project src/PresupuestoFamiliarMensual.Infrastructure

# Verificar si las migraciones se ejecutaron correctamente
if [ $? -eq 0 ]; then
    echo "✅ Migraciones ejecutadas correctamente"
else
    echo "❌ Error al ejecutar migraciones"
    exit 1
fi

echo "Migración completada" 