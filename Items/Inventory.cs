using System.Reflection.Emit;
using System.Xml.Linq;

namespace DungeonExplorer;
public class Inventory : ISaveable
{
    private List<ICollectable> _items = new List<ICollectable>();

    public IReadOnlyList<ICollectable> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public Weapon BestWeapon => FindBestWeapon();
    public Armour BestArmour => FindBestArmour();

    #region Save Implementation
    public string GetSaveIdentifier() => $"inventory";

    #endregion

    public bool HasItem(ICollectable item) => _items.Contains(item);
    public void AddItem(ICollectable item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public void RemoveItem(ICollectable item) => _items.Remove(item);

    public void Clear() => _items.Clear();

    public List<Weapon> ListWeapons() => _items.OfType<Weapon>().ToList();

    private Weapon FindBestWeapon() => ListWeapons()
                                        .OrderByDescending(w => w.DamageModifier)
                                        .FirstOrDefault();

    public List<Armour> ListArmour() => _items.OfType<Armour>().ToList();

    private Armour FindBestArmour() => ListArmour()
                                        .OrderByDescending(a => a.DefenceModifier)
                                        .FirstOrDefault();

    public void UseItem(Player player)
    {
        var usableItems = _items.OfType<ICollectable>().ToList();

        if (usableItems.Count == 0)
        {
            UI.Message("You have no items to use.", wait: true);
            return;
        }

        var itemList = usableItems.Select((item, index) =>
            $"{index + 1}. {item.Name}").ToList();
        itemList.Add($"{itemList.Count + 1}. Cancel");

        List<string> inputs = Enumerable
            .Range(1, itemList.Count)
            .Select(i => i.ToString())
            .ToList();

        string input = UI.GetInput(inputs, $"Select item to use:\n\t{string.Join("\n\t", itemList)}");

        if (int.TryParse(input, out int index) && index <= usableItems.Count)
        {
            var item = usableItems[index - 1];
            item.Use(player);
            player.Inventory.RemoveItem(item);
            UI.Message($"You used {item.Name}.", wait: true);
        }
    }

    public override string ToString()
    {
        var inventory = this;
        if (inventory.Items.Count == 0)
            return "Your inventory is empty.";
        else
        {
            var items = string.Join(", ", inventory.Items.Select(i => i.Name));
            return $"You have the following items: {items}.";
        }
    }
}
