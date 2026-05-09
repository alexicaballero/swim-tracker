using SwimTracker.SharedKernel;

namespace SwimTracker.Domain;

/// <summary>
/// Represents a swimming club within the system.
/// </summary>
public sealed class Club : AuditableEntity
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the official club name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the short club acronym.
    /// </summary>
    public string Acronym { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the ISO country code of the club.
    /// </summary>
    public string CountryCode { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the city where the club operates.
    /// </summary>
    public string City { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the street address of the club, when available.
    /// </summary>
    public string? Address { get; private set; }

    /// <summary>
    /// Gets the contact phone number of the club, when available.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Gets the primary contact email address of the club.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the federation membership identifier, when available.
    /// </summary>
    public string? FederationMemberId { get; private set; }

    /// <summary>
    /// Gets the public URL of the club logo, when available.
    /// </summary>
    public string? LogoUrl { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the club is active.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Gets a value indicating whether the club has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }

    private Club()
    { }

    /// <summary>
    /// Creates a new club aggregate with its mandatory data.
    /// </summary>
    public static Club Create(
        string name, string acronym, string countryCode,
        string city, string email)
    {
        return new Club
        {
            Id = Guid.NewGuid(),
            Name = name,
            Acronym = acronym,
            CountryCode = countryCode,
            City = city,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates the editable fields of the club. Null values leave the current value unchanged.
    /// </summary>
    public void Update(
        string? acronym,
        string? countryCode,
        string? city,
        string? address,
        string? phone,
        string? email,
        string? federationMemberId,
        string? logoUrl)
    {
        if (acronym is not null) Acronym = acronym;
        if (countryCode is not null) CountryCode = countryCode;
        if (city is not null) City = city;
        if (address is not null) Address = address;
        if (phone is not null) Phone = phone;
        if (email is not null) Email = email;
        if (federationMemberId is not null) FederationMemberId = federationMemberId;
        if (logoUrl is not null) LogoUrl = logoUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}