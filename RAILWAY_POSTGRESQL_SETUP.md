# 🚀 Configuración de PostgreSQL en Railway

## 📋 Pasos para configurar PostgreSQL en Railway

### 1. Crear Base de Datos PostgreSQL en Railway

1. Ve a [Railway Dashboard](https://railway.app/dashboard)
2. Crea un nuevo proyecto o usa uno existente
3. Haz clic en **"New Service"** → **"Database"** → **"PostgreSQL"**
4. Railway creará automáticamente una base de datos PostgreSQL

### 2. Configurar Variables de Entorno

En tu proyecto de Railway, ve a la pestaña **"Variables"** y agrega:

```bash
DB_HOST=tu-host-postgresql.railway.app
DB_PORT=5432
DB_NAME=railway
DB_USER=postgres
DB_PASSWORD=tu-password-postgresql
```

**Nota:** Railway te proporcionará estos valores automáticamente cuando crees la base de datos.

### 3. Conectar el Proyecto con la Base de Datos

1. En tu proyecto de Railway, ve a **"Settings"**
2. En la sección **"Connect"**, conecta tu servicio de PostgreSQL
3. Railway configurará automáticamente las variables de entorno

### 4. Ejecutar Migraciones

El proyecto está configurado para ejecutar migraciones automáticamente al iniciar. Si necesitas ejecutarlas manualmente:

```bash
# En el directorio del proyecto
cd src/PresupuestoFamiliarMensual.API
dotnet ef database update
```

### 5. Verificar la Conexión

Una vez desplegado, puedes verificar que todo funciona:

- **Swagger UI**: `https://tu-app.railway.app/swagger`
- **Health Check**: `https://tu-app.railway.app/health`

## 🔧 Configuración Técnica

### Cadena de Conexión PostgreSQL
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true"
  }
}
```

### Variables de Entorno Requeridas
- `DB_HOST`: Host de PostgreSQL en Railway
- `DB_PORT`: Puerto (generalmente 5432)
- `DB_NAME`: Nombre de la base de datos
- `DB_USER`: Usuario de PostgreSQL
- `DB_PASSWORD`: Contraseña de PostgreSQL

## 🐛 Solución de Problemas

### Error de Conexión
Si tienes problemas de conexión:
1. Verifica que las variables de entorno estén configuradas
2. Asegúrate de que la base de datos esté activa en Railway
3. Revisa los logs de la aplicación en Railway

### Error de Migración
Si las migraciones fallan:
1. Verifica que la base de datos esté vacía
2. Ejecuta `dotnet ef database update --verbose` para más detalles
3. Revisa que el usuario tenga permisos de escritura

## 📊 Monitoreo

- **Logs**: Ve a la pestaña "Logs" en Railway para ver los logs en tiempo real
- **Métricas**: Railway proporciona métricas de uso de CPU y memoria
- **Base de Datos**: Puedes ver las métricas de la base de datos en su servicio correspondiente

## 🔗 Enlaces Útiles

- [Railway PostgreSQL Documentation](https://docs.railway.app/databases/postgresql)
- [Entity Framework Core con PostgreSQL](https://docs.microsoft.com/en-us/ef/core/providers/npgsql/)
- [Railway CLI](https://docs.railway.app/develop/cli) 