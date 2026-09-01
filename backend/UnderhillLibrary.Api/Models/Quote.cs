namespace UnderhillLibrary.Api.Models;

public class Quote
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Text  { get; set; }
    public string? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public AppUser User { get; set; } = null!;
}