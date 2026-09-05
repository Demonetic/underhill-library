namespace UnderhillLibrary.Api.Services;

using Microsoft.EntityFrameworkCore;
using UnderhillLibrary.Api.Data;
using UnderhillLibrary.Api.DTOs.Books;
using UnderhillLibrary.Api.Models;

public class BookService
{
    private readonly UnderhillLibraryDbContext _dbContext;
    
    public BookService(UnderhillLibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookResponse>> GetAllAsync(long userId)
    {
        var books = await _dbContext.Books
            .Where(book => book.UserId == userId)
            .Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                PublicationDate = book.PublicationDate,
                Description = book.Description,
                CreatedAt = book.CreatedAt
            })
            .ToListAsync();

        return books;
    }

    public async Task<BookResponse?> GetByIdAsync(long userId, long bookId)
    {
        var book = await _dbContext.Books
            .Where(book => book.Id == bookId && book.UserId == userId)
            .Select(book => new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Genre = book.Genre,
                PublicationDate = book.PublicationDate,
                Description = book.Description,
                CreatedAt = book.CreatedAt
            })
            .FirstOrDefaultAsync();

        return book;
    }

    public async Task<BookResponse> CreateAsync(long userId, BookRequest request)
    {
        Book book = new Book
        {
            UserId = userId,
            Title = request.Title,
            Author = request.Author,
            Genre = request.Genre,
            PublicationDate = request.PublicationDate,
            Description = request.Description
        };

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
        
        return MapToResponse(book);
    }

    public async Task<BookResponse?> UpdateAsync(long userId, long bookId, BookRequest request)
    {
        var book = await _dbContext.Books
            .Where(book => book.UserId == userId && book.Id == bookId)
            .FirstOrDefaultAsync();

        if (book == null)
        {
            return null;
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.Genre = request.Genre;
        book.PublicationDate = request.PublicationDate;
        book.Description = request.Description;

        await _dbContext.SaveChangesAsync();
        
        return MapToResponse(book);
    }

    public async Task<bool> DeleteAsync(long userId, long bookId)
    {
        var book = await _dbContext.Books
            .Where(book => book.Id == bookId && book.UserId == userId)
            .FirstOrDefaultAsync();

        if (book == null)
        {
            return false;
        }

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    private static BookResponse MapToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublicationDate = book.PublicationDate,
            Description = book.Description,
            CreatedAt = book.CreatedAt
        };
    }
}