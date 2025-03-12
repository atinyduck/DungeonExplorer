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
        private readonly Room current_room;
        private readonly List<Room> visited_rooms = new List<Room>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        internal Game()
        {
            // Initialize the game with one room and one player
            this.player = new Player("NAME", 100);
            this.current_room = new Room();
        }

        /// <summary>
        /// Starts this instance.
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


        private void GameLoop()
        {
            bool running = true;
            
            while (running)
            {
                visited_rooms.Add(current_room);

                GameUI.DisplayRoom(current_room, player);
            }
        }

        #endregion

        #region Display Control

        #endregion

        #region Save Control

        internal void SaveGame(string path)
        {

        }

        internal void DeleteSave(string path)
        {

        }

        internal bool CallSave(string path)
        {
            return false;
        }

        #endregion

    }
}