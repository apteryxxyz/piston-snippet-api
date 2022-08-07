namespace Backend.Models;

public class Snippet
{
    public string Id { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }
    public string Code { get; set; } = default!;
}