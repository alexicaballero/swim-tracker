using SwimTracker.SharedKernel;

namespace SwimTracker.Domain;

public static class SwimmerErrors
{
    public static readonly Error NotFound = new("Swimmer.NotFound", "The specified swimmer was not found.", ErrorType.NotFound);
    public static readonly Error InvalidName = new("Swimmer.InvalidName", "Swimmer name is required.", ErrorType.Validation);
    public static readonly Error InvalidCity = new("Swimmer.InvalidCity", "City is required.", ErrorType.Validation);
    public static readonly Error InvalidEmail = new("Swimmer.InvalidEmail", "Email is required.", ErrorType.Validation);
    public static readonly Error CreationFailed = new("Swimmer.CreationFailed", "Failed to create swimmer.", ErrorType.Failure);
    public static readonly Error InvalidDateOfBirth = new("Swimmer.InvalidDateOfBirth", "Date of birth cannot be in the future.", ErrorType.Validation);
    public static readonly Error FirstNameRequired = new("Swimmer.FirstNameRequired", "First name is required.", ErrorType.Validation);
    public static readonly Error LastNameRequired = new("Swimmer.LastNameRequired", "Last name is required.", ErrorType.Validation);
    public static readonly Error GenderRequired = new("Swimmer.GenderRequired", "Gender is required.", ErrorType.Validation);
    public static readonly Error NationalityRequired = new("Swimmer.NationalityRequired", "Nationality is required.", ErrorType.Validation);
    public static readonly Error InvalidPhone = new("Swimmer.InvalidPhone", "Phone number is invalid.", ErrorType.Validation);
    public static readonly Error OperationCancelled = new("Swimmer.OperationCancelled", "The operation was canceled.", ErrorType.Failure);
    public static readonly Error ClubNotFound = new("Swimmer.Club.NotFound", "The specified club was not found.", ErrorType.NotFound);

    public static Error ValidationFailed(string details) =>
        new("Swimmer.ValidationFailed", details, ErrorType.Validation);
}