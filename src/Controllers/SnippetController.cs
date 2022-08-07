using Microsoft.AspNetCore.Mvc;
using Backend.Contexts;
using Backend.Models;
using Backend.Interfaces;
namespace Backend.Controllers;

/// <summary>
/// Handles the snippets API endpoints.
/// </summary>
[ApiController]
[Route("/api/snippets")]
public class SnippetController : Controller
{
    /** Database context */
    private readonly SnippetContext _context;
    private readonly ILogger<SnippetController> _logger;

    public SnippetController(SnippetContext context, ILogger<SnippetController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Create a brand new code snippet.
    /// </summary>
    /// <param name="data">The snippet data.</param>
    /// <returns>The created snippet object.</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SnippetOutgoing>> PutSnippet(SnippetIncoming data)
    {
        // Generate random strings, used for ID and API key
        var id = Guid.NewGuid().ToString("N");
        var key = Guid.NewGuid().ToString("N");

        // Create an instance of the Snippet class
        var snippet = new Snippet
        {
            Id = id,
            Key = key,
            Language = data.Language,
            Version = data.Version,
            Input = data.Input,
            Arguments = data.Arguments,
            Code = data.Code
        };

        // Add the snippet to the database and save
        _context.Snippets.Add(snippet);
        await _context.SaveChangesAsync();
        _logger.LogInformation("CREATED snippet with ID {id}", id);

        return CreatedAtAction(
            nameof(GetSnippet),
            new { id },
            snippet
        );
    }

    /// <summary>
    /// Get a code snippet by its ID.
    /// </summary>
    /// <param name="id">Snippet ID.</param>
    /// <param name="key">API key.</param>
    /// <returns>Snippet object.</returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<ActionResult<SnippetOutgoing>> GetSnippet(string id, [FromHeader(Name = "key")] string key)
    {
        // Attempt to find the snippet
        var snippet = await _context.Snippets.FindAsync(id);
        if (snippet is null) return NotFound();

        // Ensure the passed API key matches the snippets
        if (snippet.Key != key) return Unauthorized();

        // Response with the snippet
        return new SnippetOutgoing(snippet, false);
    }

    /// <summary>
    /// Update the data of a code snippet.
    /// </summary>
    /// <param name="id">Snippet ID.</param>
    /// <param name="key">API Key.</param>
    /// <param name="data">New snippet data.</param>
    /// <returns></returns>
    [HttpPatch]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SnippetOutgoing>> PatchSnippet(string id,
        [FromHeader(Name = "key")] string key,
        [FromBody] SnippetIncomingPartial data)
    {
        // Attempt to find the snippet
        var snippet = await _context.Snippets.FindAsync(id);
        if (snippet is null) return NotFound();

        // Ensure the passed API key matches the snippets
        if (snippet.Key != key) return Unauthorized();

        // Update the snippets properties
        if (data.Language != null) snippet.Language = data.Language;
        if (data.Version != null) snippet.Version = data.Version;
        if (data.Input != null) snippet.Input = data.Input;
        if (data.Arguments != null) snippet.Arguments = data.Arguments;
        if (data.Code != null) snippet.Code = data.Code;

        // Save changes
        await _context.SaveChangesAsync();
        _logger.LogInformation("UPDATED snippet with ID {id}", id);

        // Response with the snippet
        return Ok(new SnippetOutgoing(snippet, false));
    }

    /// <summary>
    /// Delete a code snippet.
    /// </summary>
    /// <param name="id">Snippet ID.</param>
    /// <param name="key">API Key.</param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SnippetOutgoing>> DeleteSnippet(string id,
        [FromHeader(Name = "key")] string key)
    {
        // Attempt to find the snippet
        var snippet = await _context.Snippets.FindAsync(id);
        if (snippet is null) return NotFound();

        // Ensure the passed API key matches the snippets
        if (snippet.Key != key) return Unauthorized();

        // Remove snippet from database
        _context.Remove(snippet);

        // Save changes
        await _context.SaveChangesAsync();
        _logger.LogInformation("DELETED snippet with ID {id}", id);

        // Response with the snippet
        return Ok(new SnippetOutgoing(snippet, false));
    }
}
