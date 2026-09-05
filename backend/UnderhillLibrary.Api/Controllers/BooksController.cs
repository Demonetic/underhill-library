namespace UnderhillLibrary.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnderhillLibrary.Api.Services;
using UnderhillLibrary.Api.DTOs.Books;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        long userId = GetUserId();

        List<BookResponse> books = await _bookService.GetAllAsync(userId);

        return Ok(books);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBookById(long id)
    {
        long userId = GetUserId();

        var book = await _bookService.GetByIdAsync(userId, id);

        if (book == null)
        {
            return NotFound();
        }
        
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookRequest request)
    {
        long userId = GetUserId();

        var book = await _bookService.CreateAsync(userId, request);
        
        return CreatedAtAction(
            nameof(GetBookById),
            new { id = book.Id },
            book
        );
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateBook(long id, BookRequest request)
    {
        long userId = GetUserId();
        
        var book = await _bookService.UpdateAsync(userId, id, request);

        if (book == null)
        {
            return NotFound();
        }
        
        return Ok(book);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteBook(long id)
    {
        long userId = GetUserId();
        
        bool deleted = await _bookService.DeleteAsync(userId, id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private long GetUserId()
    {
        string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        return long.Parse(userIdClaim!);
    }
}