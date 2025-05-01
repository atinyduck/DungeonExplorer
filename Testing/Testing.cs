
namespace DungeonExplorer;

/// <summary>
/// This class is used to test the game.
/// </summary>
public static class Testing
{
    private static readonly string LogFilePath = "test_log.txt";

    /// <summary>
    /// Initializes the testing environment by clearing the log file.
    /// </summary>
    public static void Initialize()
    {
        if (File.Exists(LogFilePath))
        {
            File.Delete(LogFilePath);
        }
        Log("=== Testing Initialized ===");
    }

    /// <summary>
    /// Logs a message to the log file.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void Log(string message)
    {
        File.AppendAllText(LogFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
    }

    /// <summary>
    /// Runs all tests.
    /// </summary>
    public static void RunAllTests()
    {
        Initialize();

        Log(" ----- Starting tests... ----- ");
        UI.Message("Starting tests...", clear: true);
        TestRoomGeneration();
        TestLootGeneration();
        TestMonsterBehavior();
        TestInventoryManagement();
        UI.Message("All tests completed.", wait: true);
        Log(" ----- All tests completed. ----- ");
    }

    /// <summary>
    /// Tests room generation functionality.
    /// </summary>
    private static void TestRoomGeneration()
    {
        Log("Testing Room Generation...");
        UI.Message("Testing Room Generation...", clear: true);
        var room = new Room(depth: 1, type: RoomType.Medium);
        Debug.Assert(room.Type == RoomType.Medium, "Room type should be Medium.");
        Debug.Assert(!string.IsNullOrEmpty(room.Description), "Room description should not be empty.");
        Debug.Assert(room.HasNeighbours(), "Room should have neighbors.");
        UI.Message("Room generation test passed.", wait: true);
        Log("Room Generation Test Passed.");
    }

    /// <summary>
    /// Tests loot generation functionality.
    /// </summary>
    private static void TestLootGeneration()
    {
        Log("Testing Loot Generation...");
        UI.Message("Testing Loot Generation...", clear: true);

        var room = new Room(type: RoomType.Loot);
        Debug.Assert(room.HasLoot(), "Room with type 'Loot' should have loot.");

        var loot = room.LootRoom();
        Debug.Assert(loot != null, "LootRoom should return a valid item if loot exists.");
        Debug.Assert(!string.IsNullOrEmpty(loot.Name), "Loot item should have a name.");
        Debug.Assert(!string.IsNullOrEmpty(loot.Description), "Loot item should have a description.");
        UI.Message("Loot generation test passed.", wait: true);
        Log("Loot Generation Test Passed.");
    }

    /// <summary>
    /// Tests monster behavior.
    /// </summary>
    private static void TestMonsterBehavior()
    {
        Log("Testing Monster Behavior...");
        UI.Message("Testing Monster Behavior...", clear: true);
        var monster = new ClockworkMage();
        Debug.Assert(monster.Name == "Clockwork Mage", "Monster name should be 'Clockwork Mage'.");
        Debug.Assert(monster.Health > 0, "Monster should have positive health.");

        var player = new Player("TestPlayer");
        monster.Attack(player);
        Debug.Assert(player.Health < player.MaxHealth, "Player should take damage from monster attack.");
        UI.Message("Monster behavior test passed.", wait: true);
        Log("Monster Behavior Test Passed.");
    }

    /// <summary>
    /// Tests inventory management functionality.
    /// </summary>
    private static void TestInventoryManagement()
    {
        Log("Testing Inventory Management...");
        UI.Message("Testing Inventory Management...", clear: true);
        var inventory = new Inventory();
        var weapon = new Weapon("Sword", "A sharp blade.", 10);
        var armour = new Armour("Shield", "A sturdy shield.", 5);

        inventory.AddItem(weapon);
        inventory.AddItem(armour);

        Debug.Assert(inventory.Count == 2, "Inventory should contain 2 items.");
        Debug.Assert(inventory.BestWeapon == weapon, "BestWeapon should return the weapon with the highest damage modifier.");
        Debug.Assert(inventory.BestArmour == armour, "BestArmour should return the armour with the highest defense modifier.");

        inventory.RemoveItem(weapon);
        Debug.Assert(inventory.Count == 1, "Inventory should contain 1 item after removal.");
        Debug.Assert(!inventory.HasItem(weapon), "Inventory should not contain the removed item.");
        UI.Message("Inventory management test passed.", wait: true);
        Log("Inventory Management Test Passed.");
    }
}