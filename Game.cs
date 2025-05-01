
namespace DungeonExplorer;
/// <summary>
/// Gmae class that handles the main game loop and player interactions.
/// </summary>
public class Game
{
    /// <summary>
    /// Private enum for player actions
    /// </summary>
    private enum PlayerAction
    {
        Move,
        Loot,
        Inventory,
        UseItem,
        Quit
    }

    private Player player;
    private GameMap gameMap;
    private bool isRunning;
    private int currentSaveSlot;

    /// <summary>
    /// Initializes a new instance of the Game class
    /// </summary>
    public Game()
    {
        // Initialization handled in Start()
    }

    /// <summary>
    /// Starts the main game loop
    /// </summary>
    public void Start()
    {
        DisplayMenu();
    }

    #region Menu Systems

    /// <summary>
    /// Displays the main menu and handles player selection
    /// </summary>
    private void DisplayMenu()
    {
        while (true)
        {
            var options = new List<string> { "1", "2", "3", "Q", "D" };
            string input = UI.GetInput(options, UI.Title + UI.MainMenuText, clear: true).ToUpper();

            switch (input)
            {
                case "1": // Enter the ruins
                    HandleNewOrLoadGame();
                    break;

                case "2": // How to play
                    DisplayHowToPlay();
                    break;

                case "3": // Delete save
                    DeleteSaveGame();
                    break;

                case "Q": // Quit
                    if (UI.Confirm())
                        Environment.Exit(0);
                    break;

                case "D": // Debug
                    //Testing.TestMenu();
                    break;
            }
        }
    }

    /// <summary>
    /// Handles the save slot selection and game initialization
    /// </summary>
    private void HandleNewOrLoadGame()
    {
        currentSaveSlot = SaveManager.GetSaveSlot();
        if (currentSaveSlot == 0) return; // Player went back

        if (SaveManager.SaveExists(currentSaveSlot))
        {
            if (UI.Confirm("Load this saved game?"))
            {
                (player, gameMap) = SaveManager.LoadGame(currentSaveSlot);
                StartGameLoop();
            }
        }
        else
        {
            if (UI.Confirm("Start new game in this slot?"))
            {
                InitialiseNewGame();
                StartGameLoop();
            }
        }
    }

    /// <summary>
    /// Initialises a new game state
    /// </summary>
    private void InitialiseNewGame()
    {
        player = new Player(GetPlayerName());
        var startRoom = new Room();
        gameMap = new GameMap(startRoom);

        DisplayIntro();
    }

    #endregion

    #region Core Gameplay

    /// <summary>
    /// Starts the main game loop
    /// </summary>
    private void StartGameLoop()
    {
        isRunning = true;
        DateTime lastSaveTime = DateTime.Now;

        // Main game loop
        while (isRunning)
        {
            int autSaveInterval = 3; // Auto-save interval in minutes

            // Auto-save every x minutes
            if ((DateTime.Now - lastSaveTime).TotalMinutes >= autSaveInterval)
            {
                SaveManager.SaveGame(currentSaveSlot, player, gameMap);
                lastSaveTime = DateTime.Now;
                UI.Message("Game progress saved", wait: false);
            }

            // Handle user interactions
            DisplayCurrentRoom();
            HandlePlayerAction();
            CheckGameState();
        }
    }

    /// <summary>
    /// Displays the current room information
    /// </summary>
    private void DisplayCurrentRoom()
    {
        if (gameMap.CurrentRoom == null)
        {
            UI.Message("You are lost in the void...", clear: true);
            return;
        }

        if (gameMap.CurrentRoom.HasMonsters())
        {
            var monsters = gameMap.CurrentRoom.GetMonsters();

            // Display monster names
            string monsterNames = string.Join("\n\t", monsters.Select(m => m.Name));
            UI.Message($"You encounter\n\t{monsterNames}", wait: true);

            while (monsters.Any(m => m.Health > 0))
            {
                HandleCombat();
            }
        }

        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"{UI.Title}{player.Name}, lvl :: {player.Level}");
        builder.AppendLine($"HP :: {player.Health}/{player.MaxHealth} | XP :: {player.Experience}/{player.GetRequiredXP()}\n");
        builder.AppendLine(gameMap.CurrentRoom.ToString());

        UI.Message(builder.ToString(), clear: true, wait: false);
    }

    /// <summary>
    /// Gets and processes player input
    /// </summary>
    private void HandlePlayerAction()
    {
        var action = GetPlayerAction();

        switch (action) // Handle player action
        {
            case PlayerAction.Move:
                HandleMovement();
                break;

            case PlayerAction.Loot:
                HandleLootRoom();
                break;

            case PlayerAction.Inventory:
                DisplayInventory();
                break;

            case PlayerAction.UseItem:
                UseItem();
                break;

            case PlayerAction.Quit:
                if (UI.Confirm("Are you sure you want to quit?"))
                {
                    SaveManager.SaveGame(currentSaveSlot, player, gameMap);
                    UI.Message("Game progress saved", wait: false);
                    isRunning = false;
                }
                break;
        }
    }

    #endregion

    #region Game Actions

    /// <summary>
    /// Handles player movement between rooms
    /// </summary>
    private void HandleMovement()
    {
        // Get available directions from the current room
        var directions = Enum.GetValues(typeof(Direction)).Cast<Direction>();
        var directionOptions = directions.ToDictionary(
            d => d.ToString().Substring(0, 1),
            d => d);

        string input = UI.GetInput(directionOptions.Keys.ToList(),
            $"Choose direction: {string.Join(" ", directionOptions.Keys)}");

        if (gameMap.Move(directionOptions[input.ToUpper()]))
        {
            player.Statistics.UpdateRoomsVisited();
            DiscoverRoom();
        }
        else // Invalid move
        {
            UI.Message("You can't go that way!", wait: true);
        }
    }

    /// <summary>
    /// Handles looting the room.
    /// </summary>
    private void HandleLootRoom()
    {
        if (gameMap.CurrentRoom.HasLoot())
        {
            var item = gameMap.CurrentRoom.LootRoom();
            if (item != null)
            {
                player.Inventory.AddItem(item);
                UI.Message($"You found: {item.Name}!", wait: true);
                player.Statistics.UpdateItemsCollected();
            }
        }
        else
        {
            UI.Message("There is no loot in this room.", wait: true);
        }
    }

    /// <summary>
    /// Handles room discovery when entering a new room
    /// </summary>
    private void DiscoverRoom()
    {
        var room = gameMap.CurrentRoom;

        if (room.HasMonsters())
        {
            UI.Message("Monsters await you ahead!", ConsoleColor.Red, wait: true);
        }
    }

    /// <summary>
    /// Displays the player's inventory
    /// </summary>
    private void DisplayInventory()
    {
        if (player.Inventory.Count == 0)
        {
            UI.Message("Your inventory is empty.", wait: true);
            return;
        }

        string inventoryText = player.Inventory.ToString();
        UI.Message(inventoryText, wait: true);
    }

    /// <summary>
    /// Handles item usage from inventory
    /// </summary>
    private void UseItem()
    {
        var usableItems = player.Inventory.Items.Where(i => i is ICollectable).ToList();

        if (usableItems.Count == 0) // No usable items
        {
            UI.Message("No usable items in inventory.", wait: true);
            return;
        }

        // Display usable items
        var itemList = usableItems.Select((item, i) => $"{i + 1}. {item.Name}").ToList();
        itemList.Add($"{itemList.Count + 1}. Cancel");

        string input = UI.GetInput(
            Enumerable.Range(1, itemList.Count).Select(i => i.ToString()).ToList(),
            "Select item to use:\n" + string.Join("\n", itemList));

        if (int.TryParse(input, out int index) && index <= usableItems.Count)
        {
            if (index == itemList.Count) // Cancel
            {
                UI.Message("Cancelled item usage.", wait: true);
                return;
            }
            var item = usableItems[index - 1];
            item.Use(player);
            player.Inventory.RemoveItem(item);
            UI.Message($"Used {item.Name}.", wait: true);
        }
    }
    #region Combat System

    /// <summary>
    /// Handles the complete combat encounter
    /// </summary>
    private void HandleCombat()
    {
        var monsters = gameMap.CurrentRoom.GetMonsters();
        var currentMonster = monsters[0]; // Focus on one monster at a time

        // Combat introduction
        UI.Message($"\nA {currentMonster.Name} approaches!", ConsoleColor.Red, true);

        DisplayCombatStats(currentMonster);

        while (currentMonster.Health > 0 && player.Health > 0)
        {
            var action = GetCombatAction();

            switch (action) // Handle combat action
            {
                case CombatAction.Attack:
                    player.Attack(currentMonster);
                    break;

                case CombatAction.UseItem:
                    UseItem();
                    break;

                case CombatAction.Flee:
                    if (AttemptFlee(currentMonster))
                    {
                        return; // Exit combat
                    }
                    break;
            }

            // Monster gets a turn if still alive
            if (currentMonster.Health > 0)
            {
                currentMonster.Attack(player);
            }

            DisplayCombatStats(currentMonster);
        }

        // Handle combat conclusion
        if (currentMonster.Health <= 0)
        {
            ConcludeCombat(currentMonster);
        }
    }

    /// <summary>
    /// Displays current combat status for player and monster
    /// </summary>
    private void DisplayCombatStats(Monster monster)
    {
        var sb = new StringBuilder();
        sb.AppendLine("\n=== Combat Status ===");
        sb.AppendLine($"You: {player.Health}/{player.MaxHealth} HP | ATK: {player.AttackPower} | DEF: {player.Defence}");
        sb.AppendLine($"{monster.Name}: {monster.Health}/{monster.MaxHealth} HP | ATK: {monster.AttackPower} | DEF: {monster.Defence}");

        if (player.ActiveEffects.Count != 0) // Display active effects
        {
            sb.AppendLine("\nActive Effects:");
            foreach (var effect in player.ActiveEffects)
            {
                sb.AppendLine($"- {effect.ToString()}");
            }
        }

        UI.Message(sb.ToString(), ConsoleColor.Yellow, false);
    }

    /// <summary>
    /// Attempts to flee from combat with a chance of failure
    /// </summary>
    private bool AttemptFlee(Monster monster)
    {
        // Base 70% chance to flee, modified by player speed
        double fleeChance = 0.7 - (player.Inventory.Items.Count * 0.001);
        if (new Random().NextDouble() <= fleeChance)
        {
            // Move back to previous room
            if (gameMap.Move(Direction.South))
            {
                UI.Message("You successfully fled from combat!", ConsoleColor.Cyan, wait: true);
                return true;
            }
        }

        UI.Message("You failed to escape!", ConsoleColor.Red, wait: true);
        return false;
    }

    /// <summary>
    /// Handles post-combat rewards
    /// </summary>
    private void ConcludeCombat(Monster monster)
    {
        // Update player stats
        gameMap.CurrentRoom.RemoveMonsters();
        player.Statistics.UpdateMonstersKilled();
        player.GainExperience(monster.RewardXP);

        UI.Message($"\nYou gained {monster.RewardXP} experience!", ConsoleColor.Green, true);

        // Handle loot drops
        var drops = monster.GenerateDrops();
        if (drops.Count != 0)
        {
            UI.Message("\nThe monster dropped:", ConsoleColor.Yellow, true);
            foreach (var item in drops)
            {
                player.Inventory.AddItem(item);
                UI.Message($"- {item.Name}", ConsoleColor.Yellow, false);
            }
            player.Statistics.UpdateItemsCollected(drops.Count);
        }
    }

    #endregion

    #region Helper Classes

    /// <summary>
    /// Enum for possible combat actions
    /// </summary>
    private enum CombatAction
    {
        Attack,
        UseItem,
        Flee
    }

    #endregion

    #endregion

    #region Game State Management

    /// <summary>
    /// Checks win/lose conditions
    /// </summary>
    private void CheckGameState()
    {
        if (player.Health <= 0)
        {
            GameOver(false);
            return;
        }

        if (gameMap.CurrentRoom.Type == RoomType.Boss &&
            !gameMap.CurrentRoom.HasMonsters())
        {
            GameOver(true);
        }
    }

    /// <summary>
    /// Handles game over state
    /// </summary>
    /// <param name="victory">True if player won</param>
    private void GameOver(bool victory)
    {
        UI.Message(victory ? "Victory! You've cleared the dungeon!" :
            "Defeat! You have been overcome...", clear: true);

        UI.Message(player.Statistics.ToString(), wait: true);
        isRunning = false;

        if (UI.Confirm("Play again?"))
        {
            Start();
        }
        else
        {
            Environment.Exit(0);
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the player's chosen action from input
    /// </summary>
    private PlayerAction GetPlayerAction()
    {
        var options = new Dictionary<string, PlayerAction>
        {
            ["M"] = PlayerAction.Move,
            ["L"] = PlayerAction.Loot,
            ["I"] = PlayerAction.Inventory,
            ["U"] = PlayerAction.UseItem,
            ["Q"] = PlayerAction.Quit
        };

        string input = UI.GetInput(options.Keys.ToList(),
            $"Choose action:\n" +
            " M - Move\n" +
            " L - Loot\n" +
            " I - Inventory\n" +
            " U - Use\n" +
            " Q - Quit");

        return options[input.ToUpper()];
    }

    /// <summary>
    /// Gets the player's combat action choice
    /// </summary>
    private CombatAction GetCombatAction()
    {
        var options = new Dictionary<string, CombatAction>
        {
            ["A"] = CombatAction.Attack,
            ["U"] = CombatAction.UseItem,
            ["F"] = CombatAction.Flee
        };

        string input = UI.GetInput(options.Keys.ToList(),
            "Choose action:\n" +
            "A - Attack\n" +
            "U - Use Item\n" +
            "F - Attempt to Flee");

        return options[input.ToUpper()];
    }

    /// <summary>
    /// Gets the player's name via input
    /// </summary>
    private string GetPlayerName()
    {
        string name;
        do
        {
            UI.Message($"{UI.Title}{UI.Intro} ", clear: true);
            name = Console.ReadLine()?.Trim();
        } while (string.IsNullOrWhiteSpace(name));

        return name;
    }

    /// <summary>
    /// Displays the game introduction
    /// </summary>
    private void DisplayIntro()
    {
        string introText = $"{UI.Title}Welcome {player.Name} to the Iron Maw!\n\r{UI.WelcomeText}";
        UI.Message(introText, wait: true, clear: true);
    }

    /// <summary>
    /// Displays game instructions
    /// </summary>
    private void DisplayHowToPlay()
    {
        string instructions = $"{UI.Title}=== How to Play ===\n\n" +
            "Navigate using N/E/S/W commands\n" +
            "Collect items and defeat monsters\n" +
            "Use [I]nventory and [U]se commands\n" +
            "Defeat the final boss to win!";
        UI.Message(instructions, wait: true, clear: true);
    }

    /// <summary>
    /// Handles save game deletion
    /// </summary>
    private void DeleteSaveGame()
    {
        // Prompt for save slot
        int slotNumber = SaveManager.GetSaveSlot();
        if (slotNumber != null && SaveManager.SaveExists(slotNumber))
        {
            if (UI.Confirm($"Delete save in slot {slotNumber}?"))
            {
                SaveManager.DeleteSave(slotNumber);
                UI.Message("Save deleted.", wait: true);
            }
        }
    }

    #endregion
}