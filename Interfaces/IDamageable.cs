using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public interface IDamageable
    {
        int Health { get; }
        int MaxHealth { get; }
        void TakeDamage(int damage);
        void Heal(int amount);
    }
}
