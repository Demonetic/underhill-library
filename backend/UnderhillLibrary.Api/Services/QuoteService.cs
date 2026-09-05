namespace UnderhillLibrary.Api.Services;

using Microsoft.EntityFrameworkCore;
using UnderhillLibrary.Api.Data;
using UnderhillLibrary.Api.DTOs.Quotes;
using UnderhillLibrary.Api.Models;

public class QuoteService
{
    private readonly UnderhillLibraryDbContext _dbContext;

    public QuoteService(UnderhillLibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<QuoteResponse>> GetAllAsync(long userId)
    {
        var quotes = await _dbContext.Quotes
            .Where(quote => quote.UserId == userId)
            .Select(quote => new QuoteResponse
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author,
                CreatedAt = quote.CreatedAt
            })
            .ToListAsync();

        return quotes;
    }

    public async Task<QuoteResponse?> GetByIdAsync(long userId, long quoteId)
    {
        var quote = await _dbContext.Quotes
            .Where(quote => quote.Id == quoteId && quote.UserId == userId)
            .Select(quote => new QuoteResponse
            {
                Id = quote.Id,
                Text = quote.Text,
                Author = quote.Author,
                CreatedAt = quote.CreatedAt
            })
            .FirstOrDefaultAsync();

        return quote;
    }

    public async Task<QuoteResponse> CreateAsync(long userId, QuoteRequest request)
    {
        Quote quote = new Quote
        {
            UserId = userId,
            Text = request.Text,
            Author = request.Author
        };
        
        _dbContext.Quotes.Add(quote);
        await _dbContext.SaveChangesAsync();

        return MapToResponse(quote);
    }

    public async Task<QuoteResponse?> UpdateAsync(long userId, long quoteId, QuoteRequest request)
    {
        var quote = await _dbContext.Quotes
            .Where(quote => quote.UserId == userId && quote.Id == quoteId)
            .FirstOrDefaultAsync();

        if (quote == null)
        {
            return null;
        }

        quote.Text = request.Text;
        quote.Author = request.Author;

        await _dbContext.SaveChangesAsync();
        
        return MapToResponse(quote);
    }

    public async Task<bool> DeleteAsync(long userId, long quoteId)
    {
        var quote = await _dbContext.Quotes
            .Where(quote => quote.Id == quoteId && quote.UserId == userId)
            .FirstOrDefaultAsync();

        if (quote == null)
        {
            return false;
        }
        
        _dbContext.Quotes.Remove(quote);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private static QuoteResponse MapToResponse(Quote quote)
    {
        return new QuoteResponse
        {
            Id = quote.Id,
            Text = quote.Text,
            Author = quote.Author,
            CreatedAt = quote.CreatedAt
        };
    }
}