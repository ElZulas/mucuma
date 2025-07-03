# 🚂 Despliegue en Railway

## Configuración del Proyecto

Este proyecto está configurado para desplegarse automáticamente en Railway.

### 📋 Requisitos Previos

1. **Cuenta en Railway**: [railway.app](https://railway.app)
2. **Repositorio en GitHub**: Con el código del proyecto
3. **Base de datos PostgreSQL**: Proporcionada por Railway

### 🚀 Pasos para Desplegar

#### 1. Conectar Repositorio
1. Ve a [Railway Dashboard](https://railway.app/dashboard)
2. Haz clic en "New Project"
3. Selecciona "Deploy from GitHub repo"
4. Conecta tu repositorio de GitHub
5. Selecciona el repositorio `PresupuestoFamiliarMensual`

#### 2. Configurar Base de Datos
1. En tu proyecto Railway, haz clic en "New Service"
2. Selecciona "Database" → "PostgreSQL"
3. Railway creará automáticamente una base de datos PostgreSQL

#### 3. Configurar Variables de Entorno
Railway detectará automáticamente la variable `DATABASE_URL` de la base de datos PostgreSQL.

**Variables opcionales:**
- `ASPNETCORE_ENVIRONMENT`: `Production`
- `ASPNETCORE_URLS`: `http://0.0.0.0:8080`

#### 4. Desplegar la Aplicación
1. Railway detectará automáticamente el `Dockerfile`
2. El build se ejecutará automáticamente
3. La aplicación se desplegará en un contenedor Docker

### 🔧 Configuración Automática

El proyecto incluye:

- **Dockerfile**: Configuración para contenedor Docker
- **railway.json**: Configuración específica para Railway
- **Health Check**: Endpoint `/api/referencedata/health`
- **Migraciones**: Se ejecutan automáticamente al iniciar

### 📊 Monitoreo

- **Logs**: Disponibles en Railway Dashboard
- **Health Check**: Automático en `/api/referencedata/health`
- **Métricas**: CPU, memoria y red en tiempo real

### 🔗 URLs

- **API**: `https://tu-proyecto.railway.app`
- **Swagger**: `https://tu-proyecto.railway.app/swagger`
- **Health Check**: `https://tu-proyecto.railway.app/api/referencedata/health`

### 🛠️ Comandos Útiles

```bash
# Ver logs en tiempo real
railway logs

# Ejecutar migraciones manualmente
railway run dotnet ef database update --project src/PresupuestoFamiliarMensual.Infrastructure

# Conectar a la base de datos
railway connect
```

### 🔒 Seguridad

- **HTTPS**: Automático en Railway
- **Variables de entorno**: Encriptadas
- **Base de datos**: Aislada y segura

### 📈 Escalabilidad

Railway permite escalar automáticamente según la demanda:
- **Auto-scaling**: Basado en CPU y memoria
- **Load balancing**: Automático
- **CDN**: Global para mejor rendimiento

### 🆘 Solución de Problemas

#### Error de Conexión a Base de Datos
1. Verifica que la variable `DATABASE_URL` esté configurada
2. Revisa los logs de la aplicación
3. Verifica que la base de datos esté activa

#### Error de Migraciones
1. Ejecuta manualmente: `railway run dotnet ef database update`
2. Verifica la conectividad a la base de datos
3. Revisa los logs de migración

#### Error de Build
1. Verifica que el Dockerfile esté correcto
2. Revisa los logs de build
3. Asegúrate de que todas las dependencias estén incluidas

### 📞 Soporte

- **Railway Docs**: [docs.railway.app](https://docs.railway.app)
- **Discord**: [Railway Discord](https://discord.gg/railway)
- **GitHub Issues**: Para problemas específicos del proyecto 