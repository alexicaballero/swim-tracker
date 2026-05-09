using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Swimmers.CreateSwimmer;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Swimmers;

public class CreateSwimmerEndpointTests
{
    private readonly Mock<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>> _handlerMock;
    private readonly CancellationToken _cancellationToken;

    public CreateSwimmerEndpointTests()
    {
        _handlerMock = new Mock<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>>();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenRequestIsSuccessful_ReturnsOkResult()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var request = new CreateSwimmerRequest(
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

        var response = new CreateSwimmerResponse(
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
            .ReturnsAsync(Result.Success(response));

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<Ok<CreateSwimmerResponse>>();
        var okResult = (Ok<CreateSwimmerResponse>)result;
        okResult.Value.Should().BeEquivalentTo(response);

        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenRequestFails_ReturnsInternalServerError()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var request = new CreateSwimmerRequest(
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

        var error = new Error("Swimmer.Error", "Failed to create swimmer");
        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<CreateSwimmerResponse>(error));

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<InternalServerError>();

        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithMinimalData_CallsHandler()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var request = new CreateSwimmerRequest(
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

        var response = new CreateSwimmerResponse(
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
            .ReturnsAsync(Result.Success(response));

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<Ok<CreateSwimmerResponse>>();
        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    private async Task<IResult> ExecuteEndpoint(CreateSwimmerRequest request)
    {
        // Simula la lógica del endpoint
        var handler = _handlerMock.Object;
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.InternalServerError();
        }
    }
}
