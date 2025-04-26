using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public interface ICollectable
    {
        string Name { get; }
        string Description { get; }
        void Use(Creature target);
    }
}
