using System.IO;

namespace Marky;

/// <summary>
/// A helper class for reading from and writing to files.
/// </summary>
public static class FileHandler
{
    /// <summary>
    /// Reads all lines from a specified file.
    /// </summary>
    /// <param name="path">The absolute or relative path to the file.</param>
    /// <returns>An array of strings, where each element is a line from the file.</returns>
    public static string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    /// <summary>
    /// Writes a string to a specified file, overwriting it if it exists.
    /// </summary>
    /// <param name="path">The absolute or relative path to the file.</param>
    /// <param name="content">The string content to write to the file.</param>
    public static void WriteAllText(string path, string content)
    {
        File.WriteAllText(path, content);
    }
}
