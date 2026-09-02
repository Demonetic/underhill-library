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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UnderhillLibraryDbContext).Assembly);
    }
}