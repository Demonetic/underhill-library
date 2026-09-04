namespace UnderhillLibrary.Api.DTOs.Auth;

public record LoginResponse
{
    public required string Token { get; init; }
}