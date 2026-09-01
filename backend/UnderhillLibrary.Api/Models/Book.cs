namespace UnderhillLibrary.Api.Models;

public class Book
{
    public long Id  { get; set; }
    public long UserId { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Genre { get; set; }
    public DateOnly PublicationDate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public AppUser User { get; set; } = null!;
}