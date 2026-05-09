# SwimTracker.Api.UnitTests

Proyecto de pruebas unitarias para los endpoints del API de SwimTracker.

## Descripción

Este proyecto contiene pruebas unitarias para validar el comportamiento de los endpoints del API de SwimTracker. Las pruebas están diseñadas para ser rápidas, aisladas y no requieren dependencias externas como bases de datos o servicios web.

## Estructura del Proyecto

```
SwimTracker.Api.UnitTests/
├── Endpoints/
│   ├── Clubs/
│   │   ├── CreateClubEndpointTests.cs
│   │   ├── GetClubEndpointTests.cs
│   │   └── GetClubsEndpointTests.cs
│   └── Swimmers/
│       ├── CreateSwimmerEndpointTests.cs
│       ├── GetSwimmerEndpointTests.cs
│       └── GetSwimmersEndpointTests.cs
└── SwimTracker.Api.UnitTests.csproj
```

## Tecnologías Utilizadas

- **xUnit**: Framework de pruebas
- **Moq**: Librería para crear mocks y stubs
- **FluentAssertions**: Librería para assertions más legibles
- **.NET 10.0**: Framework de desarrollo

## Cobertura de Pruebas

### Endpoints de Clubs

#### CreateClubEndpointTests (3 pruebas)
- Creación exitosa de un club
- Manejo de errores en la creación
- Validación con datos vacíos

#### GetClubEndpointTests (3 pruebas)
- Obtención exitosa de un club por ID
- Manejo de club no encontrado
- Manejo de GUID vacío

#### GetClubsEndpointTests (3 pruebas)
- Obtención de lista de clubs
- Obtención de lista vacía
- Manejo de errores en la obtención

### Endpoints de Swimmers

#### CreateSwimmerEndpointTests (3 pruebas)
- Creación exitosa de un nadador
- Manejo de errores en la creación
- Creación con datos mínimos requeridos

#### GetSwimmerEndpointTests (4 pruebas)
- Obtención exitosa de un nadador por ID
- Manejo de nadador no encontrado
- Manejo de GUID vacío
- Obtención con campos opcionales nulos

#### GetSwimmersEndpointTests (4 pruebas)
- Obtención de lista de nadadores
- Obtención de lista vacía
- Manejo de errores en la obtención
- Obtención con múltiples nadadores

## Ejecución de Pruebas

### Ejecutar todas las pruebas
```bash
dotnet test
```

### Ejecutar pruebas con detalles
```bash
dotnet test --verbosity detailed
```

### Ejecutar pruebas de una clase específica
```bash
dotnet test --filter "FullyQualifiedName~CreateClubEndpointTests"
```

### Ejecutar una prueba específica
```bash
dotnet test --filter "FullyQualifiedName~CreateClubEndpointTests.HandleAsync_WhenRequestIsSuccessful_ReturnsCreatedResult"
```

### Generar reporte de cobertura
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Estadísticas

- **Total de pruebas**: 20
- **Pruebas exitosas**: 20
- **Pruebas fallidas**: 0
- **Cobertura estimada**: Alta cobertura de los endpoints principales

## Enfoque de las Pruebas

Las pruebas en este proyecto son **pruebas unitarias puras**:

1. **Aislamiento**: Cada prueba es independiente y no depende de otras
2. **Mocking**: Se usan mocks para simular las dependencias (IRequestHandler, IHandler)
3. **Velocidad**: Las pruebas son rápidas y pueden ejecutarse frecuentemente
4. **Sin dependencias externas**: No requieren base de datos, servicios web, o infraestructura externa
5. **Comportamiento del endpoint**: Se verifica que los endpoints retornen los resultados correctos según la respuesta del handler

## Patrón de Pruebas

Todas las pruebas siguen el patrón **AAA (Arrange-Act-Assert)**:

```csharp
[Fact]
public async Task HandleAsync_WhenRequestIsSuccessful_ReturnsCreatedResult()
{
    // Arrange - Configuración de datos y mocks
    var request = new CreateClubRequest(...);
    _handlerMock.Setup(...).ReturnsAsync(Result.Success());

    // Act - Ejecución del código a probar
    var result = await ExecuteEndpoint(request);

    // Assert - Verificación de resultados
    result.Should().BeOfType<Created<CreateClubRequest>>();
    _handlerMock.Verify(..., Times.Once);
}
```

## Convenciones de Nombres

Los nombres de las pruebas siguen el patrón:
```
MethodName_Scenario_ExpectedBehavior
```

Ejemplos:
- `HandleAsync_WhenRequestIsSuccessful_ReturnsCreatedResult`
- `HandleAsync_WhenClubDoesNotExist_ReturnsNotFound`
- `HandleAsync_WithEmptyGuid_CallsHandler`

## Mantenimiento

Al agregar nuevos endpoints al API:

1. Crear una nueva clase de pruebas en la carpeta correspondiente
2. Seguir el mismo patrón de las pruebas existentes
3. Mockear las dependencias necesarias
4. Probar escenarios de éxito y error
5. Verificar que los handlers se llamen correctamente

## Referencias

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Unit Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

## Últimos Resultados

```
Test summary: total: 20; failed: 0; succeeded: 20; skipped: 0
```

Todas las pruebas están pasando exitosamente.
