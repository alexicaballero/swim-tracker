using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimTracker.Domain;

namespace SwimTracker.Infrastructure.Persistence.Configurations;

internal class SwimmerConfiguration : IEntityTypeConfiguration<Swimmer>
{
    public void Configure(EntityTypeBuilder<Swimmer> builder)
    {
        builder.ToTable("swimmers");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.ClubId).HasColumnName("club_id");
        builder.Property(s => s.KeycloakUserId).HasColumnName("keycloak_user_id");
        builder.Property(s => s.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(s => s.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        builder.Property(s => s.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(s => s.Gender).HasColumnName("gender").HasMaxLength(20).IsRequired();
        builder.Property(s => s.Nationality).HasColumnName("nationality").HasMaxLength(3).IsRequired();
        builder.Property(s => s.Email).HasColumnName("email").HasMaxLength(150);
        builder.Property(s => s.Phone).HasColumnName("phone").HasMaxLength(30);
        builder.Property(s => s.LicenseNumber).HasColumnName("license_number").HasMaxLength(50);
        builder.Property(s => s.LicenseStatus).HasColumnName("license_status").HasMaxLength(20);
        builder.Property(s => s.LicenseExpiresAt).HasColumnName("license_expires_at");
        builder.Property(s => s.FederationAthleteId).HasColumnName("federation_athlete_id").HasMaxLength(50);
        builder.Property(s => s.GuardianConsent).HasColumnName("guardian_consent").HasDefaultValue(false);
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.UpdatedAt).HasColumnName("updated_at");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by");
        builder.Property(s => s.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);
        builder.Property(s => s.DeletedAt).HasColumnName("deleted_at");

        builder.HasIndex(s => s.ClubId);
        //builder.HasIndex(s => new { s.ClubId, s.LicenseNumber });
        //builder.HasIndex(s => s.KeycloakUserId);

        builder.HasOne<Club>()
            .WithMany()
            .HasForeignKey(s => s.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
