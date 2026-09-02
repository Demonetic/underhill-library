namespace UnderhillLibrary.Api.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UnderhillLibrary.Api.Models;
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .HasColumnName("id");
        builder.Property(b => b.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        builder.HasOne(b => b.User)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(b => b.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(b => b.Author)
            .HasColumnName("author")
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(b => b.Genre)
            .HasColumnName("genre")
            .HasMaxLength(100);
        builder.Property(b => b.PublicationDate)
            .HasColumnName("publication_date")
            .IsRequired()
            .HasColumnType("date");
        builder.Property(b => b.Description)
            .HasColumnName("description")
            .HasColumnType("text");
        builder.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}