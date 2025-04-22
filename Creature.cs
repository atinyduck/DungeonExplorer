using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public abstract class Creature : IDamageable
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public Inventory Inventory { get; private set; }
        public int BaseDefence { get; private set; }
        public int BaseAttackPower { get; private set; }
        public int Defence { get; private set; }
        public int AttackPower { get; private set; }
        public Weapon EquippedWeapon { get; private set; }
        public Armour EquippedArmour { get; private set; }

        protected Creature(string name, int maxHealth, int defence, int attackPower)
        {
            Name = name;
            Health = maxHealth;
            MaxHealth = maxHealth;
            Inventory = new Inventory();
            BaseDefence = defence;
            BaseAttackPower = attackPower;
            Defence = defence;
            AttackPower = attackPower;
        }
        
        public abstract void Attack(IDamageable target);

        public void TakeDamage(int amount)
        {
            int actualDamage = Math.Max(0, amount - Defence);
            Health = Math.Max(0, Health - actualDamage);
            // Display Damage
    }

        public void Heal(int amount)
        {
            Health = Math.Min(MaxHealth, Health + amount);
            // Display Heal
        }

        public void EquipArmour(Armour armour)
        {
            EquippedArmour = armour;
            RecalculateStats();
        }

        public void EquipWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
            RecalculateStats();
        }

        public void ModifyBaseStat(BaseStatistic stat, int newValue)
        {
            switch (stat)
            {
                case BaseStatistic.Defence:
                    BaseDefence = newValue;
                    break;
                case BaseStatistic.AttackPower:
                    BaseAttackPower = newValue;
                    break;
                case BaseStatistic.MaxHealth:
                    MaxHealth = newValue;
                    // If the max is reduced, ensure that health does not exceed it.
                    Health = Math.Min(Health, MaxHealth);
                    break;
            }
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            AttackPower = BaseAttackPower + EquippedWeapon?.DamageModifier;
            Defence = BaseDefence + EquippedArmour?.DefenceModifier;
        }

        public override string ToString()
        {
            // Display Creature
            return "";
        }
    }
}