using SwimTracker.Domain;
using SwimTracker.SharedKernel;
using SwimTracker.Application.Abstractions.Validation;

namespace SwimTracker.Application.Swimmers.CreateSwimmer;

public class CreateSwimmerValidator : IValidator<CreateSwimmerRequest>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateSwimmerValidator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public List<string> ValidateRequest(CreateSwimmerRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.FirstName))
            errors.Add(SwimmerErrors.FirstNameRequired.Description);

        if (string.IsNullOrWhiteSpace(request.LastName))
            errors.Add(SwimmerErrors.LastNameRequired.Description);

        if (request.DateOfBirth == default || request.DateOfBirth > DateOnly.FromDateTime(_dateTimeProvider.UtcNow))
            errors.Add(SwimmerErrors.InvalidDateOfBirth.Description);

        if (string.IsNullOrWhiteSpace(request.Gender))
            errors.Add(SwimmerErrors.GenderRequired.Description);

        if (string.IsNullOrWhiteSpace(request.Nationality))
            errors.Add(SwimmerErrors.NationalityRequired.Description);

        if (!string.IsNullOrWhiteSpace(request.Email) && !IsValidEmail(request.Email))
            errors.Add(SwimmerErrors.InvalidEmail.Description);

        if (!string.IsNullOrWhiteSpace(request.Phone) && !IsValidPhone(request.Phone))
            errors.Add(SwimmerErrors.InvalidPhone.Description);

        return errors;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhone(string phone)
    {
        try
        {
            var phoneNumber = System.Text.RegularExpressions.Regex.Replace(phone, @"\D", "");
            return phoneNumber.Length >= 10 && phoneNumber.Length <= 15;
        }
        catch
        {
            return false;
        }
    }
}
