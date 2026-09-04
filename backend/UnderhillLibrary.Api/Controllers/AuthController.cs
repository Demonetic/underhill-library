namespace UnderhillLibrary.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using UnderhillLibrary.Api.DTOs.Auth;
using UnderhillLibrary.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
       bool result = await _authService.RegisterAsync(request);

       if (!result)
       {
           return Conflict();
       }

       return StatusCode(201);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        string? token = await _authService.LoginAsync(request);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new LoginResponse
        {
            Token = token
        });
    }
}