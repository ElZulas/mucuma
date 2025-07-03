# 📊 Presupuesto Familiar Mensual

Una aplicación completa para el control de presupuestos familiares mensuales, desarrollada con arquitectura limpia y las mejores prácticas de ingeniería de software.

## 🎯 Problemática Resuelta

Una familia necesita una aplicación para controlar sus gastos y no exceder su presupuesto mensual, con reglas de negocio estrictas que garantizan la integridad financiera.

## 🏗️ Arquitectura del Proyecto

El proyecto sigue los principios de **Clean Architecture** con separación clara de responsabilidades:

```
PresupuestoFamiliarMensual/
├── src/
│   ├── PresupuestoFamiliarMensual.Core/          # Capa de Dominio
│   │   ├── Entities/                             # Entidades del dominio
│   │   ├── Interfaces/                           # Contratos de repositorios
│   │   └── Exceptions/                           # Excepciones de dominio
│   ├── PresupuestoFamiliarMensual.Application/   # Capa de Aplicación
│   │   ├── DTOs/                                 # Objetos de transferencia
│   │   ├── Services/                             # Servicios de aplicación
│   │   └── Mapping/                              # Configuración AutoMapper
│   ├── PresupuestoFamiliarMensual.Infrastructure/# Capa de Infraestructura
│   │   ├── Data/                                 # Contexto EF y UnitOfWork
│   │   └── Repositories/                         # Implementaciones de repositorios
│   └── PresupuestoFamiliarMensual.API/          # Capa de Presentación
│       └── Controllers/                          # Controladores REST
```

## 🧩 Entidades del Dominio

### Core Entities
- **FamilyMember**: Miembros de la familia
- **Month**: Meses del año
- **Budget**: Presupuesto mensual
- **BudgetCategory**: Categorías de gasto con límites
- **Expense**: Gastos registrados

## ⚙️ Funcionalidades Implementadas

### Endpoints de Presupuestos (`/api/budgets`)
- `GET /api/budgets` - Obtener todos los presupuestos
- `GET /api/budgets/{id}` - Obtener presupuesto por ID
- `GET /api/budgets/family-member/{familyMemberId}` - Presupuestos por miembro
- `POST /api/budgets` - Crear nuevo presupuesto
- `PUT /api/budgets/{id}` - Actualizar presupuesto
- `DELETE /api/budgets/{id}` - Eliminar presupuesto

### Endpoints de Categorías (`/api/budgets/{budgetId}/categories`)
- `GET /api/budgets/{budgetId}/categories` - Obtener categorías del presupuesto
- `GET /api/budgets/{budgetId}/categories/{id}` - Obtener categoría por ID
- `POST /api/budgets/{budgetId}/categories` - Crear nueva categoría
- `PUT /api/budgets/{budgetId}/categories/{id}` - Actualizar categoría
- `DELETE /api/budgets/{budgetId}/categories/{id}` - Eliminar categoría

### Endpoints de Gastos (`/api/budgets/{budgetId}/expenses`)
- `GET /api/budgets/{budgetId}/expenses` - Obtener gastos del presupuesto
- `GET /api/budgets/{budgetId}/expenses/{id}` - Obtener gasto por ID
- `POST /api/budgets/{budgetId}/expenses` - Registrar nuevo gasto
- `DELETE /api/budgets/{budgetId}/expenses/{id}` - Eliminar gasto

### Endpoints de Referencia (`/api/reference-data`)
- `GET /api/reference-data/family-members` - Obtener miembros de familia
- `GET /api/reference-data/months` - Obtener meses disponibles

## 🚨 Reglas de Negocio Críticas Implementadas

### 1. Control de Límites de Categoría
```csharp
// El sistema NO permite gastos que excedan el límite de la categoría
if (createExpenseDto.Amount > remainingAmount)
{
    throw new CategoryLimitExceededException(
        category.Name, 
        currentSpent, 
        category.Limit, 
        createExpenseDto.Amount);
}
```

### 2. Nombres de Categoría Únicos
```csharp
// No se permiten categorías con nombre repetido en el mismo presupuesto
var existsByName = await _unitOfWork.BudgetCategories
    .ExistsByNameInBudgetAsync(normalizedName, budgetId);
if (existsByName)
    throw new DuplicateCategoryNameException(normalizedName, budgetId);
```

### 3. Protección de Categorías con Gastos
```csharp
// No se puede eliminar una categoría que tiene gastos registrados
if (category.HasExpenses)
    throw new CategoryWithExpensesException(category.Name, category.Id, category.Expenses.Count);
```

## 🛠️ Tecnologías Utilizadas

- **.NET 8.0** - Framework principal
- **Entity Framework Core 8.0** - ORM para base de datos
- **SQL Server** - Base de datos (LocalDB para desarrollo)
- **AutoMapper** - Mapeo entre entidades y DTOs
- **Swagger/OpenAPI** - Documentación de API
- **Clean Architecture** - Patrón arquitectónico
- **Repository Pattern** - Patrón de acceso a datos
- **Unit of Work** - Patrón para transacciones

## 🚀 Instalación y Configuración

### Prerrequisitos
- .NET 8.0 SDK
- SQL Server LocalDB (incluido con Visual Studio)
- Visual Studio 2022 o VS Code

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <repository-url>
   cd PresupuestoFamiliarMensual
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Crear la base de datos**
   ```bash
   cd src/PresupuestoFamiliarMensual.API
   dotnet ef database update
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

5. **Acceder a la documentación**
   - Swagger UI: `https://localhost:7001/swagger`
   - API Base: `https://localhost:7001/api`

## 📝 Ejemplos de Uso

### 1. Crear un Presupuesto
```http
POST /api/budgets
Content-Type: application/json

{
  "totalAmount": 5000.00,
  "familyMemberId": 1,
  "monthId": 1
}
```

### 2. Crear una Categoría
```http
POST /api/budgets/1/categories
Content-Type: application/json

{
  "name": "Comida",
  "limit": 1000.00
}
```

### 3. Registrar un Gasto
```http
POST /api/budgets/1/expenses
Content-Type: application/json

{
  "amount": 150.00,
  "description": "Supermercado",
  "budgetCategoryId": 1,
  "familyMemberId": 1
}
```

## 🔍 Casos de Prueba

### Caso 1: Límite de Categoría Excedido
```http
POST /api/budgets/1/expenses
{
  "amount": 1200.00,
  "description": "Gasto que excede límite",
  "budgetCategoryId": 1,
  "familyMemberId": 1
}
```
**Respuesta esperada:**
```json
{
  "message": "No se puede registrar el gasto de $1200.00 en la categoría 'Comida'. Ya se han gastado $150.00 de $1000.00 disponibles.",
  "categoryName": "Comida",
  "currentSpent": 150.00,
  "limit": 1000.00,
  "attemptedAmount": 1200.00
}
```

### Caso 2: Categoría con Nombre Duplicado
```http
POST /api/budgets/1/categories
{
  "name": "Comida",
  "limit": 500.00
}
```
**Respuesta esperada:**
```json
{
  "message": "Ya existe una categoría con el nombre 'Comida' en este presupuesto."
}
```

### Caso 3: Eliminar Categoría con Gastos
```http
DELETE /api/budgets/1/categories/1
```
**Respuesta esperada:**
```json
{
  "message": "No se puede eliminar la categoría 'Comida' porque tiene 1 gasto(s) registrado(s). Para mantener la consistencia de los datos, primero debe eliminar todos los gastos de esta categoría."
}
```

## 📊 Características Técnicas

### Validaciones Implementadas
- ✅ Validación de monto mínimo (mayor a 0)
- ✅ Validación de límites de categoría
- ✅ Validación de nombres únicos
- ✅ Validación de integridad referencial
- ✅ Validación de datos de entrada con Data Annotations

### Seguridad y Robustez
- ✅ Manejo de excepciones personalizado
- ✅ Transacciones de base de datos
- ✅ Validación de modelo en controladores
- ✅ Respuestas HTTP apropiadas
- ✅ Logging de errores

### Performance
- ✅ Consultas optimizadas con Include
- ✅ Lazy loading deshabilitado
- ✅ Índices en base de datos
- ✅ Paginación preparada para futuras implementaciones

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.

## 👨‍💻 Autor

Desarrollado con ❤️ siguiendo las mejores prácticas de ingeniería de software.

---

**¡Confían en tu capacidad para aplicar lo aprendido!** 🚀 