using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SwimTracker.Api.UnitTests.ProblemDetails;

/// <summary>
/// Tests to validate RFC 9457 Problem Details format compliance.
/// </summary>
public class ProblemDetailsFormatTests
{
    [Fact]
    public void ProblemDetails_IncludesRequiredProperties_Type()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "https://example.com/errors/validation-failed",
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "The request contains invalid data",
            Instance = "POST /api/clubs"
        };

        // Act & Assert
        problemDetails.Type.Should().NotBeNullOrEmpty();
        problemDetails.Type.Should().Contain("validation-failed");
    }

    [Fact]
    public void ProblemDetails_IncludesRequiredProperties_Status()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "test-error",
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = "Resource not found"
        };

        // Act & Assert
        problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        problemDetails.Status.Should().BeGreaterThanOrEqualTo(400);
        problemDetails.Status.Should().BeLessThan(600);
    }

    [Fact]
    public void ProblemDetails_IncludesRequiredProperties_Title()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "test-error",
            Status = StatusCodes.Status400BadRequest,
            Title = "Bad Request",
            Detail = "Invalid request format"
        };

        // Act & Assert
        problemDetails.Title.Should().NotBeNullOrEmpty();
        problemDetails.Title.Length.Should().BeLessThan(255);
    }

    [Fact]
    public void ProblemDetails_IncludesOptionalProperties_Detail()
    {
        // Arrange
        var detail = "The club name must not be empty";
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "validation-error",
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = detail
        };

        // Act & Assert
        problemDetails.Detail.Should().NotBeNullOrEmpty();
        problemDetails.Detail.Should().Be(detail);
    }

    [Fact]
    public void ProblemDetails_IncludesOptionalProperties_Instance()
    {
        // Arrange
        var instance = "POST /api/swimmers";
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "creation-error",
            Status = StatusCodes.Status400BadRequest,
            Title = "Creation Failed",
            Detail = "Invalid data",
            Instance = instance
        };

        // Act & Assert
        problemDetails.Instance.Should().NotBeNullOrEmpty();
        problemDetails.Instance.Should().Be(instance);
    }

    [Fact]
    public void ProblemDetails_SupportsExtensionsForCustomProperties()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "test-error",
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal Server Error"
        };

        problemDetails.Extensions["requestId"] = "req-123456";
        problemDetails.Extensions["traceId"] = "trace-789";
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow.ToString("O");

        // Act & Assert
        problemDetails.Extensions.Should().ContainKey("requestId");
        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions.Should().ContainKey("timestamp");
        problemDetails.Extensions["requestId"].Should().Be("req-123456");
    }

    [Theory]
    [InlineData(StatusCodes.Status400BadRequest, "Bad Request")]
    [InlineData(StatusCodes.Status404NotFound, "Not Found")]
    [InlineData(StatusCodes.Status500InternalServerError, "Internal Server Error")]
    public void ProblemDetails_StatusCodesMapping(int statusCode, string expectedTitle)
    {
        // Arrange & Act
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "error",
            Status = statusCode,
            Title = expectedTitle
        };

        // Assert
        problemDetails.Status.Should().Be(statusCode);
        problemDetails.Title.Should().Be(expectedTitle);
    }

    [Fact]
    public void ProblemDetails_TypeCanBeUrl()
    {
        // Arrange & Act
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "https://swimtracker.com/errors/club-not-found",
            Status = StatusCodes.Status404NotFound,
            Title = "Club Not Found"
        };

        // Assert
        problemDetails.Type.Should().StartWith("https://");
    }

    [Fact]
    public void ProblemDetails_TypeCanBeUniqueIdentifier()
    {
        // Arrange & Act
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "Club.InvalidId",
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid Club ID"
        };

        // Assert
        problemDetails.Type.Should().Contain(".");
        problemDetails.Type.Should().NotContain("://");
    }

    [Fact]
    public void ProblemDetails_AllPropertiesCanBeSerializedToJson()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "validation-error",
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "Club name is required",
            Instance = "POST /api/clubs"
        };

        problemDetails.Extensions["requestId"] = "req-001";

        // Act
        var json = System.Text.Json.JsonSerializer.Serialize(problemDetails);

        // Assert
        json.Should().Contain("\"type\"");
        json.Should().Contain("\"status\"");
        json.Should().Contain("\"title\"");
        json.Should().Contain("\"detail\"");
        json.Should().Contain("\"instance\"");
        json.Should().Contain("\"requestId\"");
    }
}
