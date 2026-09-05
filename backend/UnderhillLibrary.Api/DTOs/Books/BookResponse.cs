namespace UnderhillLibrary.Api.DTOs.Books;
public record BookResponse
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required string Author { get; init; }
    public string? Genre { get; init; }
    public DateOnly PublicationDate { get; init; }
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
}