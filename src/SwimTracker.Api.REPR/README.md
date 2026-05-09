# SwimTracker.Api.REPR

> **Proyecto de demostración** del patrón **REPR** (Request-Endpoint-Response) en ASP.NET Core

## Propósito del Proyecto

Este proyecto fue creado específicamente para **ejemplificar y estudiar** el patrón REPR como alternativa moderna a los controladores tradicionales en ASP.NET Core. Implementa la misma funcionalidad (gestión de clubes de natación y nadadores) usando **dos enfoques diferentes**:

1. **Endpoints de Clubs** → Implementados con **FastEndpoints**
2. **Endpoints de Swimmers** → Implementados con **código personalizado** (sin librerías externas)

Esto permite comparar ambas aproximaciones en el mismo proyecto y evaluar cuál se adapta mejor a diferentes necesidades.

## ¿Qué es el Patrón REPR?

**REPR** (**Re**quest-**E**nd**P**oint-**R**esponse) es un patrón arquitectónico que organiza el código de APIs alrededor de **endpoints individuales** en lugar de **controladores monolíticos**.

### Comparación Visual

```
Enfoque Tradicional (Controllers)
ClubsController.cs           ← Una clase con múltiples responsabilidades
├─ GetClub()                 
├─ CreateClub()             
├─ GetClubs()
└─ UpdateClub()

Patrón REPR (Endpoints)
Endpoints/Clubs/             ← Una carpeta, una responsabilidad por archivo
├─ GetClub.cs                ← Una clase = Una operación
├─ CreateClub.cs             ← Fácil de encontrar y modificar
├─ GetClubs.cs               ← Sin conflictos de merge
└─ UpdateClub.cs             ← Testeable aisladamente
```

### Beneficios Demostrados

- **Single Responsibility Principle**: Cada endpoint tiene una única razón para cambiar
- **Mejor Testabilidad**: Clases pequeñas más fáciles de probar
- **Escalabilidad**: Equipos trabajan en paralelo sin conflictos
- **Mantenibilidad**: Localización inmediata de la lógica
- **Configuración Granular**: Cada endpoint tiene su propia política

## Estructura del Proyecto

```
SwimTracker.Api.REPR/
│
├── Program.cs                          ← Configuración de FastEndpoints + Endpoints personalizados
├── appsettings.json
│
├── Endpoints/
│   ├── IEndpoint.cs                    ← Interfaz para endpoints personalizados
│   │
│   ├── Clubs/                          ← Implementación con FastEndpoints
│   │   ├── GetClub.cs                  ← Endpoint<TRequest, TResponse>
│   │   ├── CreateClub.cs               ← Configuración fluida
│   │   └── GetClubs.cs                 ← EndpointWithoutRequest<TResponse>
│   │
│   └── Swimmers/                       ← Implementación Personalizada
│       ├── GetSwimmer.cs               ← IEndpoint + MapEndpoint()
│       ├── CreateSwimmer.cs            ← IResult + HandleAsync()
│       └── GetSwimmers.cs              ← API Minimal de ASP.NET Core
│
└── Extensions/
    └── EndpointExtensions.cs           ← Registro automático de endpoints personalizados
```

## Dos Enfoques, Misma Funcionalidad

### Enfoque 1: FastEndpoints (Clubs)

```csharp
public class GetClub : Endpoint<GetClubRequest, ClubResponse>
{
    private readonly IRequestHandler<GetClubRequest, ClubResponse> _requestHandler;

    public GetClub(IRequestHandler<GetClubRequest, ClubResponse> requestHandler)
        => _requestHandler = requestHandler;

    public override void Configure()
    {
        Get("/clubs/{id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetClubRequest req, CancellationToken ct)
    {
        var result = await _requestHandler.HandleAsync(req, ct);
        
        if (result.IsFailure)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}
```

**Ventajas**:
- API fluida y expresiva
- Validación integrada
- Documentación Swagger automática
- Muchas características listas para usar
- Productividad inmediata

### Enfoque 2: Implementación Personalizada (Swimmers)

```csharp
public class GetSwimmer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/swimmers/{id:guid}", HandleAsync)
            .WithName("GetSwimmer")
            .WithTags("Swimmers")
            .Produces<GetSwimmerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }

    private async Task<IResult> HandleAsync(
        Guid id,
        IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> requestHandler,
        CancellationToken cancellationToken)
    {
        var request = new GetSwimmerRequest(id);
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        return result.IsFailure
            ? Results.NotFound()
            : Results.Ok(result.Value);
    }
}
```

**Ventajas**:
- Cero dependencias externas
- Control total del código
- Usa API Minimal estándar de ASP.NET Core
- Solución extremadamente ligera
- Fácil de entender y mantener

## Cómo Ejecutar el Proyecto

### Requisitos Previos

- .NET 10.0 SDK o superior
- PostgreSQL 16+ o Docker
- Visual Studio 2025, Rider o VS Code

### Paso 1: Iniciar Base de Datos

```bash
# Desde la raíz del repositorio
docker compose up -d postgres
```

Esto levanta PostgreSQL con las credenciales:
- **Usuario**: `swim`
- **Contraseña**: `swim_secret`
- **Base de datos**: `swimtracker`
- **Puerto**: `5432`

### Paso 2: Ejecutar el Proyecto

```bash
# Desde la raíz del repositorio
dotnet run --project src/SwimTracker.Api.REPR/SwimTracker.Api.REPR.csproj
```

O directamente desde la carpeta del proyecto:

```bash
cd src/SwimTracker.Api.REPR
dotnet run
```

### Paso 3: Explorar los Endpoints

Abre tu navegador en: **http://localhost:5000/swagger**

## API Endpoints Disponibles

### Clubs (FastEndpoints)

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/clubs` | Obtener todos los clubes |
| `GET` | `/api/clubs/{id}` | Obtener un club por ID |
| `POST` | `/api/clubs` | Crear un nuevo club |

### Swimmers (Implementación Personalizada)

| Método | Ruta | Descripción |
|--------|------|-------------|
| `GET` | `/api/swimmers` | Obtener todos los nadadores |
| `GET` | `/api/swimmers/{id}` | Obtener un nadador por ID |
| `POST` | `/api/swimmers` | Crear un nuevo nadador |

## Ejemplos de Uso

### Crear un Club (FastEndpoints)

```bash
POST /api/clubs
Content-Type: application/json

{
  "name": "Club Natación Barcelona",
  "acronym": "CNB",
  "countryCode": "ES",
  "city": "Barcelona",
  "email": "info@cnb.com"
}
```

### Crear un Nadador (Implementación Personalizada)

```bash
POST /api/swimmers
Content-Type: application/json

{
  "clubId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "firstName": "Juan",
  "lastName": "Pérez",
  "dateOfBirth": "2000-05-15",
  "gender": "M",
  "nationality": "ES",
  "email": "juan.perez@example.com",
  "phone": "+34 600 123 456"
}
```

## Comparación de Enfoques

| Característica | FastEndpoints | Implementación Personalizada |
|----------------|---------------|------------------------------|
| **Dependencias externas** | FastEndpoints, FastEndpoints.Swagger | Ninguna |
| **Complejidad** | Media | Baja |
| **Configuración inicial** | 2 paquetes NuGet + configuración | Interface + Extension method |
| **Validación** | Integrada | Manual |
| **Documentación** | Automática | Manual con `.Produces()` |
| **Curva de aprendizaje** | Nueva API | API Minimal estándar |
| **Flexibilidad** | Alta (muchas opciones) | Total (código propio) |
| **Ideal para** | Proyectos grandes, equipos | Proyectos ligeros, control total |

## Material de Aprendizaje

Este proyecto está acompañado del artículo completo:

**[ARTICULO_REPR.md](./ARTICULO_REPR.md)** - Guía paso a paso de la migración de Controllers a REPR

El artículo cubre:
- Explicación detallada del patrón REPR
- Migración completa de `ClubsController` con FastEndpoints
- Migración completa de `SwimmersController` con implementación personalizada
- Comparación de ambos enfoques
- Mejores prácticas y recomendaciones

## ¿Cuándo Usar Cada Enfoque?

### Usa FastEndpoints si:

- Necesitas validación compleja integrada
- Quieres productividad inmediata
- Tu equipo valora convenciones y estructura
- No te importa añadir dependencias externas
- Necesitas características avanzadas (versionado, rate limiting, etc.)

### Usa Implementación Personalizada si:

- Quieres cero dependencias de terceros
- Prefieres total control sobre el código
- Tu equipo ya conoce bien ASP.NET Core Minimal APIs
- Necesitas una solución extremadamente ligera
- Valoras la simplicidad sobre las características

## Arquitectura Clean

Este proyecto sigue **Clean Architecture** con separación clara de capas:

- **Domain** (`SwimTracker.Domain`): Entidades y reglas de negocio
- **Application** (`SwimTracker.Application`): Casos de uso y contratos
- **Infrastructure** (`SwimTracker.Infrastructure`): Implementación de datos (EF Core)
- **Presentation** (`SwimTracker.Api.REPR`): **Esta capa** - Endpoints REPR

### Patrones Implementados

- **REPR Pattern**: Request-Endpoint-Response
- **Result Pattern**: Manejo funcional de errores
- **Repository Pattern**: Abstracción de datos
- **Handler Pattern**: CQRS simplificado
- **Clean Architecture**: Inversión de dependencias

## Recursos Adicionales

- **REPR Design Pattern**: [DevIQ - REPR Pattern Guide](https://deviq.com/design-patterns/repr-design-pattern/)
- **FastEndpoints Documentation**: [https://fast-endpoints.com/](https://fast-endpoints.com/)
- **ASP.NET Core Minimal APIs**: [Microsoft Docs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- **Clean Architecture**: [Uncle Bob Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Objetivo Educativo Cumplido

Este proyecto demuestra que:

1. **REPR es viable** como alternativa a Controllers tradicionales
2. **Existen múltiples formas** de implementar REPR en .NET
3. **Cada enfoque tiene sus ventajas** según el contexto
4. **La elección depende** de las necesidades del proyecto y equipo
5. **Ambos enfoques conviven** perfectamente en el mismo proyecto

---

**¿Tienes dudas sobre el patrón REPR?** Revisa el [artículo completo](../../ARTICULO_REPR.md) para una guía detallada paso a paso.

**¿Quieres contribuir?** Este es un proyecto educativo - cualquier mejora o corrección es bienvenida.
