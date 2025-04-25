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
        private Dictionary<Direction, Room> _neighbours { get; set; }
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

        public Room GetNeighbour(Direction direction)
        {
            if (_neighbours.TryGetValue(direction, out Room r)) return r;
            return null;
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
        /// Loots the room.
        /// </summary>
        public Item LootRoom()
        {
            StringBuilder output = new StringBuilder("You search the room and find:\n");

            if (!_loot.Any())
            {
                output.AppendLine("Nothing of use.");
                UI.DisplayMessage(output.ToString(), wait: true);
                return null;
            }

            for (int i = 0; i < _loot.Count; i++)
            {
                Item item = _loot[i];
                output.AppendLine($"\t{i + 1}. {item.Name} : {item.Description}");
            }
            output.AppendLine($"\t{_loot.Count + 1}. Take nothing.");
            output.AppendLine("Choose an item to take: ");

            List<string> inputRange = Enumerable.Range(0, _loot.Count)
                .Select(x => x.ToString())
                .ToList();
            string lootIndex = UI.GetInput(inputRange, output.ToString());


            if (int.TryParse(lootIndex, out int index)) return _loot[index - 1];
            else return LootRoom();
            
        }

        public void GenerateLoot()
        {
            Random random = new Random();
            if (random.Next(0, LootChance) == 0)
            {

            }
        }

        #endregion
        #region Monsters

        public void GenerateMonsters()
        {

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