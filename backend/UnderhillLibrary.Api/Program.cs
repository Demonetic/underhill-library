using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UnderhillLibrary.Api.Data;
using UnderhillLibrary.Api.Models;
using UnderhillLibrary.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                 throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<UnderhillLibraryDbContext>(options => 
    options.UseMySql(connString, ServerVersion.AutoDetect(connString)));
builder.Services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
