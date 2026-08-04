namespace SwimTracker.SharedKernel;

public enum ErrorType
{
    Failure = 0,  // Unexpected errors → HTTP 500
    Validation = 1,  // Invalid input     → HTTP 400
    NotFound = 2,  // Resource missing  → HTTP 404
    Conflict = 3   // Inconsistent state → HTTP 409
}
