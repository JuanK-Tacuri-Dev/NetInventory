# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Descripción del proyecto

**NetInventory** — Sistema de Gestión de Inventario construido como prueba técnica Senior .NET.

Stack: ASP.NET Core 8 API + Blazor WebAssembly + EF Core 8 + ASP.NET Core Identity + JWT Bearer + Serilog + FluentValidation 11.

## Comandos esenciales

```bash
# Compilar solución completa
dotnet build NetInventory.sln

# Ejecutar todos los tests
dotnet test tests/NetInventory.UnitTests/NetInventory.UnitTests.csproj

# Filtrar un test específico
dotnet test --filter "FullyQualifiedName~RegisterMovementTests"

# Tests con cobertura
dotnet test tests/NetInventory.UnitTests/NetInventory.UnitTests.csproj \
  --collect:"XPlat Code Coverage" --results-directory ./coverage-final

# Ejecutar API (SQLite local en desarrollo)
dotnet run --project src/NetInventory.Api

# Ejecutar cliente Blazor
dotnet run --project src/NetInventory.Client

# Agregar migración EF Core (requiere DOTNET_ROOT en WSL)
DOTNET_ROOT=/root/.dotnet PATH="/root/.dotnet:$PATH" \
  /root/.dotnet/tools/dotnet-ef migrations add <NombreMigracion> \
  --project src/NetInventory.Infrastructure \
  --startup-project src/NetInventory.Api \
  --output-dir Persistence/Migrations

# Stack completo con Docker (SQL Server)
docker compose up --build
```

## Arquitectura — Clean Architecture

```
Domain → Application → Infrastructure
                    ↗              ↘
              Api (controladores)   Client (Blazor WASM)
```

### Domain (`src/NetInventory.Domain/`)
- Entidades SRP puras: `Product`, `StockMovement` — **sin anotaciones EF ni atributos de datos**
- Value Objects con factory `Create()` que retorna `Result<T>`: `Sku`, `Money`
- `Common/Result.cs` y `Common/Error.cs` — patrón Result<T> universal (sin excepciones de flujo de negocio)
- Interfaces de repositorio en Domain, implementadas en Infrastructure

### Application (`src/NetInventory.Application/`)
- CQRS manual: `Products/Commands/` y `Products/Queries/` — **sin MediatR**
- `Common/ValidationBehavior<TRequest>` — pipeline FluentValidation; registrado como open generic
- Strategy Pattern: `IMovementStrategy` → `InboundStrategy` / `OutboundStrategy`
- `Common/Interfaces/ICurrentUserService.cs` — implementado en Infrastructure via `IHttpContextAccessor`

### Infrastructure (`src/NetInventory.Infrastructure/`)
- `AppDbContext` extiende `IdentityDbContext<IdentityUser>`
- Mapeo EF Core vía `IEntityTypeConfiguration` en `Persistence/Configurations/` (Fluent API, no atributos)
- Value Objects mapeados con `OwnsOne()`: `SKU` → columna "SKU", `UnitPrice` → decimal(18,2)
- `AddInfrastructure()` detecta SQLite (si el connection string contiene ".db") vs SQL Server automáticamente
- `LowStockBackgroundService` — `BackgroundService`, revisa cada hora, loga con Serilog.Warning
- **Requiere** `<FrameworkReference Include="Microsoft.AspNetCore.App" />` en el .csproj para `IHttpContextAccessor`

### API (`src/NetInventory.Api/`)
- Todos los endpoints bajo `[Authorize]` (JWT Bearer)
- `GlobalExceptionMiddleware` — captura excepciones no controladas → 500 JSON camelCase
- `CorrelationIdMiddleware` — propaga/genera `X-Correlation-ID`
- Respuesta estándar: `ApiResponse<T>` con `success`, `data`, `error`, `errorCode`
- CORS: política "Dev" (localhost:5002, localhost:8080) y "Prod" (configurable via `AllowedOrigins`)
- JWT Secret: `appsettings.Development.json` tiene el secret de desarrollo; producción usa variable de entorno

### Client (`src/NetInventory.Client/`)
- `AuthService` extiende `AuthenticationStateProvider`, guarda JWT en localStorage
- `ApiBaseUrl` configurado en `wwwroot/appsettings.json` (dev: https://localhost:5001) y `wwwroot/appsettings.Production.json`
- Filas con `QuantityInStock < 10` resaltadas en amarillo (`background-color:#fff3cd`)
- `App.razor` usa `AuthorizeRouteView` con `<NotAuthorized><RedirectToLogin /></NotAuthorized>`

## Patrones y convenciones de código

- **Primary constructors** en servicios y repositorios: `public sealed class Repo(AppDbContext ctx)`
- **`sealed`** en todas las clases concretas que no se heredan
- **`Result<T>`** para todos los flujos de negocio — nunca `throw` para control de flujo
- Validators en la misma carpeta del Command/Query que validan
- Repositories usan `AsNoTracking()` en queries, tracking solo en comandos
- `var` solo cuando el tipo es evidente en la misma línea

## Tests (`tests/NetInventory.UnitTests/`)

Cobertura actual: **Domain 96.7% lines**, **Application 85.8% lines**, **Total 89.4% lines** (63 tests).

- `Domain/` — `ProductTests`, `SkuTests`, `MoneyTests`, `StockMovementTests`, `ResultTests`
- `Application/` — handlers de Commands y Queries con Moq, `InboundStrategyTests`, `OutboundStrategyTests`
- Usa FluentAssertions para aserciones legibles

## Variables de entorno y secretos

El archivo `.env.example` documenta las variables requeridas. **Nunca commitear `.env`**.

Para producción, configurar:
- `ConnectionStrings__Default` — SQL Server connection string
- `JwtSettings__Secret` — mínimo 32 caracteres
- `AllowedOrigins` — URL del cliente Blazor en producción
