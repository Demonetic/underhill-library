namespace UnderhillLibrary.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UnderhillLibrary.Api.Services;
using UnderhillLibrary.Api.DTOs.Quotes;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuotesController : ControllerBase
{
    private readonly QuoteService _quoteService;

    public QuotesController(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetQuotes()
    {
        long userId = GetUserId();

        List<QuoteResponse> quotes = await _quoteService.GetAllAsync(userId);
        
        return Ok(quotes);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetQuoteById(long id)
    {
        long userId = GetUserId();
        
        var quote = await _quoteService.GetByIdAsync(userId, id);

        if (quote == null)
        {
            return NotFound();
        }
        
        return Ok(quote);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuote(QuoteRequest request)
    {
        long userId = GetUserId();

        var quote = await _quoteService.CreateAsync(userId, request);
        
        return CreatedAtAction(
            nameof(GetQuoteById),
            new { id = quote.Id },
            quote
        );
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateQuote(long id, QuoteRequest request)
    {
        long userId = GetUserId();

        var quote = await _quoteService.UpdateAsync(userId, id, request);

        if (quote == null)
        {
            return NotFound();
        }
        
        return Ok(quote);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteQuote(long id)
    {
        long userId = GetUserId();

        bool deleted = await _quoteService.DeleteAsync(userId, id);

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