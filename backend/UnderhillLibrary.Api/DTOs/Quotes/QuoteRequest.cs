namespace UnderhillLibrary.Api.DTOs.Quotes;

using System.ComponentModel.DataAnnotations;

public record QuoteRequest
{
    [Required(ErrorMessage = "Text is required.")]
    public required string Text { get; init; }
    [StringLength(100)]
    public string? Author { get; init; }
}