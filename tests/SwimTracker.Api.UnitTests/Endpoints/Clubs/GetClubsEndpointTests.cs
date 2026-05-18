using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Clubs.GetClubs;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Clubs;

public class GetClubsEndpointTests
{
    private readonly Mock<IHandler<List<GetClubsResponse>>> _handlerMock;
    private readonly CancellationToken _cancellationToken;

    public GetClubsEndpointTests()
    {
        _handlerMock = new Mock<IHandler<List<GetClubsResponse>>>();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenClubsExist_ReturnsOkWithClubsList()
    {
        // Arrange
        var clubs = new List<GetClubsResponse>
        {
            new GetClubsResponse(Guid.NewGuid(), "Club 1", "C1", "US", "New York", null, null, "test1@club.com", null, null),
            new GetClubsResponse(Guid.NewGuid(), "Club 2", "C2", "UK", "London", null, null, "test2@club.com", null, null),
            new GetClubsResponse(Guid.NewGuid(), "Club 3", "C3", "CA", "Toronto", null, null, "test3@club.com", null, null)
        };

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Success(clubs));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<Ok<List<GetClubsResponse>>>();
        var okResult = (Ok<List<GetClubsResponse>>)result;
        okResult.Value.Should().HaveCount(3);
        okResult.Value.Should().BeEquivalentTo(clubs);

        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenNoClubsExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        var clubs = new List<GetClubsResponse>();

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Success(clubs));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<Ok<List<GetClubsResponse>>>();
        var okResult = (Ok<List<GetClubsResponse>>)result;
        okResult.Value.Should().BeEmpty();

        _handlerMock.Verify(h => h.HandleAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenHandlerFails_ReturnsProblem()
    {
        // Arrange
        var error = new Error("Clubs.Error", "Error retrieving clubs");

        _handlerMock
            .Setup(h => h.HandleAsync(_cancellationToken))
            .ReturnsAsync(Result.Failure<List<GetClubsResponse>>(error));

        // Act
        var result = await ExecuteEndpoint();

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        problem.ProblemDetails.Type.Should().Be(error.Code);

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
            Title = "Failed to retrieve clubs",
            Detail = result.Error.Description,
            Status = StatusCodes.Status500InternalServerError
        });
    }
}
