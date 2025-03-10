using System;
using System.Collections.Generic;
using System.Reflection;

namespace DungeonExplorer
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; private set; }
        private int MaxHealth;
        private readonly List<string> inventory = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="health">The health.</param>
        public Player(string name, int health) 
        {
            Name = name;
            Health = health;
            MaxHealth = health;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            const string Title = "\r\n=================================================\r\n                  PLAYER STATS                  \r\n=================================================";

            string Stats = $"\r\nName: {this.Name} \r\nHP: {this.Health} \r\n\r\nInventory:\r\n{this.GetInventoryContents()}\r\n";

            return Title + Stats;
        }

        /// <summary>
        /// Picks up item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void PickUpItem(string item)
        {
            inventory.Add(item);
        }

        /// <summary>
        /// Heals the specified amount up to Max Health.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void Heal(int amount)
        { 
            this.Health += amount;
            if (this.Health > this.MaxHealth)
            {
                this.Health = this.MaxHealth;
            }
        }

        /// <summary>
        /// Makes the player take damage.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void TakeDamage(int amount)
        {
            this.Health -= amount;
            if (this.Health < 0) this.Health = 0;
        }

        /// <summary>
        /// Gets the inventory contents.
        /// </summary>
        /// <returns>string: The inventory as a string.</returns>
        private string GetInventoryContents()
        {
            return string.Join(", ", inventory);
        }
    }
}