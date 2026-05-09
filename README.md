# SwimTracker

Sistema de gestión y seguimiento para clubs de natación, nadadores y competiciones.

## Descripción

SwimTracker es una API REST desarrollada con ASP.NET Core que permite gestionar clubs de natación, nadadores, y sus actividades. El proyecto está construido siguiendo los principios de Clean Architecture, garantizando un código mantenible, testeable y escalable.

## Características

- Gestión completa de clubs de natación
- Registro y seguimiento de nadadores
- Arquitectura limpia y desacoplada
- Endpoints minimalistas (sin controladores tradicionales)
- Documentación automática con Swagger/OpenAPI
- Suite completa de pruebas unitarias
- Soporte para contenedores Docker
- Base de datos PostgreSQL
- Entity Framework Core con migraciones

## Arquitectura

El proyecto sigue los principios de **Clean Architecture** organizado en capas:

```
┌─────────────────────────────────────────────────┐
│             SwimTracker.Api (Presentación)      │
│  - Endpoints minimalistas                       │
│  - Configuración de servicios                   │
│  - Swagger/OpenAPI                              │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│      SwimTracker.Application (Casos de Uso)     │
│  - Handlers de peticiones                       │
│  - DTOs (Requests/Responses)                    │
│  - Interfaces de repositorios                   │
│  - Lógica de negocio                            │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│        SwimTracker.Domain (Entidades)           │
│  - Entidades del dominio                        │
│  - Errores del dominio                          │
│  - Reglas de negocio                            │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│   SwimTracker.Infrastructure (Infraestructura)  │
│  - DbContext de Entity Framework                │
│  - Implementación de repositorios               │
│  - Migraciones de base de datos                 │
│  - Servicios externos                           │
└─────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────┐
│     SwimTracker.SharedKernel (Compartido)       │
│  - Clases base (Entity, AuditableEntity)        │
│  - Result pattern                               │
│  - Error handling                               │
│  - Interfaces comunes                           │
└─────────────────────────────────────────────────┘
```

## Tecnologías

- **Framework**: .NET 10.0
- **Lenguaje**: C# 13
- **Base de Datos**: PostgreSQL 16
- **ORM**: Entity Framework Core 10
- **Documentación**: Swagger/OpenAPI
- **Testing**: xUnit, Moq, FluentAssertions
- **Contenedores**: Docker & Docker Compose

### Paquetes Principales

- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Swashbuckle.AspNetCore`
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`

## Estructura del Proyecto

```
swim-tracker/
├── src/
│   ├── SwimTracker.Api/                    # Capa de presentación
│   │   ├── Endpoints/                      # Endpoints minimalistas
│   │   │   ├── Clubs/                      # Endpoints de clubs
│   │   │   └── Swimmers/                   # Endpoints de nadadores
│   │   ├── Extensions/                     # Métodos de extensión
│   │   ├── Properties/                     # Configuración de lanzamiento
│   │   ├── Program.cs                      # Punto de entrada
│   │   ├── appsettings.json               # Configuración
│   │   └── Dockerfile                      # Imagen Docker
│   │
│   ├── SwimTracker.Application/            # Capa de aplicación
│   │   ├── Abstractions/                   # Interfaces y contratos
│   │   │   ├── Data/                       # Interfaces de datos
│   │   │   └── Messaging/                  # IRequestHandler, IHandler
│   │   ├── Clubs/                          # Casos de uso de clubs
│   │   │   ├── CreateClub/                 # Crear club
│   │   │   ├── GetClub/                    # Obtener club por ID
│   │   │   └── GetClubs/                   # Listar clubs
│   │   ├── Swimmers/                       # Casos de uso de nadadores
│   │   │   ├── CreateSwimmer/              # Crear nadador
│   │   │   ├── GetSwimmer/                 # Obtener nadador por ID
│   │   │   └── GetSwimmers/                # Listar nadadores
│   │   └── DependencyInjection.cs         # Registro de servicios
│   │
│   ├── SwimTracker.Domain/                 # Capa de dominio
│   │   ├── Club.cs                         # Entidad Club
│   │   ├── ClubErrors.cs                   # Errores de Club
│   │   ├── Swimmer.cs                      # Entidad Swimmer
│   │   └── SwimErrors.cs                   # Errores de Swimmer
│   │
│   ├── SwimTracker.Infrastructure/         # Capa de infraestructura
│   │   ├── Migrations/                     # Migraciones de EF Core
│   │   ├── Persistence/                    # Contexto y repositorios
│   │   │   ├── SwimTrackerDbContext.cs    # DbContext principal
│   │   │   └── Repositories/               # Implementación repositorios
│   │   ├── Time/                           # Servicios de tiempo
│   │   └── DependencyInjection.cs         # Registro de servicios
│   │
│   └── SwimTracker.SharedKernel/           # Código compartido
│       ├── Entity.cs                       # Clase base para entidades
│       ├── AuditableEntity.cs             # Entidad con auditoría
│       ├── Result.cs                       # Result pattern
│       ├── Error.cs                        # Manejo de errores
│       └── Interfaces/                     # Interfaces comunes
│
├── tests/
│   └── SwimTracker.Api.UnitTests/          # Pruebas unitarias
│       ├── Endpoints/
│       │   ├── Clubs/                      # Tests de endpoints clubs
│       │   └── Swimmers/                   # Tests de endpoints nadadores
│       └── README.md                       # Documentación de tests
│
├── infra/
│   └── postgres/                           # Configuración PostgreSQL
│
├── docker-compose.yml                      # Orquestación de contenedores
├── swim-tracker.slnx                       # Solución de VS 2025
└── README.md                               # Este archivo
```

## Requisitos Previos

- [.NET SDK 10.0+](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 16+](https://www.postgresql.org/download/) o [Docker](https://www.docker.com/)
- [Visual Studio 2025](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/swim-tracker.git
cd swim-tracker
```

### 2. Configurar Base de Datos

#### Opción A: Usar Docker Compose (Recomendado)

```bash
docker-compose up -d postgres
```

Esto levantará PostgreSQL en el puerto 5432 con las credenciales:
- **Usuario**: `swim`
- **Contraseña**: `swim_secret`
- **Base de datos**: `swimtracker`

#### Opción B: Usar PostgreSQL Local

Edita `src/SwimTracker.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DbConnectionString": "Host=localhost;Port=5432;Database=swimtracker;Username=tu_usuario;Password=tu_password"
  }
}
```

### 3. Ejecutar Migraciones

```bash
cd src/SwimTracker.Api
dotnet ef database update
```

### 4. Restaurar Dependencias

```bash
dotnet restore
```

## Ejecución

### Ejecutar Localmente

```bash
cd src/SwimTracker.Api
dotnet run
```

La API estará disponible en:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger UI**: https://localhost:5001/swagger

### Ejecutar con Docker

```bash
docker-compose up --build
```

### Modo de Desarrollo con Hot Reload

```bash
cd src/SwimTracker.Api
dotnet watch run
```

## Endpoints Disponibles

### Clubs

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/clubs` | Obtener todos los clubs |
| `GET` | `/api/clubs/{id}` | Obtener un club por ID |
| `POST` | `/api/clubs` | Crear un nuevo club |

#### Ejemplo: Crear Club

```bash
POST /api/clubs
Content-Type: application/json

{
  "name": "Club Natación Barcelona",
  "acronym": "CNB",
  "countryCode": "ES",
  "city": "Barcelona",
  "email": "info@cnbarcelona.com"
}
```

### Swimmers (Nadadores)

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/swimmers` | Obtener todos los nadadores |
| `GET` | `/api/swimmers/{id}` | Obtener un nadador por ID |
| `POST` | `/api/swimmers` | Crear un nuevo nadador |

#### Ejemplo: Crear Nadador

```bash
POST /api/swimmers
Content-Type: application/json

{
  "clubId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Michael",
  "lastName": "Phelps",
  "dateOfBirth": "1985-06-30",
  "gender": "Male",
  "nationality": "US",
  "email": "mphelps@example.com",
  "phone": "+1-555-0123",
  "licenseNumber": "USA-MP-001",
  "licenseStatus": "Active",
  "licenseExpiresAt": "2026-12-31"
}
```

### Documentación Interactiva

Accede a **Swagger UI** para explorar y probar todos los endpoints:

```
https://localhost:5001/swagger
```

## Testing

El proyecto incluye una suite completa de **pruebas unitarias**.

### Ejecutar Todas las Pruebas

```bash
cd tests/SwimTracker.Api.UnitTests
dotnet test
```

### Ejecutar con Detalles

```bash
dotnet test --verbosity detailed
```

### Ejecutar Pruebas Específicas

```bash
# Solo pruebas de Clubs
dotnet test --filter "FullyQualifiedName~Clubs"

# Solo pruebas de Swimmers
dotnet test --filter "FullyQualifiedName~Swimmers"
```

### Cobertura de Código

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Estadísticas de Pruebas

- **20 pruebas unitarias**
- **100% de éxito**
- **6 clases de pruebas**
- Cobertura de casos de éxito y error

Ver más detalles en [tests/SwimTracker.Api.UnitTests/README.md](tests/SwimTracker.Api.UnitTests/README.md)

## Docker

### Construir Imagen

```bash
docker build -t swimtracker-api -f src/SwimTracker.Api/Dockerfile .
```

### Ejecutar Contenedor

```bash
docker run -d -p 5000:8080 \
  -e ConnectionStrings__DbConnectionString="Host=postgres;Port=5432;Database=swimtracker;Username=swim;Password=swim_secret" \
  --name swimtracker-api \
  swimtracker-api
```

### Docker Compose

```bash
# Levantar todos los servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Detener servicios
docker-compose down

# Detener y eliminar volúmenes
docker-compose down -v
```

## Base de Datos

### Crear Nueva Migración

```bash
cd src/SwimTracker.Api
dotnet ef migrations add NombreDeMigracion
```

### Aplicar Migraciones

```bash
dotnet ef database update
```

### Revertir Migración

```bash
dotnet ef database update NombreMigracionAnterior
```

### Ver Migraciones

```bash
dotnet ef migrations list
```

## Modelo de Datos

### Club

```csharp
- Id: Guid
- Name: string
- Acronym: string
- CountryCode: string
- City: string
- Address: string?
- Phone: string?
- Email: string
- FederationMemberId: string?
- LogoUrl: string?
- CreatedAt: DateTime
- UpdatedAt: DateTime
```

### Swimmer

```csharp
- Id: Guid
- ClubId: Guid
- FirstName: string
- LastName: string
- DateOfBirth: DateOnly
- Gender: string
- Nationality: string
- Email: string?
- Phone: string?
- LicenseNumber: string?
- LicenseStatus: string?
- LicenseExpiresAt: DateOnly?
- CreatedAt: DateTime
- UpdatedAt: DateTime
```

## Configuración

### Variables de Entorno

```bash
# Base de datos
ConnectionStrings__DbConnectionString="Host=localhost;Port=5432;Database=swimtracker;Username=swim;Password=swim_secret"

# Ambiente
ASPNETCORE_ENVIRONMENT=Development

# Puerto
ASPNETCORE_HTTP_PORTS=8080
```

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DbConnectionString": "Host=localhost;Port=5432;Database=swimtracker;Username=swim;Password=swim_secret"
  },
  "AllowedHosts": "*"
}
```

## Principios de Diseño

- **Clean Architecture**: Separación clara de responsabilidades
- **SOLID Principles**: Código mantenible y extensible
- **Result Pattern**: Manejo elegante de errores
- **Repository Pattern**: Abstracción de acceso a datos
- **Minimal APIs**: Endpoints ligeros sin controladores pesados
- **Dependency Injection**: Inversión de control nativa
- **Unit Testing**: Pruebas aisladas y rápidas

## Contribución

Las contribuciones son bienvenidas! Por favor, sigue estos pasos:

1. **Fork** el proyecto
2. Crea tu **Feature Branch** (`git checkout -b feature/AmazingFeature`)
3. **Commit** tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. **Push** a la rama (`git push origin feature/AmazingFeature`)
5. Abre un **Pull Request**

### Estándares de Código

- Usa **C# 13** y características modernas de .NET
- Sigue las convenciones de nombres de .NET
- Escribe **pruebas unitarias** para nuevas funcionalidades
- Documenta métodos públicos con **XML comments**
- Mantén la **cobertura de código** alta

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo `LICENSE` para más detalles.

## Autores

- **Alexi Caballero Esteban** - [alexicaballero](https://github.com/alexicaballero)

## Roadmap

- [ ] Autenticación y autorización con JWT
- [ ] Gestión de competiciones
- [ ] Registro de tiempos y marcas
- [ ] Sistema de rankings
- [ ] API de notificaciones
- [ ] Integración con sistemas de cronometraje
- [ ] Dashboard web (React/Vue)
- [ ] App móvil (React Native/Flutter)

---

Si este proyecto te ha sido útil, considera darle una estrella en GitHub!

**Hecho con ❤️ y ASP.NET Core**
