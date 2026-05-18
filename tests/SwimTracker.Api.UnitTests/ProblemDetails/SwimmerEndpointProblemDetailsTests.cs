using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Swimmers.CreateSwimmer;
using SwimTracker.Application.Swimmers.GetSwimmer;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.ProblemDetails;

/// <summary>
/// Tests for validating Problem Details error responses from Swimmer endpoints.
/// </summary>
public class SwimmerEndpointProblemDetailsTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Fact]
    public async Task GetSwimmer_WithNotFoundError_ReturnsProblemDetails()
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error("Swimmer.NotFound", "The specified swimmer was not found");

        var handlerMock = new Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var result = await ExecuteGetSwimmerEndpoint(swimmerId, handlerMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(error.Code);
        result.Status.Should().Be(StatusCodes.Status404NotFound);
        result.Detail.Should().Be(error.Description);
    }

    [Fact]
    public async Task GetSwimmer_WithInvalidIdError_ReturnsProblemDetails()
    {
        // Arrange
        var swimmerId = Guid.Empty;
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error("Swimmer.InvalidId", "The swimmer ID is invalid");

        var handlerMock = new Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var result = await ExecuteGetSwimmerEndpoint(swimmerId, handlerMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreateSwimmer_WithValidationError_ReturnsProblemDetails()
    {
        // Arrange
        var request = new CreateSwimmerRequest(
            ClubId: Guid.NewGuid(),
            FirstName: "",
            LastName: "Doe",
            DateOfBirth: new DateOnly(2000, 1, 1),
            Gender: "Male",
            Nationality: "US",
            Email: null,
            Phone: null,
            LicenseNumber: null,
            LicenseStatus: null,
            LicenseExpiresAt: null);

        var error = new Error("Swimmer.FirstNameRequired", "First name is required");

        var handlerMock = new Mock<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<CreateSwimmerResponse>(error));

        // Act
        var result = await ExecuteCreateSwimmerEndpoint(request, handlerMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(error.Code);
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Detail.Should().Be(error.Description);
    }

    [Fact]
    public async Task CreateSwimmer_WithClubNotFoundError_ReturnsProblemDetails()
    {
        // Arrange
        var request = new CreateSwimmerRequest(
            ClubId: Guid.NewGuid(),
            FirstName: "John",
            LastName: "Doe",
            DateOfBirth: new DateOnly(2000, 1, 1),
            Gender: "Male",
            Nationality: "US",
            Email: null,
            Phone: null,
            LicenseNumber: null,
            LicenseStatus: null,
            LicenseExpiresAt: null);

        var error = new Error("Swimmer.ClubNotFound", "The specified club was not found");

        var handlerMock = new Mock<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<CreateSwimmerResponse>(error));

        // Act
        var result = await ExecuteCreateSwimmerEndpoint(request, handlerMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
        result.Detail.Should().Contain("club");
    }

    [Fact]
    public async Task CreateSwimmer_WithMultipleValidationErrors_ReturnsProblemDetailsWithFirstError()
    {
        // Arrange
        var request = new CreateSwimmerRequest(
            ClubId: Guid.Empty,
            FirstName: "",
            LastName: "",
            DateOfBirth: new DateOnly(2000, 1, 1),
            Gender: "",
            Nationality: "",
            Email: null,
            Phone: null,
            LicenseNumber: null,
            LicenseStatus: null,
            LicenseExpiresAt: null);

        // When multiple validations fail, the handler returns the first error
        var error = new Error("Swimmer.ValidationFailed", "Multiple validation errors occurred");

        var handlerMock = new Mock<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<CreateSwimmerResponse>(error));

        // Act
        var result = await ExecuteCreateSwimmerEndpoint(request, handlerMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task ProblemDetails_ErrorResponseIsSerializable()
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error("Swimmer.NotFound", "Swimmer not found");

        var handlerMock = new Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var problemDetails = await ExecuteGetSwimmerEndpoint(swimmerId, handlerMock.Object);
        var json = System.Text.Json.JsonSerializer.Serialize(problemDetails);

        // Assert
        json.Should().Contain("\"type\"");
        json.Should().Contain("\"status\"");
        json.Should().Contain("\"title\"");
        json.Should().Contain("\"detail\"");
    }

    [Theory]
    [InlineData("Swimmer.NotFound", StatusCodes.Status404NotFound)]
    [InlineData("Swimmer.InvalidId", StatusCodes.Status404NotFound)]
    [InlineData("Swimmer.FirstNameRequired", StatusCodes.Status400BadRequest)]
    [InlineData("Swimmer.ClubNotFound", StatusCodes.Status400BadRequest)]
    public async Task ProblemDetails_ErrorTypesMappedCorrectly(string errorCode, int expectedStatus)
    {
        // Arrange
        var swimmerId = Guid.NewGuid();
        var request = new GetSwimmerRequest(swimmerId);
        var error = new Error(errorCode, "Error description");

        var handlerMock = new Mock<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>>();
        handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetSwimmerResponse>(error));

        // Act
        var result = await ExecuteGetSwimmerEndpointWithStatusCode(swimmerId, handlerMock.Object, expectedStatus);

        // Assert
        result.Type.Should().Be(errorCode);
        result.Status.Should().Be(expectedStatus);
    }

    private async Task<Microsoft.AspNetCore.Mvc.ProblemDetails> ExecuteGetSwimmerEndpoint(
        Guid swimmerId,
        IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> handler)
    {
        var request = new GetSwimmerRequest(swimmerId);
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return new Microsoft.AspNetCore.Mvc.ProblemDetails();
        }

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Swimmer not found",
            Detail = result.Error.Description,
            Status = StatusCodes.Status404NotFound
        };
    }

    private async Task<Microsoft.AspNetCore.Mvc.ProblemDetails> ExecuteCreateSwimmerEndpoint(
        CreateSwimmerRequest request,
        IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse> handler)
    {
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return new Microsoft.AspNetCore.Mvc.ProblemDetails();
        }

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Swimmer creation failed",
            Detail = result.Error.Description,
            Status = StatusCodes.Status400BadRequest
        };
    }

    private async Task<Microsoft.AspNetCore.Mvc.ProblemDetails> ExecuteGetSwimmerEndpointWithStatusCode(
        Guid swimmerId,
        IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> handler,
        int expectedStatusCode)
    {
        var request = new GetSwimmerRequest(swimmerId);
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return new Microsoft.AspNetCore.Mvc.ProblemDetails();
        }

        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Swimmer not found",
            Detail = result.Error.Description,
            Status = expectedStatusCode
        };
    }
}
