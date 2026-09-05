namespace UnderhillLibrary.Api.DTOs.Quotes;

public record QuoteResponse
{
    public long Id { get; init; }
    public required string Text { get; init; }
    public string? Author { get; init; }
    public DateTime  CreatedAt { get; init; }
}