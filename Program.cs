using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// <summary>
            /// This game is the Assessment for CMP1903M 2425 A01
            /// 
            /// This game will be know as Rust & Ruin
            /// A game set in the decaying ruins of a steampunk Earth
            /// 
            /// </summary>

            Game game = new Game();

            game.Start();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
