using Backend.Models;
namespace Backend.Interfaces;

/// <summary>
/// Class to represent an request body object
/// to the piston API execute endpoint.
/// </summary>
public class ExecuteRequest
{
    public string language { get; set; } = default!;
    public string version { get; set; } = default!;
    public ExecuteFile[] files { get; set; } = default!;
    public string? stdin { get; set; } = default!;
    public string[]? args { get; set; } = default!;
    
    public ExecuteRequest(Snippet snippet)
    {
        if (snippet is null) return;

        language = snippet.Language;
        version = snippet.Version;
        files = new ExecuteFile[] { new ExecuteFile { content = snippet.Code } };
        stdin = snippet.Input;
        args = snippet.Arguments;
    }
}

public class ExecuteFile
{
    public string name { get; set; } = default!;
    public string content { get; set; } = default!;
}

/// <summary>
/// Class representing a response from the
/// piston API execute endpoint.
/// </summary>
public class ExecuteResponse
{
    public string? message { get; set; }
    public string language { get; set; } = default!;
    public string version { get; set; } = default!;
    public ExecuteRun run { get; set; } = default!;
}

public class ExecuteRun
{
    public string srdout { get; set; } = default!;
    public string stderr { get; set; } = default!;
    public int code { get; set; } = default!;
    public string? signal { get; set; }
    public string output { get; set; } = default!;
}

/// <summary>
/// A class representing the outgoing execute
/// result object.
/// </summary>
public class ExecuteOutgoing
{
    public string Language { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Output { get; set; } = default!;

    public ExecuteOutgoing(ExecuteResponse response)
    {
        Language = response.language;
        Version = response.version;
        Output = response.run.output;
    }
}