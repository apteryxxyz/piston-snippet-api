namespace Backend.Interfaces;

public class RuntimeResponse
{
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Runtime { get; set; } = default!;
    public string[] Aliases { get; set; } = default!;
}