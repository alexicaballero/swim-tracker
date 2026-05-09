using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimTracker.Domain;

namespace SwimTracker.Infrastructure.Persistence.Configurations;

internal class ClubConfiguration : IEntityTypeConfiguration<Club>
{
    public void Configure(EntityTypeBuilder<Club> builder)
    {
        builder.ToTable("clubs");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        builder.Property(c => c.Acronym).HasColumnName("acronym").HasMaxLength(10).IsRequired();
        builder.Property(c => c.CountryCode).HasColumnName("country_code").HasMaxLength(3).IsRequired();
        builder.Property(c => c.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Address).HasColumnName("address").HasMaxLength(250);
        builder.Property(c => c.Phone).HasColumnName("phone").HasMaxLength(30);
        builder.Property(c => c.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
        builder.Property(c => c.FederationMemberId).HasColumnName("federation_member_id").HasMaxLength(50);
        builder.Property(c => c.LogoUrl).HasColumnName("logo_url").HasMaxLength(500);
        builder.Property(c => c.IsActive).HasColumnName("is_active").HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
        builder.Property(c => c.UpdatedAt).HasColumnName("updated_at");
        builder.Property(c => c.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

        builder.HasIndex(c => c.Name).IsUnique();
    }
}
