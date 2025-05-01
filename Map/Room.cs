namespace DungeonExplorer;
/// <summary>
/// Enum for all types of rooms.
/// </summary>
public enum RoomType 
{ 
    Empty, 
    Loot, 
    Easy, 
    Medium, 
    Hard,
    Boss,
}

/// <summary>
/// Class representing a <see cref="Room"> in the dungeon.
/// </summary>
public class Room
{
    /// <summary>
    /// The Room object holds all functionality for each room in this game.
    /// </summary>
    private static readonly Random _random = new Random();

    private readonly int _maxBranching = 3;
    private readonly int _maxDepth = 5;
    public RoomType Type { get; private set; }
    private Inventory _loot { get; set; }
    private List<Monster> _monsters { get; set; }
    private Dictionary<Direction, Room> _neighbours { get; set; } = new Dictionary<Direction, Room>();
    public string Description { get; private set; }
    private static List<string> _descriptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class.
    /// </summary>
    public Room(int maxBranching = 3, int depth = 0,
        RoomType type = RoomType.Empty, Inventory loot = null, List<Monster> monsters = null, 
        Dictionary<Direction, Room> neighbours = null, string description = null)
    {
        UI.Message($"{depth} : New Room");

        // Set the base values for the room
        _maxBranching = Math.Clamp(maxBranching, 1, 3);
        _maxDepth = Math.Clamp(depth, 5, 8);
        Type = type;
        _loot = loot ?? new Inventory();
        _monsters = monsters ?? new List<Monster>();

        string descriptionString = FileManager.ReadFileAsync("room_descriptions.txt").Result;
        _descriptions = descriptionString
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        UI.Message("Base values set.");

        Description = string.IsNullOrEmpty(description) ? GenerateDescription() : description;

        // Randomly generate loot, monsters, and neighbours
        GenerateLoot();
        UI.Message("Loot values set.");
        GenerateMonsters();
        UI.Message("Monster values set.");

        if (neighbours != null) _neighbours = new Dictionary<Direction, Room>(neighbours);

        if (depth <= 5 && neighbours == null) GenerateNeighbours(depth + 1);
        UI.Message("Neighbour values set.");
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier</returns>
    public string GetSaveIdentifier() => $"room_{GetHashCode()}";

    #region Room Generation

    /// <summary>
    /// Generates the neighbours.
    /// </summary>
    /// <param name="depth">The depth.</param>
    public void GenerateNeighbours(int depth = 0)
    {
        // Check if the room is already generated
        _neighbours = _neighbours ?? new Dictionary<Direction, Room>();

        if (depth >= _maxDepth) // Prevent excessive recrursion
        {
            UI.Message($"Max depth reached at depth {depth}.", wait: false);
            return;
        }

        // Available directions
        var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        directions = directions.OrderBy(x => _random.Next()).ToList(); // Shuffle directions

        // Determine the number of branches
        int branches = _random.Next(1, Math.Min(_maxBranching, directions.Count) + 1);

        UI.Message($"Depth: {depth}, Branches: {branches}, Directions: {string.Join(", ", directions)}", wait: false);

        int generatedBranches = 0;

        foreach (var direction in directions)
        {
            if (generatedBranches >= branches) break; // Stop if we've generated enough branches

            if (_neighbours.ContainsKey(direction))
            {
                UI.Message($"Skipping direction {direction} as it already has a neighbor.", wait: false);
                continue; // Skip if neighbor already exists
            }

            // Generate a new room and add it as a neighbor
            var newRoom = GenerateRoom(depth + 1);
            if (newRoom == null)
            {
                UI.Message($"Failed to generate room in direction {direction} at depth {depth + 1}.", wait: false);
                continue;
            }

            AddNeighbour(direction, newRoom);
            newRoom.AddNeighbour(GetOppositeDirection(direction), this);

            UI.Message($"Generated neighbor in direction {direction} at depth {depth + 1}.", wait: false);

            generatedBranches++;
        }

        // Debugging: Log the generated neighbors
        if (_neighbours.Count == 0)
        {
            UI.Message($"No neighbors generated at depth {depth}.", wait: false);
        }
        else
        {
            UI.Message($"Generated neighbors at depth {depth}: {string.Join(", ", _neighbours.Keys)}", wait: false);
        }

        // Ensure the required number of branches were generated
        if (generatedBranches < branches)
        {
            UI.Message($"Warning: Only {generatedBranches} out of {branches} branches were generated at depth {depth}.", wait: false);
        }
    }

    /// <summary>
    /// Generates the room.
    /// </summary>
    /// <param name="depth">The depth.</param>
    /// <param name="maxDepth">The maximum depth.</param>
    /// <returns>The room</returns>
    public static Room GenerateRoom(int depth = 1, int maxDepth = 8)
    {
        if (depth > maxDepth) return new Room(type: RoomType.Empty); // Prevent excessive recursion

        return depth switch // Generate a room based on the depth
        {
            1 => new Room(depth: depth, type: RoomType.Easy),
            2 => new Room(depth: depth, type: RoomType.Easy),
            3 => new Room(depth: depth, type: RoomType.Medium),
            4 => new Room(depth: depth, type: RoomType.Medium),
            5 => new Room(depth: depth, type: RoomType.Hard),
            6 => new Room(depth: depth, type: RoomType.Hard),
            7 => new Room(depth: depth, type: RoomType.Boss),
            _ => new Room(type: RoomType.Empty)
        };
    }

    #endregion

    /// <summary>
    /// Adds the neighbour.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <param name="room">The room.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public void AddNeighbour(Direction direction, Room room)
    {
        ArgumentNullException.ThrowIfNull(room);
        if (_neighbours.ContainsKey(direction)) return;
        _neighbours.Add(direction, room);
    }

    /// <summary>
    /// Gets the neighbours.
    /// </summary>
    /// <returns>The neighbours</returns>
    public Dictionary<Direction, Room> GetNeighbours() => _neighbours;

    /// <summary>
    /// Sets the neighbours.
    /// </summary>
    /// <param name="n">The n.</param>
    public void SetNeighbours(Dictionary<Direction, Room> n) => _neighbours = new (n);

    /// <summary>
    /// Gets the neighbour.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns>The neighbouring <see cref="Room"/></returns>
    public Room GetNeighbour(Direction direction) => _neighbours.TryGetValue(direction, out Room neighbour) ? neighbour : null;

    /// <summary>
    /// Determines whether this instance has neighbours.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance has neighbours; otherwise, <c>false</c>.
    /// </returns>
    public bool HasNeighbours() => _neighbours.Count > 0;

    /// <summary>
    /// Gets the opposite direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns>The <see cref="Direction"/></returns>
    private static Direction GetOppositeDirection(Direction direction)
    {
        return direction switch // Get the opposite direction
        {
            Direction.North => Direction.South,
            Direction.East => Direction.West,
            Direction.South => Direction.North,
            Direction.West => Direction.East,
        };
    }

    #region Description

    /// <summary>
    /// Generates the description.
    /// </summary>
    /// <returns>The random description</returns>
    public static string GenerateDescription()
    {
        if (_descriptions == null || _descriptions.Count == 0)
        {
            return "An eerily empty room.";
        }
        return _descriptions[_random.Next(_descriptions.Count)];
    }

    #endregion
    
    #region Loot

    /// <summary>
    /// Removes a specified item from loot.
    /// </summary>
    public void RemoveLoot(ICollectable item)
    {
        // If loot contains the specified item
        if (_loot.Items.Contains(item))
        {
            // Then remove it
            _loot.RemoveItem(item);
        }
        else
        {
            // If not then display appropriate message
            UI.Message("You do not have this item.");
        }

    }

    /// <summary>
    /// Loots the room.
    /// </summary>
    /// <returns>The item.</returns>
    public ICollectable LootRoom()
    {
        StringBuilder output = new StringBuilder("You search the room and find:\n");

        if (_loot.Items.Count == 0) // No loot found
        {
            output.AppendLine("Nothing of use.");
            UI.Message(output.ToString(), wait: true);
            return null;
        }

        for (int i = 0; i < _loot.Items.Count; i++) // Display loot items
        {
            ICollectable item = _loot.Items[i];
            output.AppendLine($"\t{i + 1}. {item.Name} : {item.Description}");
        }
        output.AppendLine($"\t{_loot.Count + 1}. Take nothing.");
        output.AppendLine("Choose an item to take: ");

        // Get user input for loot selection
        List<string> inputRange = Enumerable.Range(0, _loot.Count + 1)
            .Select(x => x.ToString())
            .ToList();
        string lootIndex = UI.GetInput(inputRange, output.ToString());
        
        if (int.TryParse(lootIndex, out int index)) return _loot.Items[index - 1];
        else return LootRoom();
    }

    /// <summary>
    /// Generates the loot.
    /// </summary>
    public void GenerateLoot()
    {
        if (Type == RoomType.Empty) return;

        int lootCount = _random.Next(1, 4); // Random number of loot items (1 to 3)
        UI.Message($"Generating {lootCount} loot items for room of type {Type}.", wait: false);

        ICollectable newItem = null;

        for (int i = 0; i <= lootCount; i++)
        {
            int randomType = _random.Next(0, 7); // Randomly choose between 0, 1, or up to 6

            if (randomType == 0)
            {
                newItem = Armour.GenerateItem();
            }
            else if (randomType == 1)
            {
                newItem = Weapon.GenerateItem();
            }
            else
            {
                newItem = Item.GenerateItem();
            }

            if (newItem != null)
            {
                _loot.AddItem(newItem); // Add the generated item to the loot inventory
                UI.Message($"Generated loot: {newItem.Name} - {newItem.Description}", wait: false);
            }
        }

        if (_loot.Count == 0)
        {
            UI.Message("No loot was generated for this room.", wait: false);
        }
        else
        {
            UI.Message($"Loot generated: {_loot.Count} items.", wait: false);
        }
    }

    /// <summary>
    /// Determines whether this instance has loot.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance has loot; otherwise, <c>false</c>.
    /// </returns>
    public bool HasLoot() => _loot.Count > 0;

    #endregion
    #region Monsters

    /// <summary>
    /// Generates the monsters.
    /// </summary>
    public void GenerateMonsters()
    {
        switch (Type) // Generate monsters based on room type
        {
            case RoomType.Easy:
                _monsters.Add(new ClockworkMage());
                break;

            case RoomType.Medium:
                for (int i = _random.Next(1, 3); i > 0; i--) _monsters.Add(new RepairUnit());
                break;

            case RoomType.Hard:
                _monsters.Add(new RustingConstruct());
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Removes the monsters.
    /// </summary>
    public void RemoveMonsters() => _monsters.RemoveAll(m => m.Health <= 0);

    /// <summary>
    /// Gets the monsters.
    /// </summary>
    /// <returns>The list of monsters.</returns>
    public List<Monster> GetMonsters() => _monsters;

    /// <summary>
    /// Determines whether this instance has monsters.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance has monsters; otherwise, <c>false</c>.
    /// </returns>
    public bool HasMonsters() => _monsters.Count > 0;

    #endregion

    #region Overrides 

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        // Display the description
        builder.AppendLine($"What you see...\n\t{Description}");

        // Display the loot 
        if (_loot.Items.Count != 0)
            builder.AppendLine("\nYou see items scattered around...");

        if (_monsters.Any(m => m.Health > 0))
            builder.AppendLine("\nYou see monsters lurking in the shadows...");

        if (HasNeighbours())
        {
            builder.AppendLine("\nYou see doors leading to other rooms...");
            foreach (var neighbour in _neighbours)
            {
                builder.AppendLine($"{neighbour.Key} :: {neighbour.Value.Description}");
            }
        }
        else
        {
            builder.AppendLine("\nYou see no doors leading to other rooms...");
        }

            // Return the display 
            return builder.ToString() + "\n";
    }

    #endregion
}