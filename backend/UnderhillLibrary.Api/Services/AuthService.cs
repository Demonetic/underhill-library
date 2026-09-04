namespace UnderhillLibrary.Api.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UnderhillLibrary.Api.Data;
using UnderhillLibrary.Api.Models;
using UnderhillLibrary.Api.DTOs.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
public class AuthService
{
    private readonly UnderhillLibraryDbContext _dbContext;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(UnderhillLibraryDbContext dbContext, IPasswordHasher<AppUser> passwordHasher, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    // Registers a new user and returns whether the registration was successful
    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        // Check if the username is already registered
        if (await _dbContext.Users.AnyAsync(user => user.Username == request.Username))
        {
            return false;
        }
        
        // Create new user with the submitted username
        AppUser newUser = new AppUser
        {
            Username = request.Username,
            PasswordHash = ""
        };

        // Hash the password before saving it to the user
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        // Add five default quotes for the new user
        newUser.Quotes.Add(new Quote
        {
            Text = "Citat 1"
        });
        newUser.Quotes.Add(new Quote
        {
            Text = "Citat 2"
        });
        newUser.Quotes.Add(new Quote
        {
            Text = "Citat 3"
        });
        newUser.Quotes.Add(new Quote
        {
            Text = "Citat 4"
        });
        newUser.Quotes.Add(new Quote
        {
            Text = "Citat 5"
        });
        
        // Save the new user and their default quotes to the db
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    // Authenticates a user and returns a JWT token if the credentials are valid
    public async Task<string?> LoginAsync(LoginRequest request)
    {
        // Find the user by username
        AppUser? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

        // Stop the login attempt if the user does not exist
        if (user == null)
        {
            return null;
        }

        // Compare the submitted password with the stored password hash
        PasswordVerificationResult result =
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        // Stop the login attempt if the password is incorrect
        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return GenerateToken(user);
    }

    // Creates and returns a signed JWT token for an authenticated user
    private string GenerateToken(AppUser user)
    {
        // Read the secret signing key from configuration
        string jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        
        // Read the name of the application that issues the token
        string issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer is not configured.");
        
        // Read the name of the client that the token is intented for
        string audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
        
        // Read the token lifetime from appsettings.json
        int expirationMinutes = _configuration.GetValue<int?>("Jwt:ExpirationMinutes") ?? throw new InvalidOperationException("JWT Expiration Minutes is not configured.");
        
        // Claims contain information about the authenticated user, NameIdentifier stores the user ID and will later be used to retrieve
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        
        // Convert the key back to bytes from Base64
        byte[] keyBytes = Convert.FromBase64String(jwtKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        
        // HMAC SHA-256 allows the API to detect if somebody modifies the token
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        
        // Use UTC so token times behave consistently regardless of which computer or server is running
        DateTime now = DateTime.UtcNow;
        
        // Create the token with its user information, lifetime and signature
        var jwtToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials
        );
        
        // Convert the JwtSecurityToken object into the compact string that can be returned
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}