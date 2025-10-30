# Marky - A Markdown to HTML Converter

Marky is a .NET console application designed to convert Markdown files (`.md`) into styled HTML documents (`.html`). It supports a rich subset of Markdown features and provides a clean, semantic HTML output.

## Features

*   **Headers:** Supports H1 through H6 Markdown headers.
*   **Text Formatting:** Converts **bold** (`**text**`) and *italic* (`*text*`) text.
*   **Lists:** Handles both unordered (`* item`) and ordered (`1. item`) lists, including **arbitrarily nested lists**.
*   **Blockquotes:** Converts `> blockquote` syntax.
*   **Horizontal Rules:** Supports `---` for horizontal rules.
*   **Inline Code:** Converts `` `code` `` snippets.
*   **Links:** Transforms `[text](url)` into HTML anchor tags.
*   **Images:** Converts `![alt text](image.jpg)` into HTML image tags.
*   **Paragraph Grouping:** Intelligently groups adjacent lines of text into single HTML `<p>` tags.
*   **Full HTML5 Output:** Generates a complete HTML5 document with basic inline styling for readability.
*   **Command-Line Interface:** Easy-to-use CLI for specifying input and output files.

## Project Structure

```
/Marky/
|-- Program.cs              // Main entry point, orchestrates CLI and conversion.
|-- MarkdownConverter.cs    // Core logic for stateful Markdown to HTML conversion.
|-- IParseRule.cs           // Interface for defining parsing rules.
|-- CliParser.cs            // Handles command-line argument parsing.
|-- HtmlBuilder.cs          // Constructs the final HTML5 document.
|-- FileHandler.cs          // Utility for file I/O operations.
|-- /Rules/                 // Directory containing individual Markdown parsing rules.
|-- Marky.csproj

/Marky.Tests/
|-- MarkdownConverterTests.cs // Integration tests for the overall converter.
|-- /RuleTests/             // Directory containing unit tests for individual rules.
|-- Marky.Tests.csproj
```

## Building and Running

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Build

```bash
dotnet build
```

### Run

To convert a Markdown file, use the following command:

```bash
dotnet run --project Marky/Marky.csproj -- <input_file.md> [--output <output_file.html>]
```

**Example:**

```bash
dotnet run --project Marky/Marky.csproj -- ../../docs/sample.md --output output.html
```

### Command-Line Arguments

*   `<input_file.md>` (Required): The path to the Markdown file you want to convert.
*   `--output <output_file.html>` (Optional): Specifies the name of the output HTML file. If omitted, the output file name will be derived from the input file (e.g., `input.md` becomes `input.html`).

## Testing

To run the unit and integration tests, execute:

```bash
dotnet test Marky.Tests/Marky.Tests.csproj
```
