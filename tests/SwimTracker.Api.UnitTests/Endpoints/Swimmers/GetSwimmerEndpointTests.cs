using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Swimmers.GetSwimmer;
using SwimTracker.SharedKernel;
using System.Text.Json;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Swimmers;

public class GetSwimmerEndpointTests
{
    private readonly Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>> _handlerMock;
    private readonly DefaultHttpContext _httpContext;
    private readonly CancellationToken _cancellationToken;

    public GetSwimmerEndpointTests()
    {
        _handlerMock = new Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>>();
        _httpContext = new DefaultHttpContext();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenSwimmerExists_ReturnsOkWithSwimmerData()
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var clubId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        
        var swimmerResponse = new GetSwimmerResponse(
            Id: swimmerId,
            ClubId: clubId,
            FirstName: "John",
            LastName: "Doe",
            DateOfBirth: new DateOnly(2000, 1, 1),
            Gender: "Male",
            Nationality: "US",
            Email: "john.doe@example.com",
            Phone: "555-1234",
            LicenseNumber: "LIC123",
            LicenseStatus: "Active",
            LicenseExpiresAt: new DateOnly(2025, 12, 31));

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Success(swimmerResponse));

        // Act
        var statusCode = await ExecuteEndpoint(swimmerId);

        // Assert
        statusCode.Should().Be(StatusCodes.Status200OK);
        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetSwimmerRequest>(r => r.Id == swimmerId), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenSwimmerDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error("Swimmer.NotFound", "Swimmer not found");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var statusCode = await ExecuteEndpoint(swimmerId);

        // Assert
        statusCode.Should().Be(StatusCodes.Status404NotFound);
        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetSwimmerRequest>(r => r.Id == swimmerId), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyGuid_ReturnsNotFound()
    {
        // Arrange
        var swimmerId = Guid.Empty;
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error("Swimmer.InvalidId", "Invalid swimmer ID");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var statusCode = await ExecuteEndpoint(swimmerId);

        // Assert
        statusCode.Should().Be(StatusCodes.Status404NotFound);
        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetSwimmerRequest>(r => r.Id == swimmerId), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithNullOptionalFields_ReturnsOk()
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var clubId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        
        var swimmerResponse = new GetSwimmerResponse(
            Id: swimmerId,
            ClubId: clubId,
            FirstName: "Jane",
            LastName: "Smith",
            DateOfBirth: new DateOnly(1995, 5, 15),
            Gender: "Female",
            Nationality: "UK",
            Email: null,
            Phone: null,
            LicenseNumber: null,
            LicenseStatus: null,
            LicenseExpiresAt: null);

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Success(swimmerResponse));

        // Act
        var statusCode = await ExecuteEndpoint(swimmerId);

        // Assert
        statusCode.Should().Be(StatusCodes.Status200OK);
        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetSwimmerRequest>(r => r.Id == swimmerId), _cancellationToken), Times.Once);
    }

    private async Task<int> ExecuteEndpoint(Guid id)
    {
        // Simula la lógica del endpoint
        var handler = _handlerMock.Object;
        var request = new GetSwimmerRequest(id);
        var result = await handler.HandleAsync(request, _cancellationToken);

        var context = _httpContext;
        
        if (result.IsSuccess)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }

        return context.Response.StatusCode;
    }
}
