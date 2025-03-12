using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal static class GameUI
    {
        #region String Constants

        internal const string TITLE = @"
=================================================
                  RUST & RUIN                  
=================================================

";

        private const string INTRO = @"The world you know is fading, devoured by rust, decay, and time.  
Once-thriving civilisations now lie buried beneath dunes of  
corroded steel and forgotten ruins. Towering constructs from  
a bygone age stand silent, their gears jammed, their  
purposes lost to memory.

In this land of ruin, survival is both a battle and a burden.  

You are a survivor, a wanderer of the Rustlands. Each step  
brings you closer to the heart of the decay—a sprawling  
labyrinth of ancient chambers and forgotten vaults known  
as the Iron Maw.

Before we begin we must know your name adventurer.
What will we call you on this adventure...
";

        private const string WELCOME = @"The sound of groaning metal fills the air as you step into the ruins. 
Rust clings to every surface, and decay has claimed all that was once 
strong and whole. Yet, in this desolation, a faint glimmer of hope 
remains a legend of power hidden deep within the labyrinth. 

You are an adventurer, seeking answers, treasure, or perhaps redemption. 
Before you lies a series of rooms, each more treacherous than the last. 

Do you have what it takes to navigate the dangers, claim the treasure, 
and uncover the secrets of the Rust & Ruin?";

        private const string MENU = @"In a forgotten realm consumed by corrosion and decay, each 
door leads deeper into the ruins of a kingdom lost to time. 
Monsters lurk in shadowed corridors, and only those brave 
enough can uncover the secrets hidden within.

Will you press on through the rust and ruin, or will you 
succumb to the darkness?

1. Enter the Ruins
2. How to Play
3. Abandon Quest
4. Quit
Choose your fate [1 - 4]";

        #endregion

        #region Output Control

        /// <summary>
        /// Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal static void DisplayMessage(string message, bool clear = false, bool wait = true)
        {
            if (clear) Console.Clear();

            Console.WriteLine(message);
            
            if (wait) WaitForInput();
        }

        /// <summary>
        /// Displays the intro.
        /// </summary>
        /// <param name="player">The player.</param>
        internal static void DisplayIntro(Player player)
        {
            // Get a new name input from the user
            player.Name = GetInput(new List<string>(), (TITLE + INTRO), false);

            string intro_txt = TITLE + $"Welcome {player.Name} to the Iron Maw!\n\r" + WELCOME;

            DisplayMessage(intro_txt, true);
        }

        /// <summary>
        /// Displays the menu.
        /// </summary>
        internal static void DisplayMenu()
        {
            bool playing = true;
            while (playing)
            {
                List<string> menu_options = new List<string>() { "1", "2", "3", "4", "D"};
                string menu_input = GetInput(menu_options, TITLE + MENU);

                switch (menu_input)
                {
                    case "1":
                        // Start the game
                        return;

                    case "2":
                        // Display how to
                        DisplayHowTo();
                        continue;

                    case "3":
                        // Delete the save game
                        DisplayMessage(TITLE + "Waiting on implementation", true);
                        continue;

                    case "4":
                        // Quit the game
                        if (ConfirmQuit())
                        {
                            Environment.Exit(0);
                        }
                        break;

                    case "D":
                        // Debugging 
                        Testing.TestMenu();
                        break;

                    default:
                        // Designed to catch anomalies
                        DisplayMessage(TITLE + "Error processing menu input, quitting.", true);
                        Environment.Exit(1);
                        break;
                }
            }
        }

        /// <summary>
        /// Displays how to play the game.
        /// </summary>
        internal static void DisplayHowTo()
        {
            DisplayMessage(TITLE + "Waiting on implementation", true);
        }

        #endregion

        #region Input Control

        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <param name="valid_inputs">The valid inputs.</param>
        /// <param name="output">The output.</param>
        /// <returns>string: The user's input.</returns>
        internal static string GetInput(List<string> valid_inputs, string output, bool enforce_validation = true)
        {
            string input = null;

            while (true)
            {
                // Output message and get user input
                DisplayMessage(output, clear: true, wait: false);
                Console.Write("\n :: ");
                input = Console.ReadLine()?.Trim();

                if (!enforce_validation)
                {
                    return input;
                }

                if (!valid_inputs.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    DisplayMessage($"{input} is not a valid option.");
                }
                else
                {
                    return input;
                }
            }
        }

        internal static bool ConfirmQuit()
        {
            string quit_txt = "Are you sure you want to quit? [ Y/N ]";

            List<string> valid_inputs = new List<string>() { "Y", "N" };
            string quit_input = GetInput(valid_inputs, quit_txt);

            return quit_input.ToUpper() == "Y";
        }

        internal static void WaitForInput()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}
