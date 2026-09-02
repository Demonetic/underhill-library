namespace UnderhillLibrary.Api.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnderhillLibrary.Api.Models;
public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("users");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("id");
        builder.Property(a => a.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(a => a.Username)
            .IsUnique();
        builder.Property(a => a.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
    }
}