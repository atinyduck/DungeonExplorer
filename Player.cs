using System;
using System.Collections.Generic;
using System.Reflection;

namespace DungeonExplorer
{
    internal class Player : Creature
    {
        public int Experience { get; private set; }
        public int Level { get; private set; } = 1;

        public Player(string name) : base(name, maxHealth: 100, Defence: 10, attackPower: 15)
        {
            Inventory = new Inventory();
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            //Display increase

            while (Experience >= GetRequiredXP())
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            // Increase Stats
            Level++;
            ModfiyBaseStat(BaseStatistics.MaxHealth, MaxHealth + 10);
            ModfiyBaseStat(BaseStatistics.AttackPower, BaseAttackPower + 2);
            ModfiyBaseStat(BaseStatistics.Defence, BaseDefence + 1);
            Heal(MaxHealth);

            //Display level up
        }

        private int GetRequiredXP() => Level * 100;

        internal List<string> GetInventory() => Inventory;

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            const string Title = "\r\n=================================================" +
                "\r\n                  PLAYER STATS                  \r\n" +
                "=================================================";

            string stats = $"\r\nName: {Name} \r\nHP: {Health} \r\n\r\nInventory:\r\n{GetInventory()}\r\n";

            return Title + stats;
        }

        public override void Attack(IDamagable target)
        {
            int damage = AttackPower;
            target.TakeDamage(damage);
            //Display attack;
        }
    }
}