using System;
using System.IO;

namespace Marky;

/// <summary>
/// Represents the parsed command-line arguments.
/// </summary>
public class CliArguments
{
    public string InputFile { get; init; } = string.Empty;
    public string OutputFile { get; init; } = string.Empty;
    public bool IsValid { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
}

/// <summary>
/// Parses command-line arguments for the Marky application.
/// </summary>
public static class CliParser
{
    /// <summary>
    /// Parses the command-line arguments array.
    /// </summary>
    /// <param name="args">The string array of arguments from Main.</param>
    /// <returns>A populated CliArguments object.</returns>
    public static CliArguments Parse(string[] args)
    {
        if (args.Length == 0)
        {
            return new CliArguments { IsValid = false, ErrorMessage = "No input file specified." };
        }

        string inputFile = args[0];
        string outputFile = string.Empty;

        if (!File.Exists(inputFile))
        {
            return new CliArguments { IsValid = false, ErrorMessage = $"Input file not found: {inputFile}" };
        }

        // Look for an --output flag
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].Equals("--output", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                outputFile = args[i + 1];
                break;
            }
        }

        // If no output file was specified, derive it from the input file.
        if (string.IsNullOrEmpty(outputFile))
        {
            outputFile = Path.ChangeExtension(inputFile, ".html");
        }

        return new CliArguments
        {
            InputFile = inputFile,
            OutputFile = outputFile,
            IsValid = true
        };
    }

    /// <summary>
    /// Prints the usage instructions for the application.
    /// </summary>
    public static void PrintUsage()
    {
        Console.WriteLine("Marky - A simple Markdown to HTML converter.");
        Console.WriteLine("Usage: dotnet run -- <input.md> [--output <output.html>]");
    }
}
