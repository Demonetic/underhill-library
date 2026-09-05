namespace UnderhillLibrary.Api.DTOs.Books;

using System.ComponentModel.DataAnnotations;

public record BookRequest
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(255)]
    public required string Title { get; init; }
    [Required(ErrorMessage = "Author is required")]
    [StringLength(150)]
    public required string Author { get; init; }
    [StringLength(100)]
    public string? Genre { get; init; }
    [Required(ErrorMessage = "Publication date is required")]
    public required DateOnly PublicationDate { get; init; }
    public string? Description { get; init; }
}