using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class ClockworkMage : Monster
    {
        const int AttackNum = 2;
        public ClockworkMage()
             : base("Clockwork Mage", maxHealth: 60, defence: 3, attackPower: 6, rewardXP: 25)
        { }

        public override void Attack(IDamageable target)
        {
            for (int i = AttackNum; i < 2; i++)
            {
                int damage = Convert.ToInt16(AttackPower / AttackNum);
                target.TakeDamage(damage);
                //Display attack
            }
        }
    }

    public class RustingConstruct : Monster
    {
        private const int LootChance = 3;

        public RustingConstruct()
             : base("Rusting Construct", maxHealth: 100, defence: 6, attackPower: 8, rewardXP: 32)
        { }

        public override void Attack(IDamageable target)
        {
            int damage = AttackPower;
            target.TakeDamage(damage);
            //Display attack        
        }

        public override List<Item> GenerateDrops()
        {
            var drops = new List<Item>();
            Random random = new Random();
            if (random.Next(0, LootChance) == 0)
                {
                    //Add armour to the drops
                }

            return drops;
        }
    }

    public class RepairUnit : Monster
    {
        private const int RepairChance = 4;
        private const float RepairPercent = 0.1f;

        public RepairUnit()
             : base("Clockwork Mage", maxHealth: 50, defence: 4, attackPower: 4, rewardXP: 22)
        { }

        public override void Attack(IDamageable target)
        {
            int damage = AttackPower;
            target.TakeDamage(damage);
            //Display attack   

            
            Random random = new Random();
            if (random.Next(0, RepairChance) == 0)
            {
                int healthIncrease = Convert.ToInt16(MaxHealth * RepairPercent);
                Heal(healthIncrease);
                // Display self-heal
            }
        }
    }
}
