using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Weapon : Item
    {
        public int DamageModifier { get; private set; }

        public Weapon(string name, string description, int damageModifier)
             : base(name, description)
        {
            DamageModifier = damageModifier;
        } 

        public override string ToString()
        {
            // Display the weapon name, description and stats.
            return "TEMPORARY";
        }

        public override void Use(Creature target)
        {
            target.EquipWeapon(this);
            //Display equipped weapon
        }
    }
}