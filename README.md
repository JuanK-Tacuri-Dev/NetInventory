# NetInventory

Sistema de gestión de inventario desarrollado como prueba técnica Senior .NET. Implementa una API REST con ASP.NET Core 8 y una interfaz web SPA con Blazor WebAssembly, siguiendo Clean Architecture y principios SOLID.

---

## Tecnologías

| Capa | Tecnología |
|---|---|
| API | ASP.NET Core 8, JWT Bearer, Serilog, Swagger |
| ORM | Entity Framework Core 8 |
| Validación | FluentValidation 11 |
| Mapeo | Mapster |
| Cliente | Blazor WebAssembly (.NET 8) |
| Base de datos (dev) | SQLite (se crea automáticamente) |
| Base de datos (docker) | SQL Server 2022 |
| Contenedores | Docker + Docker Compose |
| Tests | xUnit, Moq, FluentAssertions |

---

## Requisitos previos

**Modo local (desarrollo):**
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- No requiere base de datos externa — SQLite se crea al iniciar

**Modo Docker:**
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- No requiere .NET ni SQL Server instalados

---

## Inicio rápido

### Opción A — Local con SQLite

```bash
# 1. Clonar el repositorio
git clone <url-del-repositorio>
cd NetInventory

# 2. Restaurar dependencias
dotnet restore

# 3. Terminal 1 — Iniciar API (aplica migraciones y siembra datos automáticamente)
dotnet run --project src/NetInventory.Api

# 4. Terminal 2 — Iniciar cliente Blazor
dotnet run --project src/NetInventory.Client
```

| Servicio | URL |
|---|---|
| UI Blazor | https://localhost:5002 |
| API + Swagger | https://localhost:5001/swagger |

### Opción B — Docker con SQL Server

```bash
# 1. Clonar el repositorio
git clone <url-del-repositorio>
cd NetInventory

# 2. Crear archivo de variables de entorno
cp .env.example .env
# (opcional) editar .env con contraseñas propias

# 3. Construir y levantar todos los servicios
docker compose -f docker/docker-compose.yml --env-file .env up --build
```

| Servicio | URL |
|---|---|
| UI Blazor | http://localhost:8080 |
| API + Swagger | http://localhost:5001/swagger |
| SQL Server | localhost:1433 |

```bash
# Detener servicios (conserva datos)
docker compose -f docker/docker-compose.yml down

# Detener y eliminar base de datos
docker compose -f docker/docker-compose.yml down -v
```

### Primer acceso

Registrar un usuario desde la pantalla `/register` o via API:

```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@netinventory.com", "password": "Admin1234!"}'
```

---

## Estructura del proyecto

```
NetInventory/
├── src/
│   ├── NetInventory.Domain/          # Entidades, value objects, interfaces
│   ├── NetInventory.Application/     # Casos de uso, CQRS, validaciones, DTOs
│   ├── NetInventory.Infrastructure/  # EF Core, repositorios, servicios externos
│   ├── NetInventory.Api/             # Controllers REST, middleware, configuración DI
│   ├── NetInventory.Client/          # Blazor WebAssembly SPA
│   └── NetInventory.Resources/       # Mensajes de validación (resx)
├── tests/
│   └── NetInventory.UnitTests/       # Tests unitarios de Domain y Application
├── docker/
│   ├── docker-compose.yml
│   ├── api/Dockerfile
│   └── client/Dockerfile + nginx.conf
├── .env.example                      # Plantilla de variables de entorno
└── README.md
```

---

## Arquitectura

El proyecto sigue **Clean Architecture** con dependencias en una sola dirección:

```
Domain  ←  Application  ←  Infrastructure  ←  Api
                                           ←  Client (independiente, consume la API via HTTP)
```

### Domain (`NetInventory.Domain`)
- Entidades puras sin anotaciones de EF: `Product`, `StockMovement`, `ErrorLog`, `AuditLog`, `AuditConfig`, `GeneralTable`, `GeneralValue`, `RefreshToken`
- Value Objects con factory `Create()` que retorna `Result<T>`: `SKU`, `Money`
- Patrón `Result<T>` / `Error` para flujos de negocio sin excepciones
- Interfaces de repositorio definidas aquí, implementadas en Infrastructure

### Application (`NetInventory.Application`)
- **CQRS manual** sin MediatR: Commands y Queries con handlers independientes
- **IDispatcher** como punto de entrada único a todos los handlers
- **Pipeline de validación**: `ValidationBehavior<TRequest>` con FluentValidation registrado como open generic
- **Strategy Pattern**: `IMovementStrategy` → `InboundStrategy` / `OutboundStrategy` para entradas y salidas de stock
- DTOs con Mapster para mapeo automático por convención
- `ICurrentUserService` abstrae el usuario autenticado del contexto HTTP

### Infrastructure (`NetInventory.Infrastructure`)
- `AppDbContext` extiende `IdentityDbContext<IdentityUser>`
- Mapeo EF Core via Fluent API (`IEntityTypeConfiguration`) — sin atributos en entidades
- Value Objects mapeados con `OwnsOne()`: SKU → columna `SKU`, Money → `decimal(18,2)`
- Detección automática SQLite vs SQL Server según el connection string
- Vista SQL `vw_Products` para consultas optimizadas con JOIN a categorías
- `LowStockBackgroundService`: servicio en background que verifica stock bajo periódicamente — intervalo configurable desde BD (`LOW_STOCK_MINUTES`)
- `GeneralValuesCacheWarmupService`: precalienta el caché de catálogos al arrancar
- Seeding automático de datos base al iniciar (`DbSeeder`)

### API (`NetInventory.Api`)
- Todos los endpoints protegidos con `[Authorize]` (JWT Bearer)
- Respuesta estándar: `ApiResponse<T>` con `success`, `data`, `error`, `errorCode`
- **Middleware pipeline**:
  - `CorrelationIdMiddleware` — propaga/genera `X-Correlation-ID` en cada request
  - `AuditMiddleware` — registra requests/responses según reglas configurables en BD
  - `GlobalExceptionMiddleware` — captura excepciones no controladas → 500 JSON uniforme

### Client (`NetInventory.Client`)
- `AuthService` extiende `AuthenticationStateProvider`, almacena JWT en localStorage
- `InventoryEventService`: evento compartido que comunica componentes desacoplados (ej: notificación inmediata al bell tras registrar movimiento)
- Polling de notificaciones de stock bajo parametrizable desde BD (`NOTIFICATION_POLL_MINUTES`)
- Sesión con cierre automático por inactividad (15 minutos con aviso previo)
- Validaciones en cliente con `DataAnnotations` + atributos custom (`NoWhitespace`, `SkuFormat`, `MaxDecimalPlaces`, `SafeText`)

---

## Funcionalidades

### Productos
- Listado paginado con filtros por nombre, SKU, categoría, precio y stock
- Búsqueda con debounce por columna
- Resaltado visual de filas con stock bajo
- Crear, editar y eliminar productos
- SKU inmutable una vez creado

### Movimientos de stock
- Registro de entradas (Inbound) y salidas (Outbound) con motivo opcional
- Modal de registro rápido desde el listado de productos
- Historial completo de movimientos por producto con tipo, cantidad, motivo, fecha y usuario

### Notificaciones
- Campana en la barra superior con badge de productos en stock bajo
- Dropdown con lista de productos afectados y acceso directo al detalle
- Polling automático con intervalo configurable desde la base de datos
- Refresco inmediato al registrar un movimiento sin esperar el ciclo de polling

### Autenticación
- Registro y login con JWT + Refresh Token
- Cierre de sesión automático por inactividad
- Rutas protegidas con redirección a login

### Auditoría
- Registro configurable de requests/responses por método HTTP y patrón de URL
- Consulta de logs de auditoría con filtros
- Logs de errores con stack trace, correlación y referencia única

### Catálogos (GeneralTable / GeneralValue)
- Tabla de configuración clave-valor usada para categorías y parámetros del sistema
- Caché en memoria con TTL configurable
- Parámetros clave: `LOW_STOCK_MINUTES`, `NOTIFICATION_POLL_MINUTES`

---

## API Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/auth/register` | Registrar usuario |
| POST | `/api/auth/login` | Iniciar sesión |
| POST | `/api/auth/refresh` | Renovar token |
| GET | `/api/products` | Listar productos (con filtro lowStockOnly) |
| POST | `/api/products/paged` | Listado paginado con filtros |
| GET | `/api/products/{id}` | Obtener producto por ID |
| POST | `/api/products` | Crear producto |
| PUT | `/api/products/{id}` | Actualizar producto |
| DELETE | `/api/products/{id}` | Eliminar producto |
| GET | `/api/products/{id}/movements` | Historial de movimientos |
| POST | `/api/products/{id}/movements` | Registrar movimiento |
| GET | `/api/general-values` | Obtener valores por tabla |
| GET | `/api/audit-logs` | Consultar logs de auditoría |
| GET/POST/PUT/DELETE | `/api/audit-configs` | Gestión de reglas de auditoría |
| GET | `/api/error-logs` | Consultar logs de errores |

Documentación interactiva disponible en `/swagger`.

---

## Variables de entorno

| Variable | Descripción | Requerida en |
|---|---|---|
| `ConnectionStrings__Default` | Cadena de conexión a la BD | Docker / Producción |
| `SA_PASSWORD` | Contraseña del usuario `sa` de SQL Server | Docker |
| `JwtSettings__Secret` | Clave secreta JWT (mínimo 32 caracteres) | Siempre |
| `JwtSettings__Issuer` | Emisor del token | Opcional (default: `NetInventory`) |
| `JwtSettings__Audience` | Audiencia del token | Opcional (default: `NetInventory`) |

Ver `.env.example` para la plantilla completa.

---

## Tests

```bash
# Ejecutar todos los tests
dotnet test tests/NetInventory.UnitTests/NetInventory.UnitTests.csproj

# Con reporte de cobertura
dotnet test tests/NetInventory.UnitTests/NetInventory.UnitTests.csproj \
  --collect:"XPlat Code Coverage" --results-directory ./coverage
```

**Cobertura:** ~90% líneas — 63 tests unitarios

| Suite | Tests |
|---|---|
| Domain | `ProductTests`, `SkuTests`, `MoneyTests`, `StockMovementTests`, `ResultTests` |
| Application — Commands | `CreateProduct`, `UpdateProduct`, `DeleteProduct`, `RegisterMovement` |
| Application — Queries | `GetProducts`, `GetProductById`, `GetMovements` |
| Application — Strategies | `InboundStrategy`, `OutboundStrategy` |

---

## Migraciones

```bash
# Crear nueva migración
dotnet ef migrations add <NombreMigracion> \
  --project src/NetInventory.Infrastructure \
  --startup-project src/NetInventory.Api

# Aplicar migraciones manualmente
dotnet ef database update \
  --project src/NetInventory.Infrastructure \
  --startup-project src/NetInventory.Api
```

> En modo local y Docker las migraciones se aplican automáticamente al iniciar la API.

---

## Datos iniciales

Al crear la base de datos por primera vez el sistema siembra automáticamente:

- **9 categorías de productos**: Electrónica, Ropa, Alimentos, Hogar, Juguetes, Herramientas, Deportes, Libros, Otros
- **Parámetros del sistema**:
  - `LOW_STOCK_MINUTES` = `4` — intervalo del servicio de verificación de stock (minutos)
  - `NOTIFICATION_POLL_MINUTES` = `5` — intervalo del polling de notificaciones en el cliente (minutos)
