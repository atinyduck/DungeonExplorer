using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Armour : Item
    {
        public int DefenceModifier { get; private set; }

        public Armour(string name, string description, int defenseModifier)
             : base(name, description)
        {
            DefenceModifier = defenseModifier;
        }

        public override string ToString()
        {
            // Display the armour name, description and stats.
            return "TEMPORARY";
        }

        public override void Use(Creature target)
        {
            target.EquipArmour(this);
            //Display equipped armour
        }
    }
}