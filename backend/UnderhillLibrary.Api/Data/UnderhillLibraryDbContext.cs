namespace UnderhillLibrary.Api.Data;

using Microsoft.EntityFrameworkCore;
using UnderhillLibrary.Api.Models;
public class UnderhillLibraryDbContext : DbContext
{
    public DbSet<Book> Books
    {
        get { return Set<Book>(); }
    }
    public DbSet<AppUser> Users
    {
        get { return Set<AppUser>(); }
    }
    public DbSet<Quote> Quotes
    {
        get { return Set<Quote>(); }
    }
    public UnderhillLibraryDbContext(DbContextOptions<UnderhillLibraryDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var appUser = modelBuilder.Entity<AppUser>();
        var book = modelBuilder.Entity<Book>();
        var quote = modelBuilder.Entity<Quote>();

        appUser.ToTable("users");
        appUser.HasKey(a => a.Id);
        appUser.Property(a => a.Id)
            .HasColumnName("id");
        appUser.Property(a => a.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);
        appUser.HasIndex(a => a.Username)
            .IsUnique();
        appUser.Property(a => a.PasswordHash)
            .HasColumnName("password_hash")
            .IsRequired()
            .HasMaxLength(255);
        appUser.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        book.ToTable("books");
        book.HasKey(b => b.Id);
        book.Property(b => b.Id)
            .HasColumnName("id");
        book.Property(b => b.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        book.HasOne(b => b.User)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        book.Property(b => b.Title)
            .HasColumnName("title")
            .IsRequired()
            .HasMaxLength(255);
        book.Property(b => b.Author)
            .HasColumnName("author")
            .IsRequired()
            .HasMaxLength(150);
        book.Property(b => b.Genre)
            .HasColumnName("genre")
            .HasMaxLength(100);
        book.Property(b => b.PublicationDate)
            .HasColumnName("publication_date")
            .IsRequired()
            .HasColumnType("date");
        book.Property(b => b.Description)
            .HasColumnName("description")
            .HasColumnType("text");
        book.Property(b => b.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        quote.ToTable("quotes");
        quote.HasKey(q => q.Id);
        quote.Property(q => q.Id)
            .HasColumnName("id");
        quote.Property(q => q.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        quote.HasOne(q => q.User)
            .WithMany(a => a.Quotes)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        quote.Property(q => q.Text)
            .HasColumnName("text")
            .HasColumnType("text")
            .IsRequired();
        quote.Property(q => q.Author)
            .HasColumnName("author")
            .HasMaxLength(100);
        quote.Property(q => q.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}