using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Monster : Creature
    {
        public int RewardXP { get; private set; }

        protected Monster(string name, int maxHealth, int defence, int attackPower, int rewardXP)
            : base(name, maxHealth, defence, attackPower)
        {
            RewardXP = rewardXP;
        }

        public virtual List<Item> GenerateDrops()
        {
            var drops = new List<Item>();

            if (Random.Next(0, 2) == 0)
            {
                // Add item here (Health potion)
            }
            return drops;
        }

        public override void Attack(IDamageable target)
        {
            int damage = AttackPower;
            if (EquippedWeapon != null)
            {
                damage += EquippedWeapon.AttackPower;
            }
            target.TakeDamage(damage);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
