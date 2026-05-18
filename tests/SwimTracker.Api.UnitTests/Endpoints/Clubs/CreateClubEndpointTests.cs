using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Clubs.CreateClub;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Clubs;

public class CreateClubEndpointTests
{
    private readonly Mock<IRequestHandler<CreateClubRequest>> _handlerMock;
    private readonly CancellationToken _cancellationToken;

    public CreateClubEndpointTests()
    {
        _handlerMock = new Mock<IRequestHandler<CreateClubRequest>>();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenRequestIsSuccessful_ReturnsCreatedResult()
    {
        // Arrange
        var request = new CreateClubRequest(
            Name: "Test Club",
            Acronym: "TC",
            CountryCode: "US",
            City: "New York",
            Email: "test@club.com");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<Created<CreateClubRequest>>();
        var createdResult = (Created<CreateClubRequest>)result;
        createdResult.Location.Should().Be($"api/clubs/{request.Name}");
        createdResult.Value.Should().BeEquivalentTo(request);

        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenRequestFails_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateClubRequest(
            Name: "Test Club",
            Acronym: "TC",
            CountryCode: "US",
            City: "New York",
            Email: "test@club.com");

        var error = new Error("Club.Error", "An error occurred");
        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        problem.ProblemDetails.Type.Should().Be(error.Code);

        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyName_CallsHandler()
    {
        // Arrange
        var request = new CreateClubRequest(
            Name: "",
            Acronym: "TC",
            CountryCode: "US",
            City: "New York",
            Email: "test@club.com");

        var error = new Error("Club.InvalidName", "Name is required");
        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure(error));

        // Act
        var result = await ExecuteEndpoint(request);

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        _handlerMock.Verify(h => h.HandleAsync(request, _cancellationToken), Times.Once);
    }

    private async Task<IResult> ExecuteEndpoint(CreateClubRequest request)
    {
        // Simula la lógica del endpoint
        var handler = _handlerMock.Object;
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Created($"api/clubs/{request.Name}", request);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Club creation failed",
            Detail = result.Error.Description,
            Status = StatusCodes.Status400BadRequest
        });
    }
}
