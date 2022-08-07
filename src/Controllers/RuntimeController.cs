using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
namespace Backend.Controllers;

[ApiController]
[Route("/api/runtimes")]
public class RuntimeController : Controller
{
    private readonly HttpClient _client;
    private readonly ILogger<RuntimeController> _logger;

    public RuntimeController(IHttpClientFactory clientFactory, ILogger<RuntimeController> logger)
    {
        if (clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
        _client = clientFactory.CreateClient("piston");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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