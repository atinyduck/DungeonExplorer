using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Program
    {
        /// <summary>
        /// Run point of the program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // This game is the Assessment for CMP1903M 2425 A01

            Game game = new Game();

            game.Start();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
