using System.Security.Cryptography;

namespace DungeonExplorer;
public enum RoomType 
{ 
    Empty, 
    Loot, 
    Easy, 
    Medium, 
    Hard,
    Boss,
} 

public class Room
{
    /// <summary>
    /// The Room object holds all functionality for each room in this game.
    /// </summary>

    private int _maxBranching = 3;
    public RoomType Type { get; private set; }
    private Inventory _loot { get; set; }
    private List<Monster> _monsters { get; set; }
    private Dictionary<Direction, Room> _neighbours { get; set; }
    public string Description { get; private set; }
    private static List<string> _descriptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Room"/> class.
    /// </summary>
    public Room(int maxBranching = 3, int depth = 0,
        RoomType type = RoomType.Empty, Inventory loot = null, List<Monster> monsters = null, 
        Dictionary<Direction, Room> neighbours = null, string description = null)
    {
        _maxBranching = Math.Clamp(maxBranching, 1, 3);

        Type = type;
        
        if (loot == null) _loot = new Inventory();
        else _loot = loot;

        if (monsters == null) _monsters = new List<Monster>();
        else _monsters = monsters;

        if (neighbours == null) _neighbours = GenerateNeighbours(depth);
        else _neighbours = neighbours;

        string descriptionString = FileManager.ReadFileAsync("room_descriptions.txt").Result;

        _descriptions = descriptionString
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        Description = string.IsNullOrEmpty(description) ? GenerateDescription() : description;
        GenerateLoot();
        GenerateMonsters();
    }

    #region Save Implementation
    public string GetSaveIdentifier() => $"room_{GetHashCode()}";

    #endregion

    #region Room Generation

    public Dictionary<Direction, Room> GenerateNeighbours(int depth = 0)
    {
        int maxDepth = 6;

        if (depth >= maxDepth) return new Dictionary<Direction, Room>();

        var neighbours = new Dictionary<Direction, Room>();
        var directions = new List<Direction>() {
            Direction.North,
            Direction.East,
            Direction.West
        };
        var random = new Random();
        int branches = random.Next(2, 4); // Random number of branches (2 to 3)

        for (int i = 0; i < branches && i < directions.Count; i++)
        {
            var direction = directions[i];
            var newRoom = GenerateRandomRoom(depth + 1);
            neighbours.Add(direction, newRoom);
            newRoom.AddNeighbour(Direction.South, this);
        }

        return neighbours;
    }

    public static Room GenerateStartingRoom()
    {
        Room room = GenerateEmptyRoom();
        room.Description = "You find yourself in a dimly lit chamber, the air thick with dust and the scent of damp stone.";
        room.AddNeighbour(Direction.North, GenerateRandomRoom(1));
        room.AddNeighbour(Direction.East, GenerateRandomRoom(1));
        room.AddNeighbour(Direction.West, GenerateRandomRoom(1));
        return room;
    }

    public static Room GenerateRandomRoom(int level = 1)
    {
        var random = new Random();
        switch (level) // Randomise the room type based on depth
        {
            case 0:
                return GenerateEmptyRoom();
            case 1:
                return new Room(depth: level,
                    type: random.Next(0, 2) == 0 ? RoomType.Easy : RoomType.Loot);
            case 2:
                return new Room(depth: level,
                    type: random.Next(0, 2) == 0 ? RoomType.Easy : RoomType.Medium);
            default:
                return new Room(depth: level,
                    type: random.Next(0, 2) == 0 ? RoomType.Hard : RoomType.Medium);
        }
    }

    public static Room GenerateBossRoom()
    {
        Room room = new Room(type: RoomType.Boss);
        room.Description = "You enter a large chamber, the air thick with tension. A powerful presence looms in the shadows.";
        return room;
    }

    public static Room GenerateEmptyRoom()
    {
        return new Room(type: RoomType.Empty);
    }

    #endregion

    public void AddNeighbour(Direction direction, Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        if (_neighbours.ContainsKey(direction)) return;
        _neighbours.Add(direction, room);
    }

    public Dictionary<Direction, Room> GetNeighbours() => _neighbours;

    public void SetNeighbours(Dictionary<Direction, Room> n) => _neighbours = new ();

    public Room GetNeighbour(Direction direction)
    {
        return _neighbours.TryGetValue(direction, out Room neighbour) ? neighbour : null;
    }

    public bool HasNeighbours() => _neighbours.Count > 0;

    #region Description

    private static List<string> GetDefaultDescriptions()
    {
        return new List<string>
        {
            "A dimly lit chamber with damp stone walls.",
            "The air here feels thick and musty.",
            "Flickering torchlight casts strange shadows.",
            "You hear distant dripping water echoing."
        };
    }

    public static string GenerateDescription()
    {
        var random = new Random();

        if (_descriptions == null || _descriptions.Count == 0)
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
    public ICollectable LootRoom()
    {
        StringBuilder output = new StringBuilder("You search the room and find:\n");

        if (_loot.Items.Count == 0)
        {
            output.AppendLine("Nothing of use.");
            UI.Message(output.ToString(), wait: true);
            return null;
        }

        for (int i = 0; i < _loot.Items.Count; i++)
        {
            ICollectable item = _loot.Items[i];
            output.AppendLine($"\t{i + 1}. {item.Name} : {item.Description}");
        }
        output.AppendLine($"\t{_loot.Count + 1}. Take nothing.");
        output.AppendLine("Choose an item to take: ");

        List<string> inputRange = Enumerable.Range(0, _loot.Count + 1)
            .Select(x => x.ToString())
            .ToList();
        string lootIndex = UI.GetInput(inputRange, output.ToString());


        if (int.TryParse(lootIndex, out int index)) return _loot.Items[index - 1];
        else return LootRoom();

    }

    public void GenerateLoot()
    {
        if (Type == RoomType.Empty) return;

        var random = new Random();
        int lootCount = random.Next(1, 4); // Random number of loot items (1 to 3)
        for (int i = 0; i < lootCount; i++)
        {
            int randomType = random.Next(0, i * 4);
            ICollectable newItem = null;
            if (randomType == 0)
            {
                newItem = Armour.GenerateItem();
            }
            else if(randomType == 1)
            {
                newItem = Weapon.GenerateItem();
            }
            else
            {
                newItem = Item.GenerateItem();
            }
        }
    }

    public bool HasLoot() => _loot.Count > 0;

    #endregion
    #region Monsters

    public void GenerateMonsters()
    {
        switch (Type)
        {
            case RoomType.Easy:
                _monsters.Add(new ClockworkMage());
                break;
            case RoomType.Medium:
                for (int i = new Random().Next(1, 3); i > 0; i--)
                {
                    _monsters.Add(new RepairUnit());
                }
                break;
            case RoomType.Hard:
                _monsters.Add(new RustingConstruct());
                break;
            default:
                break;
        }
    }

    public void RemoveMonsters() => _monsters.RemoveAll(m => m.Health <= 0);

    public List<Monster> GetMonsters() => _monsters; 

    public bool HasMonsters() => _monsters.Count > 0;

    #endregion
    #region Overrides 

    /// <summary>
    /// Returns the description and contents of the room
    /// </summary>
    /// <returns></returns>
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