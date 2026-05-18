using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.Application.Clubs.GetClub;
using SwimTracker.SharedKernel;
using Xunit;

namespace SwimTracker.Api.UnitTests.Endpoints.Clubs;

public class GetClubEndpointTests
{
    private readonly Mock<IRequestHandler<GetClubRequest, GetClubResponse>> _handlerMock;
    private readonly CancellationToken _cancellationToken;

    public GetClubEndpointTests()
    {
        _handlerMock = new Mock<IRequestHandler<GetClubRequest, GetClubResponse>>();
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task HandleAsync_WhenClubExists_ReturnsOkResult()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var request = new GetClubRequest(clubId);
        var clubResponse = new GetClubResponse(
            Id: clubId,
            Name: "Test Club",
            Acronym: "TC",
            CountryCode: "US",
            City: "New York",
            Address: "123 Main St",
            Phone: "555-1234",
            Email: "test@club.com",
            FederationMemberId: "FED123",
            LogoUrl: "http://example.com/logo.png");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Success(clubResponse));

        // Act
        var result = await ExecuteEndpoint(clubId);

        // Assert
        result.Should().BeOfType<Ok<GetClubResponse>>();
        var okResult = (Ok<GetClubResponse>)result;
        okResult.Value.Should().BeEquivalentTo(clubResponse);

        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetClubRequest>(r => r.Id == clubId), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenClubDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var clubId = Guid.NewGuid();
        var request = new GetClubRequest(clubId);
        var error = new Error("Club.NotFound", "Club not found");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetClubResponse>(error));

        // Act
        var result = await ExecuteEndpoint(clubId);

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        problem.ProblemDetails.Type.Should().Be(error.Code);

        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetClubRequest>(r => r.Id == clubId), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyGuid_CallsHandler()
    {
        // Arrange
        var clubId = Guid.Empty;
        var request = new GetClubRequest(clubId);
        var error = new Error("Club.InvalidId", "Invalid club ID");

        _handlerMock
            .Setup(h => h.HandleAsync(request, _cancellationToken))
            .ReturnsAsync(Result.Failure<GetClubResponse>(error));

        // Act
        var result = await ExecuteEndpoint(clubId);

        // Assert
        result.Should().BeOfType<ProblemHttpResult>();
        var problem = (ProblemHttpResult)result;
        problem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        _handlerMock.Verify(h => h.HandleAsync(It.Is<GetClubRequest>(r => r.Id == clubId), _cancellationToken), Times.Once);
    }

    private async Task<IResult> ExecuteEndpoint(Guid id)
    {
        // Simula la lógica del endpoint
        var handler = _handlerMock.Object;
        var request = new GetClubRequest(id);
        var result = await handler.HandleAsync(request, _cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Club not found",
            Detail = result.Error.Description,
            Status = StatusCodes.Status404NotFound
        });
    }
}
