using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.Remoting.Lifetime;


namespace DungeonExplorer
{
    public class Room
    {
        /// <summary>
        /// The Room object holds all functionality for each room in this game.
        /// </summary>
        
        private const int LootChance = 4;
        private const int MaxNeighbours = 3;
        private const string DescriptionSrc = "room_descriptions.txt";

        private readonly string Description;
        private readonly List<string> Loot;
        private readonly List<Room> Neighbours;

        private readonly Random rnd = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room()
        {
            this.Description = GenerateDescription();

            // If the room has loot generate it
            if (rnd.Next(LootChance) != 0)
            {
                this.Loot = GenerateLoot();
            }
            // If not set it as null
            else
            {
                this.Loot = new List<string>();
            }

            this.Neighbours = new List<Room>();

            // Generate a random amount of neighbours
            for (int neighbourCount = rnd.Next(MaxNeighbours); neighbourCount > 0 ; neighbourCount--)
            {
                this.GenerateNeighbour();
            }

        }

        /// <summary>
        /// Gets the loot.
        /// </summary>
        /// <returns> List: The list of loot. </returns>
        public List<string> GetLoot() => this.Loot;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns> string: The description. </returns>
        public string GetDescription() => this.Description;

        /// <summary>
        /// Removes a specified item from loot.
        /// </summary>
        public void RemoveLoot(string item)
        {
            // If loot contains the specified item
            if (this.Loot.Contains(item))
            {
                // Then remove it
                this.Loot.Remove(item);
            }
            else
            {
                // If not then display appropriate message
                GameUI.DisplayMessage("You do not have this item.");
            }

        }

        /// <summary>
        /// Generates the description.
        /// </summary>
        /// <returns> string: The new description. </returns>
        private string GenerateDescription()
        {
            try
            {
                if (!File.Exists(DescriptionSrc))
                {
                    return "File does not exist.";
                }

                // Read file contents and choose random description
                using (var reader = new StreamReader(DescriptionSrc))
                {
                    var descriptions = reader.ReadToEnd().Split('\n');
                    return descriptions[rnd.Next(descriptions.Length)];
                }
            }
            catch (Exception ex)
            {
                return $"Failed to read file: {ex}";
            } 
        }

        /// <summary>
        /// Generates the loot.
        /// </summary>
        /// <returns> List: The new list of loot</returns>
        private static List<string> GenerateLoot()
        {
            // Temporary functionality for Assessment 1
            List<string> new_loot = new List<string>();

            new_loot.Add("Rusted Pipe");

            return new_loot;
        }

        /// <summary>
        /// Generates the neighbour.
        /// </summary>
        private void GenerateNeighbour()
        {
            Room new_neighbour = new Room();

            while (this.Neighbours.Contains(new_neighbour))
            {
                new_neighbour = new Room();
            }
            this.Neighbours.Add(new_neighbour);
        }

        /// <summary>
        /// Gets the index of the loot.
        /// </summary>
        /// <param name="loot_count">The loot count.</param>
        /// <param name="current_loot">The current loot.</param>
        /// <returns></returns>
        private int GetLootIndex(int loot_count, List<string> current_loot)
        {
            StringBuilder loot_prompt = new StringBuilder("\nThe room contains:\n");

            // Display the loot as a numbered list
            for (int i = 0; i < loot_count; i++)
            {
                loot_prompt.Append($"\n{i + 1}. {current_loot[i]}");
            }

            loot_prompt.Append($"\n{loot_count + 1}. None");

            // Get the users number choice corresponding to the item they want
            string loot_txt = "\nEnter the item you'd like to loot.";

            loot_prompt.Append(loot_txt);

            List<string> loot_inputs = Enumerable.Range(1, loot_count + 1)
                .Select(x => x.ToString())
                .ToList();

            string input = GameUI.GetInput(loot_inputs, loot_prompt.ToString());

            return int.Parse(input) - 1;
        }

        /// <summary>
        /// Loots the room.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        private void LootRoom(Player current_player)
        {
            List<string> current_loot = this.GetLoot();
            int loot_count = this.GetLoot().Count;

            // If there is loot in the room
            if (current_loot.Any())
            {
                int loot_index = GetLootIndex(loot_count, current_loot);

                // If the choice is not None
                if (loot_index < loot_count)
                {
                    // Add the item to the player's inventory
                    string chosen_item = current_loot[loot_index];
                    current_player.PickUpItem(chosen_item);

                    // Remove the item from the room
                    this.RemoveLoot(chosen_item);
                }
                else
                {
                    GameUI.DisplayMessage("You chose to leave the items.");
                }
            }
            else
            {
                // If not output saying so
                GameUI.DisplayMessage("You could not find any useful items.");
            }
        }

        /// <summary>
        /// Displays the room.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        /// 
        public void DisplayRoom(Player current_player)
        {
            // Outputs strings
            string room_menu = $@"{GameUI.TITLE}{this.GetDescription()}

What will you do next?

1. Loot the room
2. Adventure on
3. Check Self
4. Quit Game
Choose an option [1 - 3]
";

            // Get the player's choice
            List<string> room_options = new List<string>() { "1", "2", "3", "4" };
            string choice = GameUI.GetInput(room_options, room_menu);

            // Compare their choice
            switch (choice)
            {
                case "1": // Loot the room
                    this.LootRoom(current_player);
                    break;

                case "2": // Enter another room
                    break;

                case "3": // Check the player's stats
                    GameUI.DisplayMessage(current_player.ToString());
                    break;

                case "4": // Quit game

                    // If the user wants to quit return to main menu
                    if (GameUI.ConfirmQuit())
                    {
                        GameUI.DisplayMenu();
                    }
                    else
                    {
                        DisplayRoom(current_player);
                    }
                    break;
            }
        }

    }
}