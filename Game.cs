using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;


namespace DungeonExplorer
{
    public class Game
    {
        private readonly Player player;
        private readonly Room CurrentRoom;
        private readonly List<Room> VisitedRooms;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
        {
            // Initialize the game with one room and one player
            this.player = new Player("NAME");
            this.CurrentRoom = new Room();
            this.VisitedRooms = new List<Room>();
        }

        /// <summary>
        /// Starts this instance of 'Game'.
        /// </summary>
        public void Start()
        {
            UI.DisplayMenu();

            // If no save is present generate a new one.
            if (!CallSave("REPLACE ME PLEASEEEE!!!!!!!!"))
            {
                UI.DisplayIntro(this.player);
            }

            // If there is CallSave() will get relevant information
            // Then carry on with the game
            this.GameLoop();
        }

        #region Game Control

        /// <summary>
        /// Runs the main game loop.
        /// </summary>
        private void GameLoop()
        {
            bool running = true;
            
            while (running)
            {
                VisitedRooms.Add(CurrentRoom);
                // Display loop
            }
        }

        #endregion

        #region Display Control

        #endregion

        #region Save Control

        public void SaveGame(string path)
        {
            // Waiting for implementation
        }

        public void DeleteSave(string path)
        {
            // Waiting for implementation
        }

        public bool CallSave(string path)
        {
            return false; // Waiting for implementation
        }

        #endregion

    }
}