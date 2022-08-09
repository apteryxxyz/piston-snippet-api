using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
namespace Backend.Controllers;

/// <summary>
/// Handles the runtimes API endpoints.
/// </summary>
[ApiController]
[Route("/api/runtimes")]
public class RuntimeController : Controller
{

    private readonly HttpClient _client;

    public RuntimeController(IHttpClientFactory clientFactory)
    {
        // Initialise the runtime controller with dependencies injected
        if (clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
        _client = clientFactory.CreateClient("piston");
    }

    /// <summary>
    /// Get a list of runtimes/languages that can be used.
    /// </summary>
    /// <returns>Runtimes list.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RuntimeResponse>>> GetRuntimes()
    {
        var res = await _client.GetAsync("runtimes");
        var content = await res.Content.ReadAsStringAsync();
        return Ok(content);
    }
}