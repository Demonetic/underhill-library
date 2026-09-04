namespace UnderhillLibrary.Api.DTOs.Auth;

using System.ComponentModel.DataAnnotations;

public record LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3)]
    public required string Username { get; init; }
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8)]
    public required string Password { get; init; }
}