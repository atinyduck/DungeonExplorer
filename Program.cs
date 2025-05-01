global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Text;
global using System.Threading.Tasks;
global using System.Threading;
global using System.IO;
global using System.Diagnostics;
global using System.Text.Json;

namespace DungeonExplorer;
public static class Program
{
    /// <summary>
    /// Run point of the program
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        // This game is the Assessment for CMP1903M 2425 A01 and A02

        Game game = new Game();

        game.Start();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}