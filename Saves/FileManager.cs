using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer;
public static class FileManager
{
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