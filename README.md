# NetInventory

NetInventory es un sistema de gestión de inventario construido con arquitectura limpia (Clean Architecture) sobre .NET 8. Expone una API REST documentada con Swagger (`NetInventory.Api`) y una interfaz web de página única con Blazor WebAssembly (`NetInventory.Client`). Usa SQLite en desarrollo y SQL Server en producción, autenticación stateless con JWT y logging estructurado con Serilog.

## Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Docker + Docker Compose](https://docs.docker.com/get-docker/) — solo para modo producción
- No se requiere ninguna base de datos externa para desarrollo; SQLite se crea automáticamente al aplicar las migraciones

## Inicio rapido (desarrollo local)

```bash
git clone <repo-url>
cd NetInventory

# Restaurar dependencias
dotnet restore

# Aplicar migraciones (crea inventory.db en la raiz del proyecto de Api)
dotnet ef database update --project src/NetInventory.Infrastructure --startup-project src/NetInventory.Api

# Terminal 1 — API
dotnet run --project src/NetInventory.Api

# Terminal 2 — Cliente Blazor WASM
dotnet run --project src/NetInventory.Client
```

| Servicio | URL |
|----------|-----|
| API + Swagger | http://localhost:5001/swagger |
| UI Blazor | http://localhost:5002 |

## Credenciales de prueba

Registra un usuario desde la pantalla `/register` o directamente en la API:

```bash
curl -X POST http://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{ "email": "admin@netinventory.com", "password": "Admin1234" }'
```

```json
{ "email": "admin@netinventory.com", "password": "Admin1234" }
```

## Variables de entorno

| Variable | Descripcion | Valor por defecto |
|----------|-------------|-------------------|
| `ConnectionStrings__Default` | Cadena de conexion a la base de datos | SQLite local (`inventory.db`) |
| `JwtSettings__Secret` | Clave secreta JWT (minimo 32 caracteres) | Solo para desarrollo |
| `JwtSettings__Issuer` | Emisor del token JWT | `NetInventory` |
| `JwtSettings__Audience` | Audiencia del token JWT | `NetInventory` |
| `SA_PASSWORD` | Contrasena del usuario `sa` de SQL Server | Solo en produccion |

## Ejecutar con Docker (produccion)

```bash
# 1. Copia el archivo de variables de entorno y edita los valores
cp .env.example .env

# 2. Construye las imagenes y levanta los servicios
docker compose up --build
```

| Servicio | URL |
|----------|-----|
| API + Swagger | http://localhost:5001/swagger |
| UI Blazor | http://localhost:8080 |
| SQL Server | localhost:1433 |

Para detener los servicios sin borrar datos:

```bash
docker compose down
```

Para detener y eliminar volumenes (borra la base de datos):

```bash
docker compose down -v
```

## Migraciones de base de datos

```bash
# Crear una nueva migracion
dotnet ef migrations add <NombreDeLaMigracion> \
  --project src/NetInventory.Infrastructure \
  --startup-project src/NetInventory.Api

# Aplicar migraciones pendientes
dotnet ef database update \
  --project src/NetInventory.Infrastructure \
  --startup-project src/NetInventory.Api
```

> En Docker las migraciones se aplican automaticamente al arrancar la API mediante `MigrateAsync()` en `Program.cs`.

## Ejecutar tests

```bash
# Todos los tests
dotnet test

# Con reporte de cobertura de codigo
dotnet test --collect:"XPlat Code Coverage"
```

Los resultados de cobertura se generan en `TestResults/` en formato Cobertura XML, compatible con herramientas como ReportGenerator o la extension de cobertura de Visual Studio.

## Arquitectura

El proyecto sigue Clean Architecture con cuatro capas internas y dos proyectos de entrada:

```
NetInventory.Domain          — Entidades, value objects, interfaces de repositorio
NetInventory.Application     — Casos de uso, DTOs, validaciones (sin dependencias de infraestructura)
NetInventory.Infrastructure  — EF Core, repositorios, migraciones, servicios externos
NetInventory.Api             — Controllers REST, middleware, configuracion de DI, Swagger
NetInventory.Client          — Blazor WebAssembly SPA (consume la API via HTTP)
tests/NetInventory.UnitTests — Tests unitarios de Application y Domain
```

Las dependencias fluyen en una sola direccion: `Domain <- Application <- Infrastructure <- Api`. El cliente es completamente independiente de las capas del servidor.
