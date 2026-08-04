using Microsoft.AspNetCore.Mvc;
using SwimTracker.SharedKernel;

namespace SwimTracker.Api.ResultPattern.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result) =>
        result.Match(
            value => Results.Ok(value),
            error => ToProblem(error));

    public static IResult ToHttpResult(this Result result) =>
        result.Match(
            () => Results.NoContent(),
            error => ToProblem(error));

    public static IResult ToHttpProblem(this Error error) => ToProblem(error);

    private static IResult ToProblem(Error error)
    {
        int status = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(new ProblemDetails
        {
            Type = error.Code,
            Detail = error.Description,
            Status = status
        });
    }
}