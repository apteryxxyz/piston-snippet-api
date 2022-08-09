using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Backend.Contexts;
using Backend.Interfaces;
namespace Backend.Controllers;

/// <summary>
/// Handles the snippets execute API endpoints.
/// </summary>
[ApiController]
[Route("/api/snippets/{id}/execute")]
public class ExecuteController : Controller
{
    private readonly IConfiguration _config;
    private readonly HttpClient _client;
    private readonly SnippetContext _context;
    private readonly ILogger<ExecuteController> _logger;

    public ExecuteController(IConfiguration config, IHttpClientFactory clientFactory, SnippetContext context, ILogger<ExecuteController> logger)
    {
        // Initialise the execute controller with dependencies injected
        _config = config ?? throw new ArgumentNullException(nameof(config));
        if (clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
        _client = clientFactory.CreateClient("piston");
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Execute the code within a snippet.
    /// </summary>
    /// <param name="id">Snippet ID.</param>
    /// <param name="key">API key.</param>
    /// <returns>Execute result.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExecuteOutgoing>> ExecuteSnippet(string id,
        [FromHeader(Name = "key")] string key)
    {
        // Attempt to find the snippet
        var snippet = await _context.Snippets.FindAsync(id);
        if (snippet is null) return NotFound();

        // Ensure the passed API key matches the snippets
        if (snippet.Key != key) return Unauthorized();

        // Format request content
        var execute = new ExecuteRequest(snippet);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var requestBody = JsonSerializer.Serialize(execute, jsonOptions);
        var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        // Make request to Piston API
        var response = await _client.PostAsync("execute", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<ExecuteResponse>(responseBody);

        _logger.LogInformation($"EXECUTED snippet with ID {id}");
        if (_config["Env"] == "Dev") _logger.LogInformation($"ID: {id} | Key {key}");

        if (json == null) return BadRequest();
        else if (json.message != null) return BadRequest();
        else return Ok(new ExecuteOutgoing(json));
    }
}
