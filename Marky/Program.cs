using System;
using System.IO;
using Marky;

// --- Main Execution ---
var arguments = CliParser.Parse(args);

if (!arguments.IsValid)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {arguments.ErrorMessage}");
    Console.ResetColor();
    CliParser.PrintUsage();
    return 1;
}

try
{
    Console.WriteLine($"Converting '{arguments.InputFile}'...");

    var markdownContent = string.Join("\n", FileHandler.ReadAllLines(arguments.InputFile));
    
    var converter = new MarkdownConverter();
    var htmlBody = converter.Convert(markdownContent);

    var htmlBuilder = new HtmlBuilder();
    htmlBuilder.SetTitle(Path.GetFileNameWithoutExtension(arguments.InputFile));
    htmlBuilder.AppendBody(htmlBody);

    FileHandler.WriteAllText(arguments.OutputFile, htmlBuilder.ToString());

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Successfully converted to '{arguments.OutputFile}'");
    Console.ResetColor();
    return 0;
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
    Console.ResetColor();
    return 1;
}