namespace DungeonExplorer
{
    /// <summary>
    /// Handles all game save and load operations
    /// </summary>
    public static class SaveManager
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Data transfer object for player save data
        /// </summary>
        private class SaveData
        {
            public PlayerData Player { get; set; }
            public InventoryData Inventory { get; set; }
            public StatisticsData Statistics { get; set; }
        }

        /// <summary>
        /// Contains all serialisable player attributes
        /// </summary>
        private class PlayerData
        {
            public string Name { get; set; }
            public int MaxHealth { get; set; }
            public int Defence { get; set; }
            public int AttackPower { get; set; }
            public int Experience { get; set; }
            public int Level { get; set; }
        }

        /// <summary>
        /// Represents the player's inventory state
        /// </summary>
        private class InventoryData
        {
            public List<ItemData> Items { get; set; } = new();
            public string EquippedWeaponId { get; set; }
            public string EquippedArmourId { get; set; }
        }

        /// <summary>
        /// Base class for all inventory items
        /// </summary>
        private class ItemData
        {
            public string Id { get; set; }
            public string Type { get; set; } // "Item", "Weapon", "Armour", "Potion"
            public string Name { get; set; }
            public string Description { get; set; }

            // Weapon-specific properties
            public int? DamageModifier { get; set; }

            // Armour-specific properties
            public int? DefenceModifier { get; set; }

            // Potion-specific properties
            public PotionEffect? EffectType { get; set; }
            public int? EffectDuration { get; set; }
            public int? EffectPower { get; set; }
        }

        /// <summary>
        /// Tracks game statistics and metrics
        /// </summary>
        private class StatisticsData
        {
            public int RoomsVisited { get; set; }
            public int MonstersKilled { get; set; }
            public int ItemsCollected { get; set; }
            public int TotalDamageDealt { get; set; }
            public int TotalDamageTaken { get; set; }
            public DateTime StartTime { get; set; }
        }

        /// <summary>
        /// Contains the complete game world state
        /// </summary>
        private class WorldData
        {
            public string CurrentRoomId { get; set; }
            public List<RoomData> Rooms { get; set; } = new();
        }

        /// <summary>
        /// Represents a serialisable room state including neighbour connections
        /// </summary>
        private class RoomData
        {
            public string Id { get; set; }

            public string Type { get; set; }

            public string Description { get; set; }

            public Dictionary<string, NeighbourData> Neighbours { get; set; } = new();

            public class NeighbourData
            {
                public string RoomId { get; set; }

                public string Type { get; set; }
            }

            /// <summary>
            /// Converts a Room object to serialisable data
            /// </summary>
            /// <param name="room">The room to serialise</param>
            /// <returns>Serialisable room data</returns>
            public static RoomData FromRoom(Room room)
            {
                var data = new RoomData
                {
                    Id = room.GetSaveIdentifier(),
                    Type = room.Type.ToString(),
                    Description = room.Description
                };

                // Convert neighbours to serialisable format
                foreach (var neighbour in room.GetNeighbours())
                {
                    data.Neighbours[neighbour.Key.ToString()] = new NeighbourData
                    {
                        RoomId = neighbour.Value.GetSaveIdentifier(),
                        Type = neighbour.Value.Type.ToString()
                    };
                }

                return data;
            }

            /// <summary>
            /// Creates a Room object from serialised data (without neighbours)
            /// </summary>
            /// <returns>New Room instance</returns>
            public Room ToRoom()
            {
                return new Room(type: Enum.Parse<RoomType>(Type), description: Description);
                // Neighbours will be reconnected after all rooms are loaded
            }
        }

        /// <summary>
        /// Prompts player to select a save slot
        /// </summary>
        /// <returns>Selected slot number (1-3) or 0 if cancelled</returns>
        public static int GetSaveSlot()
        {
            while (true)
            {
                Console.Clear();
                UI.Message("=== Choose Save Slot ===", clear: false);

                for (int i = 1; i <= 3; i++)
                {
                    string status = SaveExists(i) ? GetSaveInfo(i) : "EMPTY";
                    UI.Message($"{i}. Slot {i}: {status}", clear: false);
                }

                UI.Message("Q. Back to Main Menu", clear: false);

                var validInputs = new List<string> { "Q", "1", "2", "3" };
                string input = UI.GetInput(validInputs, "Select slot 1-3 or Q to cancel:");

                if (int.TryParse(input, out int slot) && slot > 0 && slot <= 3)
                {
                    return slot;
                }
                else if (input.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    return 0; // Cancelled
                }
            }
        }

        /// <summary>
        /// Checks if a save exists in the specified slot
        /// </summary>
        /// <param name="slotNumber">Slot to check (1-3)</param>
        /// <returns>True if save data exists</returns>
        public static bool SaveExists(int slotNumber)
        {
            string savePath = GetSavePath(slotNumber);
            return Directory.Exists(savePath) &&
                   File.Exists(Path.Combine(savePath, "player_data.json"));
        }

        /// <summary>
        /// Gets summary information about a save slot
        /// </summary>
        /// <param name="slotNumber">Slot to check (1-3)</param>
        /// <returns>Formatted save information or "CORRUPTED"</returns>
        public static string GetSaveInfo(int slotNumber)
        {
            try
            {
                string savePath = GetSavePath(slotNumber);
                string json = File.ReadAllText(Path.Combine(savePath, "player_data.json"));
                var data = JsonSerializer.Deserialize<SaveData>(json, jsonOptions);

                return $"{data.Player.Name} (Lvl {data.Player.Level})";
            }
            catch
            {
                return "CORRUPTED";
            }
        }

        /// <summary>
        /// Saves the current game state to the specified slot
        /// </summary>
        /// <param name="slotNumber">Slot to save to (1-3)</param>
        /// <param name="player">Current player instance</param>
        /// <param name="map">Current game map</param>
        public static void SaveGame(int slotNumber, Player player, GameMap map)
        {
            string savePath = GetSavePath(slotNumber);
            Directory.CreateDirectory(savePath);

            // Convert inventory items
            var inventoryData = new InventoryData();
            foreach (var item in player.Inventory.Items)
            {
                var itemData = new ItemData
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Description = item.Description,
                    Type = item.GetType().Name
                };

                switch (item) // Build item data based on type
                {
                    case Weapon weapon:
                        itemData.DamageModifier = weapon.DamageModifier;
                        if (player.EquippedWeapon == weapon)
                            inventoryData.EquippedWeaponId = itemData.Id;
                        break;

                    case Armour armour:
                        itemData.DefenceModifier = armour.DefenceModifier;
                        if (player.EquippedArmour == armour)
                            inventoryData.EquippedArmourId = itemData.Id;
                        break;

                    case Potion potion:
                        itemData.EffectType = potion.EffectType;
                        itemData.EffectDuration = potion.EffectDuration;
                        itemData.EffectPower = potion.EffectPower;
                        break;
                }

                inventoryData.Items.Add(itemData);
            }

            // Save player data
            var saveData = new SaveData
            {
                Player = new PlayerData
                {
                    Name = player.Name,
                    MaxHealth = player.MaxHealth,
                    Defence = player.Defence,
                    AttackPower = player.AttackPower,
                    Experience = player.Experience,
                    Level = player.Level
                },
                Inventory = inventoryData,
                Statistics = new StatisticsData
                {
                    RoomsVisited = player.Statistics.RoomsVisited,
                    MonstersKilled = player.Statistics.MonstersKilled,
                    ItemsCollected = player.Statistics.ItemsCollected,
                    TotalDamageDealt = player.Statistics.TotalDamageDealt,
                    TotalDamageTaken = player.Statistics.TotalDamageTaken,
                    StartTime = player.Statistics.StartTime
                }
            };

            File.WriteAllText(
                Path.Combine(savePath, "player_data.json"),
                JsonSerializer.Serialize(saveData, jsonOptions)
            );

            // Save world data
            var worldData = new WorldData
            {
                CurrentRoomId = map.CurrentRoom.GetSaveIdentifier(),
                Rooms = map.Map.Select(RoomData.FromRoom).ToList()
            };

            File.WriteAllText(
                Path.Combine(savePath, "world_data.json"),
                JsonSerializer.Serialize(worldData, jsonOptions)
            );
        }

        /// <summary>
        /// Reconstructs room neighbour connections after loading
        /// </summary>
        /// <param name="worldData">Loaded world data</param>
        /// <param name="rooms">Dictionary of loaded rooms by ID</param>
        private static void ReconnectRoomNeighbours(WorldData worldData, Dictionary<string, Room> rooms)
        {
            foreach (var roomData in worldData.Rooms)
            {
                if (!rooms.TryGetValue(roomData.Id, out var currentRoom))
                    continue;

                var neighbours = roomData.Neighbours.ToDictionary(
                    n => Enum.Parse<Direction>(n.Key),
                    n => rooms.TryGetValue(n.Value.RoomId, out var neighbourRoom)
                        ? neighbourRoom
                        : throw new InvalidDataException($"Missing neighbour room: {n.Value.RoomId}")
                );

                currentRoom.SetNeighbours(neighbours);
            }
        }

        /// <summary>
        /// Loads game state from specified slot
        /// </summary>
        /// <param name="slotNumber">Slot to load from (1-3)</param>
        /// <returns>Tuple containing player and map instances</returns>
        public static (Player, GameMap) LoadGame(int slotNumber)
        {
            string savePath = GetSavePath(slotNumber);

            // Load player data
            var saveData = JsonSerializer.Deserialize<SaveData>(
                File.ReadAllText(Path.Combine(savePath, "player_data.json")),
                jsonOptions);

            var player = new Player(
                saveData.Player.Name, saveData.Player.Level, saveData.Player.Experience,
                saveData.Player.MaxHealth, saveData.Player.Defence, saveData.Player.AttackPower);

            // Restore inventory
            foreach (var itemData in saveData.Inventory.Items)
            {
                ICollectable item = itemData.Type switch
                {
                    "Weapon" => new Weapon(
                        itemData.Name,
                        itemData.Description,
                        itemData.DamageModifier ?? 0),

                    "Armour" => new Armour(
                        itemData.Name,
                        itemData.Description,
                        itemData.DefenceModifier ?? 0),

                    "Potion" => new Potion(
                        itemData.Name,
                        itemData.Description,
                        itemData.EffectType ?? PotionEffect.Heal,
                        itemData.EffectDuration ?? 0,
                        itemData.EffectPower ?? 0),

                    _ => new Item(itemData.Name, itemData.Description)
                };

                player.Inventory.AddItem(item);

                // Restore equipped items
                if (itemData.Id == saveData.Inventory.EquippedWeaponId && item is Weapon weapon)
                    player.EquipWeapon(weapon);

                if (itemData.Id == saveData.Inventory.EquippedArmourId && item is Armour armour)
                    player.EquipArmour(armour);
            }

            // Restore statistics
            player.Statistics.UpdateRoomsVisited(saveData.Statistics.RoomsVisited, true);
            player.Statistics.UpdateMonstersKilled(saveData.Statistics.MonstersKilled, true);
            player.Statistics.UpdateItemsCollected(saveData.Statistics.ItemsCollected, true);
            player.Statistics.UpdateTotalDamageDealt(saveData.Statistics.TotalDamageDealt, true);
            player.Statistics.UpdateTotalDamageTaken(saveData.Statistics.TotalDamageTaken, true);
            player.Statistics.UpdateStartTime(saveData.Statistics.StartTime);

            // Load world data
            var worldData = JsonSerializer.Deserialize<WorldData>(
                File.ReadAllText(Path.Combine(savePath, "world_data.json")),
                jsonOptions);

            // First create all rooms without neighbours
            var rooms = worldData.Rooms
                .Select(r => r.ToRoom())
                .ToDictionary(r => r.GetSaveIdentifier());

            // Then reconnect all neighbours
            ReconnectRoomNeighbours(worldData, rooms);

            // Find the starting room
            if (!rooms.TryGetValue(worldData.CurrentRoomId, out var startingRoom))
            {
                startingRoom = new Room(depth: 1);
            }

            var map = new GameMap(startingRoom);

            return (player, map);
        }

        /// <summary>
        /// Deletes save data from specified slot
        /// </summary>
        /// <param name="slotNumber">Slot to delete (1-3)</param>
        public static void DeleteSave(int slotNumber)
        {
            string savePath = GetSavePath(slotNumber);
            if (Directory.Exists(savePath))
            {
                Directory.Delete(savePath, true);
            }
        }

        /// <summary>
        /// Gets the filesystem path for a save slot
        /// </summary>
        /// <param name="slotNumber">Slot number (1-3)</param>
        /// <returns>Full path to save directory</returns>
        private static string GetSavePath(int slotNumber)
        {
            return Path.Combine("saves", $"slot_{slotNumber}");
        }
    }
}