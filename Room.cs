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
        private List<Item> _loot { get; set; }
        public string Description { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room"/> class.
        /// </summary>
        public Room(List<string> availableDescriptions = null)
        {
            _descriptions = availableDescriptions ?? LoadDescriptions();

            // Generate new description
            if (_descriptions.Count != 0)
            {
                Description = GenerateDescription();
                // Remove used description from the list
                _descriptions.Remove(Description);
            }
            else
            {
                Description = "A nondescript room.";
            }

            GenerateLoot();
        }

        #region Description
        private static List<string> _descriptions { get; set; }

        private static List<string> LoadDescriptions(string source = "room_descriptions.txt")
        {
            List<string> defualtDescriptions = new List<string> {
                "A dimly lit chamber.",
                "A cold, stone-walled room.",
                "A dusty corridor.",
                "A spacious hall with echoing sounds."
            };

            try
            {
                if (!File.Exists(source))
                {
                    return defualtDescriptions;
                }

                var descriptions = File.ReadAllLines(source)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
                return descriptions.Any() ? descriptions : defualtDescriptions;
            }
            catch (Exception e)
            {
                string errorMessage = $"Error loading room: {e.Message}";
                UI.DisplayMessage(errorMessage, wait: true);
                return defualtDescriptions;
            }
        }

        private string GenerateDescription()
        {
            Random random = new Random();

            if (Description == null || Description.Count() == 0)
            {
                return "An eerily empty room.";
            }
            return _descriptions[random.Next(_descriptions.Count)];
        }

        #endregion
        #region Loot

        /// <summary>
        /// Removes a specified item from loot.
        /// </summary>
        public void RemoveLoot(Item item)
        {
            // If loot contains the specified item
            if (_loot.Contains(item))
            {
                // Then remove it
                _loot.Remove(item);
            }
            else
            {
                // If not then display appropriate message
                UI.DisplayMessage("You do not have this item.");
            }

        }

        /// <summary>
        /// Gets the index of the loot.
        /// </summary>
        /// <param name="loot_count">The loot count.</param>
        /// <param name="current_loot">The current loot.</param>
        /// <returns></returns>
        private int GetLootIndex()
        {
            int lootCount = _loot.Count();
            StringBuilder output = new StringBuilder("\nThe room contains:\n");

            // Display the loot as a numbered list
            for (int i = 0; i < lootCount; i++)
            {
                output.Append($"\n{i + 1}. {_loot[i]}");
            }

            output.Append($"\n{lootCount + 1}. None");

            // Get the users number choice corresponding to the item they want
            output.Append("\nEnter the item you'd like to loot.");

            // Get the loot input
            List<string> inputRange = Enumerable.Range(1, lootCount + 1)
                .Select(x => x.ToString())
                .ToList();
            string input = UI.GetInput(inputRange, output.ToString());

            // The index of the loot
            return int.Parse(input) - 1;
        }

        /// <summary>
        /// Loots the room.
        /// </summary>
        /// <param name="current_player">The current player.</param>
        public void LootRoom(Player current_player)
        {
            if (!_loot.Any())
            {
                UI.DisplayMessage("You could not find any useful items.");
                return;
            }

            StringBuilder output = new StringBuilder("You search the room and find:\n");
            for (int i = 0; i < _loot.Count; i++)
            {
                Item item = _loot[i];
                output.AppendLine($"\t{i + 1}. {item.Name} : {item.Description}");
            }
            output.AppendLine($"\t{_loot.Count + 1}. Take nothing.");
            output.AppendLine("Choose an item to take: ");

            List<string> inputRange = Enumerable.Range(0, _loot.Count())
                .Select(x => x.ToString())
                .ToList();
            UI.GetInput();
        }

        #endregion
        #region Overrides 

        /// <summary>
        /// Returns the description and contents of the room
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder room_string = new StringBuilder();

            // Display the description
            room_string.AppendLine($"What you see...\n\n    {Description}");

            // Display the loot 
            if (_loot.Any())
            {
                room_string.AppendLine("\nYou can see things worth picking up!");
                foreach (var item in _loot)
                {
                    room_string.AppendLine($" :: {item}");
                }
            }
            else
            {
                room_string.AppendLine("\nThere is nothing of worth in this room.");
            }

            // Return the display 
            return room_string.ToString();
        }

        #endregion
    }
}