﻿using System;
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

        private const string MAIN = @"In a forgotten realm consumed by corrosion and decay, each 
door leads deeper into the ruins of a kingdom lost to time. 
Monsters lurk in shadowed corridors, and only those brave 
enough can uncover the secrets hidden within.

Will you press on through the rust and ruin, or will you 
succumb to the darkness?

1. Enter the Ruins
2. How to Play
3. Abandon Quest
Q. Quit
Choose your fate [1 - 4]";

        private const string ROOM = @"What will you do next?

1. Loot the room
2. Adventure on
3. Go Back
4. Check Self
Q. Quit Game
Choose an option [1 - 3]";

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
                List<string> menu_options = new List<string>() { "1", "2", "3", "Q", "D"};
                string menu_input = GetInput(menu_options, TITLE + MAIN);

                switch (menu_input.ToUpper())
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

                    case "Q":
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
        /// Displays the room.
        /// </summary>
        /// <param name="player">The current player.</param>
        /// <param name="room">The current room</param>
        internal static void DisplayRoom(Room room, Player player)
        {
            bool in_room = true;
            while (in_room)
            {
                // Outputs strings
                string room_menu = TITLE + room.ToString() + ROOM;

                // Get the player's choice
                List<string> room_options = new List<string>() { "1", "2", "3", "4", "Q" };
                string choice = GetInput(room_options, room_menu);

                // Compare their choice
                switch (choice.ToUpper())
                {
                    case "1": // Loot the room
                        room.LootRoom(player);
                        break;

                    case "2": // Enter another room
                        // Ensure there is a room to move to.
                        if (room.GetNeighbours().Count > 0)
                        {
                            room.AdventureOn(player);
                        }
                        else
                        {
                            DisplayMessage("You cannot push forward, you must go back.");
                        }
                        break;

                    case "3":
                        in_room = false;
                        break;

                    case "4": // Check the player's stats
                        GameUI.DisplayMessage(player.ToString());
                        break;

                    case "Q": // Quit game

                        // If the user wants to quit return to main menu
                        if (ConfirmQuit())
                        {
                            DisplayMenu();
                        }
                        else
                        {
                            DisplayRoom(room, player);
                        }
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

        /// <summary>
        /// Confirm if the user wants to quit.
        /// </summary>
        /// <returns>bool: If the input is 'Y'</returns>
        internal static bool ConfirmQuit()
        {
            string quit_txt = "Are you sure you want to quit? [ Y/N ]\nUnsaved progress will be deleted.";

            List<string> valid_inputs = new List<string>() { "Y", "N" };
            string quit_input = GetInput(valid_inputs, quit_txt);

            return quit_input.ToUpper() == "Y";
        }

        /// <summary>
        /// Waits for input from the user.
        /// </summary>
        internal static void WaitForInput()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}
