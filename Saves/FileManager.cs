namespace DungeonExplorer;
/// <summary>
/// Class reposnsible for file management.
/// </summary>
public static class FileManager
{
    /// <summary>
    /// Reads from the file asynchronous.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <returns></returns>
    public static async Task<string> ReadFileAsync(string filePath)
    {
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
        catch (Exception ex)
        {
            UI.Message($"Error reading file: {ex.Message}", clear: true, wait: true);
            return string.Empty;
        }
    }

    /// <summary>
    /// Writes to the file asynchronous.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="content">The content.</param>
    public static async Task WriteFileAsync(string filePath, string content)
    {
        try
        {
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(content);
            }
        }
        catch (Exception ex)
        {
            UI.Message($"Error writing to file: {ex.Message}", clear: true, wait: true);
        }
    }
}