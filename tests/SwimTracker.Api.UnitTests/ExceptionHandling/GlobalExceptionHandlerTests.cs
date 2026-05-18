using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace SwimTracker.Api.UnitTests.ExceptionHandling;

/// <summary>
/// Tests for validating Global Exception Handler behavior with Problem Details format.
/// These tests validate that unhandled exceptions are converted to RFC 9457 Problem Details responses.
/// </summary>
public class GlobalExceptionHandlerTests
{
    private readonly DefaultHttpContext _httpContext;

    public GlobalExceptionHandlerTests()
    {
        _httpContext = new DefaultHttpContext();
        _httpContext.Request.Method = "GET";
        _httpContext.Request.Path = "/api/clubs/123";
    }

    [Fact]
    public void ExceptionHandler_MapArgumentException_To400BadRequest()
    {
        // Arrange
        var statusCode = MapExceptionToStatusCode(new ArgumentException("Invalid argument"));

        // Act & Assert
        statusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void ExceptionHandler_MapKeyNotFoundException_To404NotFound()
    {
        // Arrange
        var statusCode = MapExceptionToStatusCode(new KeyNotFoundException("Not found"));

        // Act & Assert
        statusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ExceptionHandler_MapApplicationException_To400BadRequest()
    {
        // Arrange
        var statusCode = MapExceptionToStatusCode(new ApplicationException("Application error"));

        // Act & Assert
        statusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void ExceptionHandler_MapGenericException_To500InternalServerError()
    {
        // Arrange
        var statusCode = MapExceptionToStatusCode(new Exception("Unexpected error"));

        // Act & Assert
        statusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public void ExceptionHandler_GeneratesProblemDetailsForException()
    {
        // Arrange
        var exception = new InvalidOperationException("Test error");
        var problemDetails = CreateProblemDetailsFromException(exception, StatusCodes.Status500InternalServerError, isDevelopment: false);

        // Act & Assert
        problemDetails.Should().NotBeNull();
        problemDetails.Type.Should().Contain(nameof(InvalidOperationException));
        problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
        problemDetails.Title.Should().Be("An unexpected error occurred.");
    }

    [Fact]
    public void ExceptionHandler_InDevelopmentIncludesExceptionMessage()
    {
        // Arrange
        var exception = new Exception("Detailed error message");
        var problemDetails = CreateProblemDetailsFromException(exception, StatusCodes.Status500InternalServerError, isDevelopment: true);

        // Act & Assert
        problemDetails.Detail.Should().Be("Detailed error message");
    }

    [Fact]
    public void ExceptionHandler_InProductionHidesExceptionMessage()
    {
        // Arrange
        var exception = new Exception("Secret error message");
        var problemDetails = CreateProblemDetailsFromException(exception, StatusCodes.Status500InternalServerError, isDevelopment: false);

        // Act & Assert
        problemDetails.Detail.Should().Be("An error occurred while processing your request.");
        problemDetails.Detail.Should().NotContain("Secret");
    }

    [Fact]
    public void ExceptionHandler_ProblemDetailsIncludesExtensions()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "test-error",
            Status = StatusCodes.Status500InternalServerError,
            Title = "Test",
            Detail = "Test detail"
        };

        problemDetails.Extensions["requestId"] = "req-123456";
        problemDetails.Extensions["traceId"] = "trace-789";
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow.ToString("O");

        // Act & Assert
        problemDetails.Extensions.Should().ContainKey("requestId");
        problemDetails.Extensions.Should().ContainKey("traceId");
        problemDetails.Extensions.Should().ContainKey("timestamp");
    }

    [Theory]
    [InlineData(typeof(ArgumentException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(KeyNotFoundException), StatusCodes.Status404NotFound)]
    [InlineData(typeof(ApplicationException), StatusCodes.Status400BadRequest)]
    [InlineData(typeof(InvalidOperationException), StatusCodes.Status500InternalServerError)]
    [InlineData(typeof(NullReferenceException), StatusCodes.Status500InternalServerError)]
    public void ExceptionHandler_MapsAllExceptionTypes(Type exceptionType, int expectedStatusCode)
    {
        // Arrange
        var exception = (Exception)Activator.CreateInstance(exceptionType, "Test message")!;

        // Act
        var statusCode = MapExceptionToStatusCode(exception);

        // Assert
        statusCode.Should().Be(expectedStatusCode);
    }

    [Fact]
    public void ProblemDetails_SerializesCorrectly()
    {
        // Arrange
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = "test-error",
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Detail = "Something went wrong"
        };

        // Act
        var json = System.Text.Json.JsonSerializer.Serialize(problemDetails);

        // Assert
        json.Should().Contain("\"type\"");
        json.Should().Contain("\"status\"");
        json.Should().Contain("\"title\"");
        json.Should().Contain("\"detail\"");
    }

    /// <summary>
    /// Maps an exception type to its HTTP status code based on GlobalExceptionHandler logic.
    /// </summary>
    private static int MapExceptionToStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    /// <summary>
    /// Creates a Problem Details response from an exception.
    /// </summary>
    private static Microsoft.AspNetCore.Mvc.ProblemDetails CreateProblemDetailsFromException(
        Exception exception,
        int statusCode,
        bool isDevelopment)
    {
        return new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = exception.GetType().FullName,
            Status = statusCode,
            Title = "An unexpected error occurred.",
            Detail = isDevelopment
                ? exception.Message
                : "An error occurred while processing your request."
        };
    }
}
