namespace UnderhillLibrary.Api.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnderhillLibrary.Api.Data;
using UnderhillLibrary.Api.Models;
using UnderhillLibrary.Api.DTOs.Auth;
public class AuthService
{
    private readonly UnderhillLibraryDbContext _dbContext;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public AuthService(UnderhillLibraryDbContext dbContext, IPasswordHasher<AppUser> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
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
}