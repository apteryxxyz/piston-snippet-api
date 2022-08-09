using Backend.Models;
namespace Backend.Interfaces;

/// <summary>
/// Class to represent incoming snippet data.
/// </summary>
public class SnippetIncoming
{
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }
}

/// <summary>
/// SnippetIncoming class but all properties are 
/// </summary>
public class SnippetIncomingPartial
{
    public string? Language { get; set; }
    public string? Version { get; set; }
    public string? Code { get; set; }
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }
}

/// <summary>
/// Class representing the outgoing snippet data.
/// </summary>
public class SnippetOutgoing
{
    public string Id { get; set; } = default!;
    public string? Key { get; set; }
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string? Input { get; set; }
    public string[]? Arguments { get; set; }

    /// <summary>
    /// This constructor takes a snippet model instance and
    /// will convert it into an outgoing object.
    /// </summary>
    /// <param name="snippet">Snippet instance.</param>
    /// <param name="includeKey">Whether to include the API key.</param>
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