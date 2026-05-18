namespace SwimTracker.Application.Abstractions.Validation;

public interface IValidator<in TRequest>
{
    List<string> ValidateRequest(TRequest request);
}
