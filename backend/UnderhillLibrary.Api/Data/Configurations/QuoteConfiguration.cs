namespace UnderhillLibrary.Api.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnderhillLibrary.Api.Models;
public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("quotes");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .HasColumnName("id");
        builder.Property(q => q.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        builder.HasOne(q => q.User)
            .WithMany(a => a.Quotes)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(q => q.Text)
            .HasColumnName("text")
            .HasColumnType("text")
            .IsRequired();
        builder.Property(q => q.Author)
            .HasColumnName("author")
            .HasMaxLength(100);
        builder.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}