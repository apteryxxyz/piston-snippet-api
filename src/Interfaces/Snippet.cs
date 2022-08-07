using Backend.Models;
namespace Backend.Interfaces;

public class SnippetIncoming
{
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }
}

public class SnippetIncomingPartial
{
    public string? Language { get; set; }
    public string? Version { get; set; }
    public string? Code { get; set; }
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }
}

public class SnippetOutgoing
{
    public string Id { get; set; } = default!;
    public string? Key { get; set; }
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }

    public SnippetOutgoing(Snippet snippet, bool includeKey)
    {
        if (snippet is null) return;

        Id = snippet.Id;
        if (includeKey) Key = snippet.Key;

        Language = snippet.Language;
        Version = snippet.Version;
        Input = snippet.Input;
        Arguments = snippet.Arguments;
        Code = snippet.Code;
    }
}