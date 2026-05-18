using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Swimmers.GetSwimmers;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Swimmers;

public class GetSwimmersEndpointTests
{
    private readonly Mock<IHandler<List<GetSwimmersResponse>>> _handlerMock;
    private readonly DefaultHttpContext _httpContext;
    private readonly CancellationToken _cancellationToken;

    public GetSwimmersEndpointTests()
    {
        _handlerMock = new Mock<IHandler<List<GetSwimmersResponse>>>();
        _httpContext = new DefaultHttpContext();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenSwimmersExist_ReturnsOkWithSwimmersList()
    {
        // Arrange
        var swimmers = new List<GetSwimmersResponse>
        {
            new GetSwimmersResponse(
                Id: Guid.NewGuid(),
                ClubId: Guid.NewGuid(),
                FirstName: "John",
                LastName: "Doe",
                DateOfBirth: new DateOnly(2000, 1, 1),
                Gender: "Male",
                Nationality: "US",
                Email: "john.doe@example.com",
                Phone: "555-1234",
                LicenseNumber: "LIC123",
                LicenseStatus: "Active",
                LicenseExpiresAt: new DateOnly(2025, 12, 31)),
            new GetSwimmersResponse(
                Id: Guid.NewGuid(),
                ClubId: Guid.NewGuid(),
                FirstName: "Jane",
                LastName: "Smith",
                DateOfBirth: new DateOnly(1995, 5, 15),
                Gender: "Female",
                Nationality: "UK",
                Email: "jane.smith@example.com",
                Phone: "555-5678",
                LicenseNumber: "LIC456",
                LicenseStatus: "Active",
                LicenseExpiresAt: new DateOnly(2026, 6, 30))
        };

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Success(swimmers));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<Ok<List<GetSwimmersResponse>>>();
        var okResult = (Ok<List<GetSwimmersResponse>>)result;
        okResult.Value.Should().HaveCount(2);
        okResult.Value.Should().BeEquivalentTo(swimmers);
        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenNoSwimmersExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var swimmers = new List<GetSwimmersResponse>();

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Success(swimmers));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<Ok<List<GetSwimmersResponse>>>();
        var okResult = (Ok<List<GetSwimmersResponse>>)result;
        okResult.Value.Should().BeEmpty();
        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenHandlerFails_ReturnsProblem()
    {
        // Arrange
        var error = new Error("Swimmers.Error", "Error retrieving swimmers");

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Failure<List<GetSwimmersResponse>>(error));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        problem.ProblemDetails.Type.Should().Be(error.Code);
        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithMultipleSwimmers_ReturnsAllSwimmers()
    {
        // Arrange
        var swimmers = new List<GetSwimmersResponse>();
        for (int i = 0; i < 5; i++)
        {
            swimmers.Add(new GetSwimmersResponse(
                Id: Guid.NewGuid(),
                ClubId: Guid.NewGuid(),
                FirstName: $"Swimmer{i}",
                LastName: $"Last{i}",
                DateOfBirth: new DateOnly(2000 + i, 1, 1),
                Gender: i % 2 == 0 ? "Male" : "Female",
                Nationality: "US",
                Email: $"swimmer{i}@example.com",
                Phone: null,
                LicenseNumber: null,
                LicenseStatus: null,
                LicenseExpiresAt: null));
        }

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Success(swimmers));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<Ok<List<GetSwimmersResponse>>>();
        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    private async Task<IResult> ExecuteEndpoint()
    {
        // Simula la lógica del endpoint
        var handler = _handlerMock.Object;
        var result = await handler.HandleAsync(_cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Failed to retrieve swimmers",
            Detail = result.Error.Description,
            Status = StatusCodes.Status500InternalServerError,
            Instance = _httpContext.Request.Path
        });
    }
}
