using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class ClockworkMage : Monster
    {
        public ClockworkMage()
             : base("Clockwork Mage", maxHealth: ~, defence: ~, attackPower: ~, rewardXP: ~)
        { }

        public override void Attack(IDamageable target)
        {
            for (int i = 0; i < 2; i++)
            {
                int damage = AttackPower / 2;
                target.TakeDamage(damage);
                //Display attack
            }
        }
    }

    public class RustingConstruct : Monster
    {
        public RustingConstruct()
             : base("Rusting Construct", maxHealth: ~, defence: ~, attackPower: ~, rewardXP: ~)
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
    
        if (Random.Next(0, 3) == 0)
            {
                //Add armour to the drops
            }

            return drops;
        }
    }

    public class RepairUnit : Monster
    {
        public RepairUnit()
             : base("Clockwork Mage", maxHealth: ~, defence: ~, attackPower: ~, rewardXP: ~)
        { }

        public override void Attack(IDamageable target)
        {
            int damage = AttackPower;
            target.TakeDamage(damage);
            //Display attack   

            if (Random.Next(0, 4) == 0)
            {
                Heal(~);
                // Display self-heal
            }
        }
    }
}
