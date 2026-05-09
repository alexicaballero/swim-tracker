using SwimTracker.SharedKernel;

namespace SwimTracker.Domain;

/// <summary>
/// Represents a swimmer registered in a club.
/// </summary>
public sealed class Swimmer : AuditableEntity
{
    /// <summary>
    /// Gets the unique identifier of the swimmer.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the identifier of the club that owns the swimmer.
    /// </summary>
    public Guid ClubId { get; private set; }

    /// <summary>
    /// Gets the linked Keycloak user identifier, when the swimmer has been invited.
    /// </summary>
    public Guid? KeycloakUserId { get; private set; }

    /// <summary>
    /// Gets the swimmer first name.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the swimmer last name.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the swimmer date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; private set; }

    /// <summary>
    /// Gets the swimmer gender.
    /// </summary>
    public string Gender { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the swimmer nationality.
    /// </summary>
    public string Nationality { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the swimmer email address, when available.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Gets the swimmer phone number, when available.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Gets the federation license number, when available.
    /// </summary>
    public string? LicenseNumber { get; private set; }

    /// <summary>
    /// Gets the current federation license status, when available.
    /// </summary>
    public string? LicenseStatus { get; private set; }

    /// <summary>
    /// Gets the federation license expiration date, when available.
    /// </summary>
    public DateOnly? LicenseExpiresAt { get; private set; }

    /// <summary>
    /// Gets the federation athlete identifier, when available.
    /// </summary>
    public string? FederationAthleteId { get; private set; }

    /// <summary>
    /// Gets a value indicating whether guardian consent has been recorded.
    /// </summary>
    public bool GuardianConsent { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the swimmer is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets the identifier of the user who created the swimmer.
    /// </summary>
    public Guid CreatedBy { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the swimmer has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Gets the UTC timestamp when the swimmer was soft-deleted, when applicable.
    /// </summary>
    public DateTime? DeletedAt { get; private set; }

    /// <summary>
    /// Returns true when the license expires within the next 30 days.
    /// </summary>
    public bool IsLicenseExpiringSoon =>
        LicenseExpiresAt.HasValue &&
        LicenseExpiresAt.Value <= DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30));

    private Swimmer()
    { }

    /// <summary>
    /// Creates a new <see cref="Swimmer"/> aggregate with mandatory fields.
    /// </summary>
    public static Swimmer Create(
        Guid clubId, string firstName, string lastName,
        DateOnly dateOfBirth, string gender, string nationality,
        string? email, string? phone, string? licenseNumber,
        string? licenseStatus, DateOnly? licenseExpiresAt,
        Guid createdBy)
    {
        return new Swimmer
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Nationality = nationality,
            Email = email,
            Phone = phone,
            LicenseNumber = licenseNumber,
            LicenseStatus = licenseStatus,
            LicenseExpiresAt = licenseExpiresAt,
            CreatedBy = createdBy,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates the editable fields of the swimmer.
    /// </summary>
    public void Update(
        string firstName, string lastName, string gender, string nationality,
        string? email, string? phone, string? licenseNumber,
        bool guardianConsent)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Nationality = nationality;
        Email = email;
        Phone = phone;
        LicenseNumber = licenseNumber;
        GuardianConsent = guardianConsent;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Soft-deletes the swimmer from the club roster.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Links the swimmer to a Keycloak user account after a successful invitation.
    /// </summary>
    public void SetKeycloakUserId(Guid keycloakUserId)
    {
        KeycloakUserId = keycloakUserId;
        UpdatedAt = DateTime.UtcNow;
    }
}