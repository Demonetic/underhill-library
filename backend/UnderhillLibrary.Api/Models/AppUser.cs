namespace UnderhillLibrary.Api.Models;

public class AppUser
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Book> Books { get; set; } = [];
    public ICollection<Quote> Quotes { get; set; } = [];
}