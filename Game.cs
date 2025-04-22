using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
using System.Runtime.Remoting;


namespace DungeonExplorer
{
    internal class Game
    {
        private readonly Player player;
        private readonly Room CurrentRoom;
        private readonly List<Room> VisitedRooms;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        internal Game()
        {
            // Initialize the game with one room and one player
            this.player = new Player("NAME");
            this.CurrentRoom = new Room();
            this.VisitedRooms = new List<Room>();
        }

        /// <summary>
        /// Starts this instance of 'Game'.
        /// </summary>
        internal void Start()
        {
            GameUI.DisplayMenu();

            // If no save is present generate a new one.
            if (!CallSave("REPLACE ME PLEASEEEE!!!!!!!!"))
            {
                GameUI.DisplayIntro(this.player);
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

                GameUI.DisplayRoom(current_room, player);
            }
        }

        #endregion

        #region Display Control

        #endregion

        #region Save Control

        internal void SaveGame(string path)
        {
            // Waiting for implementation
        }

        internal void DeleteSave(string path)
        {
            // Waiting for implementation
        }

        internal bool CallSave(string path)
        {
            return false; // Waiting for implementation
        }

        #endregion

    }
}