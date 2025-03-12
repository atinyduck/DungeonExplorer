using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.Remoting.Lifetime;


namespace DungeonExplorer
{
    internal class Room
    {
        /// <summary>
        /// The Room object holds all functionality for each room in this game.
        /// </summary>
        
        // Constants
        private const int LootChance = 4;
        private const int DeadEndChance = 12;
        internal const int MaxNeighbours = 3;
        private const int MaxRecursionDepth = 5;
        private const string DescriptionSrc = "room_descriptions.txt";

        // Descriptions
        private static readonly List<string> Descriptions = LoadDescriptions();
        private readonly string Description;

        private readonly List<string> Loot;

        // Neighbours and their direction
        private readonly Dictionary<Room, string> Neighbours = new Dictionary<Room, string>();

        private readonly Random rnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        internal Room(int depth = 0, Random random = null)
        {

            this.rnd = random ?? new Random();

            if (Descriptions.Count != 0)
            {
                this.Description = GenerateDescription();
            }

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

            if (this.rnd.Next(DeadEndChance) != 1)
            {
                // Generate a random amount of neighbours
                for (int neighbourCount = this.rnd.Next(1, MaxNeighbours + 1); neighbourCount > 0; neighbourCount--)
                {
                    this.GenerateNeighbour(depth + 1);
                }
            }
            
        }

        /// <summary>
        /// Returns the description and contents of the room
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder room_string = new StringBuilder();

            // Display the description
            room_string.AppendLine($"What you see...\n\n    {GetDescription()}");

            // Display the loot 
            if (GetLoot().Any())
            {
                room_string.AppendLine("\nYou can see things worth picking up!");
                foreach (var item in GetLoot())
                {
                    room_string.AppendLine($" :: {item}");
                }
            }
            else
            {
                room_string.AppendLine("\nThere is nothing of worth in this room.");
            }

            // Display the neighbours
            if (GetNeighbours().Any())
            {
                room_string.AppendLine($"\nYou can see {GetNeighbours().Count} door(s) leading from this room.");
            }
            else
            {
                room_string.AppendLine("\nThere are no doors leading out, go back.");
            }

            // Return the display 
            return room_string.ToString();
        }

        /// <summary>
        /// Gets the list of neighbours
        /// </summary>
        /// <returns> List: The neighbours </returns>
        internal Dictionary<Room, string> GetNeighbours() => this.Neighbours;

        /// <summary>
        /// Gets the loot.
        /// </summary>
        /// <returns> List: The list of loot. </returns>
        internal List<string> GetLoot() => this.Loot;

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns> string: The description. </returns>
        internal string GetDescription() => this.Description;

        /// <summary>
        /// Removes a specified item from loot.
        /// </summary>
        internal void RemoveLoot(string item)
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
        /// Loads the descriptions.
        /// </summary>
        /// <returns> List: The descriptions. </returns>
        private static List<string> LoadDescriptions()
        {
            List<string> catch_list = new List<string> {
                        "A dark and eerie room.",
                        "A room filled with ancient artifacts.",
                        "A room with a mysterious aura."
                        };

            try
            {
                if (!File.Exists(DescriptionSrc))
                {
                    return catch_list;
                }

                // Read file contents and choose random description
                using (var reader = new StreamReader(DescriptionSrc))
                {
                    var descriptions = reader.ReadToEnd().Split('\n').ToList();

                    // Ensure there's atleast one description
                    if (descriptions.Any())
                    {
                        return descriptions;
                    }
                    else
                    {
                        return catch_list;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error for feeback
                GameUI.DisplayMessage($"Failed to read descriptions: {ex}");
                return catch_list;
            }
        }

        /// <summary>
        /// Generates a random description
        /// </summary>
        /// <returns> String: The new description</returns>
        private string GenerateDescription()
        {
            if (Descriptions.Count == 0)
            {
                return "A mysterious room with no description, you may have gone blind?";
            }
            return Descriptions[rnd.Next(Descriptions.Count)];
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
        internal void GenerateNeighbour(int depth = 0)
        {
            if (this.rnd.Next(DeadEndChance) != 1)
            {
                try
                {
                    // If the max amount of neighbours is present do not make more
                    if (GetNeighbours().Count >= MaxNeighbours)
                    {
                        return;
                    }

                    // Check if max recursion depth has been reached
                    if (depth > MaxRecursionDepth)
                    {
                        return;
                    }

                    // Make a new room
                    Room new_neighbour = new Room(depth);
                    string direction = GetRandomDirection();

                    // Ensure direction is unique
                    while (GetNeighbours().ContainsValue(direction))
                    {
                        direction = GetRandomDirection();
                    }

                    this.Neighbours.Add(new_neighbour, direction);
                }
                catch (Exception ex)
                {
                    string message = $"Exception in GenerateNeighbour: {ex}";
                    GameUI.DisplayMessage(message, wait: false);
                    throw; // Re-throw for debugging
                }
            }
        }

        /// <summary>
        /// Gets a random direction (N, E, W)
        /// South represents the previous room
        /// </summary>
        /// <returns>string: The direction</returns>
        private string GetRandomDirection()
        {
            string[] directions = { "North", "East", "West" };
            return directions[rnd.Next(directions.Length)];
        }

        internal Room MoveToNeighbour(string direction)
        {
            foreach (var neighbour in this.Neighbours)
            {
                // Find a new room equal to the players entered direction
                if (neighbour.Value.Equals(direction, StringComparison.OrdinalIgnoreCase))
                {
                    return neighbour.Key;
                }
            }

            GameUI.DisplayMessage("You cannot go that way.");
            return this; // Stay in the current room
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

            // Get the loot input
            List<string> loot_inputs = Enumerable.Range(1, loot_count + 1)
                .Select(x => x.ToString())
                .ToList();
            string input = GameUI.GetInput(loot_inputs, loot_prompt.ToString());

            // The index of the loot
            return int.Parse(input) - 1;
        }

        /// <summary>
        /// Loots the room.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        internal void LootRoom(Player current_player)
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
        /// Move onto the next room.
        /// </summary>
        internal void AdventureOn(Player current_player)
        {
            StringBuilder directions_menu = new StringBuilder("Choose a direction:\n");
            foreach(var neighbour in GetNeighbours())
            {
                directions_menu.Append($"\n :: {neighbour.Value}");
            }

            string direction = GameUI.GetInput(GetNeighbours().Values.ToList(), directions_menu.ToString());

            Room next_room = this.MoveToNeighbour(direction);
            GameUI.DisplayRoom(next_room, current_player);
        }
    }
}