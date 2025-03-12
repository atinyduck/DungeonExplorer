using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DungeonExplorer
{
    internal static class Testing
    {
        const string TITLE = "\n\n===== Debug =====\n";

        #region Testing Menu

        internal static void TestMenu()
        {
            string output = TITLE + "Menu ::\n\nPlease enter an option :\n1. Player\n2. Room\n3. All";
            List<string> valid_inputs = new List<string>() { "1", "2", "3"};

            string input = GameUI.GetInput(valid_inputs, output);
            
            switch(input)
            {
                case "1":
                    RunPlayerTests();
                    break;

                case "2":
                    RunRoomTests();
                    break;

                case "3":
                    RunPlayerTests();
                    RunRoomTests();
                    break;
            }

            GameUI.WaitForInput();

        }

        #endregion

        #region Player Testing

        /// <summary>
        /// Run all player tests
        /// </summary>
        private static void RunPlayerTests()
        {
            Console.Write(TITLE + "Player :: ");
            TestPlayerInitialisation();
            TestInventoryManagement();
            TestToString();
            TestHealthManagement();
            Console.WriteLine("All player tests ran");
        }

        /// <summary>
        /// Test player initialisation
        /// </summary>
        private static void TestPlayerInitialisation()
        {
            Player test_player = new Player("Dummy", 100);

            // Test initialisation
            Debug.Assert(test_player.Name == "Dummy", 
                "Player has been named 'Dummy'");
            Debug.Assert(test_player.Health == 100, 
                "Player health has been set to '100'");
            Debug.Assert(test_player.MaxHealth == 100, 
                "Player max health should be '100'");
        }

        /// <summary>
        /// Test inventory management
        /// </summary>
        private static void TestInventoryManagement()
        {
            Player test_player = new Player("Dummy", 100);

            // Add items
            test_player.PickUpItem("Sword");
            test_player.PickUpItem("Shield");

            // Test if items are present
            Debug.Assert(test_player.GetInventory().Count == 2, 
                "Inventory should contain two items.");
            Debug.Assert(test_player.GetInventory().Contains("Sword"), 
                "Inventory should contain 'Sword'");
            Debug.Assert(test_player.GetInventory().Contains("Shield"), 
                "Inventory should contain 'Shield'");
        }

        /// <summary>
        /// Test ToString() method
        /// </summary>
        private static void TestToString()
        {
            Player test_player = new Player("Dummy", 100);

            // Add item
            test_player.PickUpItem("Sword");

            string player_stats = test_player.ToString();

            // Test if ToString contains correct values
            Debug.Assert(player_stats.Contains("Name: Dummy"), 
                "Player stats should contain 'Name: Hero'");
            Debug.Assert(player_stats.Contains("HP: 100"), 
                "Player stats should contain 'HP: 100'");
            Debug.Assert(player_stats.Contains("Inventory:\r\nSword"), 
                "Player stats should contain 'Inventory:\r\nSword'");
        }

        /// <summary>
        /// Test health management
        /// </summary>
        private static void TestHealthManagement()
        {
            Player test_player = new Player("Dummy", 100);

            // Test damage taken
            test_player.TakeDamage(20);
            Debug.Assert(test_player.Health == 80, 
                "Player health should be 80 after taking 20 damage");
            
            // Test damage taken for below 0
            test_player.TakeDamage(100);
            Debug.Assert(test_player.Health == 0,
                "Player health should be 0 after taking more damage than health remaining");

            // Test healing
            test_player.Heal(50);
            Debug.Assert(test_player.Health == 50, 
                "Player health should be 50 after healing 50");

            // Test healing for above max
            test_player.Heal(100);
            Debug.Assert(test_player.Health == 100,
                "Player health shou;d be at max of 100 after healing more than max health");
        }


        #endregion

        #region Room Testing

        /// <summary>
        /// Run all room tests
        /// </summary>
        private static void RunRoomTests()
        {
            Console.Write(TITLE + "Room :: ");
            TestRoomInitialisation();
            TestLootManagement();
            TestDescriptionGeneration();
            TestNeighbourManagement();
            Console.WriteLine("All room tests ran");
        }

        /// <summary>
        /// Run tests for Room initialisation
        /// </summary>
        private static void TestRoomInitialisation()
        {
            // New Room
            Room test_room = new Room();

            // GetDescription()
            Debug.Assert(!string.IsNullOrEmpty(test_room.GetDescription()), 
                "Description should be returned, or appropriate error and not be empty.");
            // GetLoot()
            Debug.Assert(test_room.GetLoot() != null, 
                "Loot list should not be null");
            // Neighbours is less than max neighbours
            Debug.Assert(test_room.GetNeighbours().Count <= Room.MaxNeighbours, 
                "There shoud not be more neighbours than max.");
        }

        /// <summary>
        /// Run tests for loot management
        /// </summary>
        private static void TestLootManagement()
        {
            Room test_room = new Room();

            while (test_room.GetLoot().Count == 0)
            {
                test_room = new Room();
            }

            // Remove item
            string item = test_room.GetLoot()[0];
            test_room.RemoveLoot(item); 

            // Assert if item has been removed
            Debug.Assert(!test_room.GetLoot().Contains(item), "Item should be removed from loot.");
        }

        /// <summary>
        /// Run tests for description generation
        /// </summary>
        private static void TestDescriptionGeneration()
        {
            Room test_room = new Room();

            string description = test_room.GetDescription();

            // Check if any errors are thrown during description generation
            Debug.Assert(!string.IsNullOrEmpty(description), 
                "Description should not be empty.");
            Debug.Assert(description != "File does not exist.", 
                "Description file should exist.");
            Debug.Assert(!description.StartsWith("Failed to read file:"), 
                "Description file should be read successfully.");
        }

        /// <summary>
        /// Run tests for generating neighbours
        /// </summary>
        private static void TestNeighbourManagement()
        {
            Room test_room = new Room();

            int initial_neighbour_count = test_room.GetNeighbours().Count;
            test_room.GenerateNeighbour();

            // Test if new neighbour has been generated
            Debug.Assert(test_room.GetNeighbours().Count == initial_neighbour_count + 1,
                "Neighbour count should increase by one.");

        }

        #endregion
    }
}