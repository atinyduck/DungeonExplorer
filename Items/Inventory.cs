using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Inventory
    {
        private List<ICollectable> _items = new List<ICollectable>();

        public IReadOnlyList<ICollectable> Items => _items.AsReadOnly();

        public int Count => _items.Count;

        public Weapon BestWeapon => FindBestWeapon();

        public Armour BestArmour => FindBestArmour();

        public bool HasItem(ICollectable item) => _items.Contains(item);

        public void AddItem(ICollectable item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void RemoveItem(ICollectable item) => _items.Remove(item);

        public void Clear() => _items.Clear();

        public List<Weapon> ListWeapons() => _items.OfType<Weapon>().ToList();

        private Weapon FindBestWeapon() =>
            ListWeapons()
                .OrderByDescending(w => w.DamageModifier)
                .FirstOrDefault();


        public List<Armour> ListArmour() => _items.OfType<Armour>().ToList();

        private Armour FindBestArmour() =>
            ListArmour()
                .OrderByDescending(a => a.DefenceModifier)
                .FirstOrDefault();
    }
}
