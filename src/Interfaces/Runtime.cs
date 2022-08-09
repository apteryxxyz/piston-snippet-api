namespace Backend.Interfaces;

/// <summary>
/// Class to represent the response from
/// the piston API runtimes endpoint.
/// </summary>
public class RuntimeResponse
{
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Runtime { get; set; } = default!;
    public string[] Aliases { get; set; } = default!;
}