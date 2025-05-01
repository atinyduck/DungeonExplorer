namespace DungeonExplorer;
/// <summary>
/// Inventory class for managing items.
/// </summary>
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Inventory : ISaveable
{
    private List<ICollectable> _items = new List<ICollectable>();
    public IReadOnlyList<ICollectable> Items => _items.AsReadOnly();
    public int Count => _items.Count;
    public Weapon BestWeapon => FindBestWeapon();
    public Armour BestArmour => FindBestArmour();

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier</returns>
    public string GetSaveIdentifier() => $"inventory";

    /// <summary>
    /// Determines whether the specified item has item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>
    ///   <c>true</c> if the specified item has item; otherwise, <c>false</c>.
    /// </returns>
    public bool HasItem(ICollectable item) => _items.Contains(item);

    /// <summary>
    /// Adds the item.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <exception cref="System.ArgumentNullException">item</exception>
    public void AddItem(ICollectable item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    /// <summary>
    /// Removes the item.
    /// </summary>
    /// <param name="item">The item.</param>
    public void RemoveItem(ICollectable item) => _items.Remove(item);

    /// <summary>
    /// Clears this instance.
    /// </summary>
    public void Clear() => _items.Clear();

    /// <summary>
    /// Lists the weapons.
    /// </summary>
    /// <returns>The list of weapons</returns>
    public List<Weapon> ListWeapons() => _items.OfType<Weapon>().ToList();

    /// <summary>
    /// Finds the best weapon.
    /// </summary>
    /// <returns>The best weapon</returns>
    private Weapon FindBestWeapon() => ListWeapons()
                                        .OrderByDescending(w => w.DamageModifier)
                                        .FirstOrDefault();

    /// <summary>
    /// Lists the armour.
    /// </summary>
    /// <returns>The list of armour</returns>
    public List<Armour> ListArmour() => _items.OfType<Armour>().ToList();

    /// <summary>
    /// Finds the best armour.
    /// </summary>
    /// <returns>The best armour</returns>
    private Armour FindBestArmour() => ListArmour()
                                        .OrderByDescending(a => a.DefenceModifier)
                                        .FirstOrDefault();

    /// <summary>
    /// Uses the item.
    /// </summary>
    /// <param name="player">The player.</param>
    public void UseItem(Player player)
    {
        var usableItems = _items.OfType<ICollectable>().ToList();

        if (usableItems.Count == 0)
        {
            UI.Message("You have no items to use.", wait: true);
            return;
        }

        // Display the list of usable items
        var itemList = usableItems.Select((item, index) =>
            $"{index + 1}. {item.Name}").ToList();
        itemList.Add($"{itemList.Count + 1}. Cancel");

        // Get user input
        List<string> validInputs = Enumerable
            .Range(1, itemList.Count)
            .Select(i => i.ToString())
            .ToList();
        string input = UI.GetInput(validInputs, $"Select item to use:\n\t{string.Join("\n\t", itemList)}");

        if (int.TryParse(input, out int index) && index <= usableItems.Count)
        {
            if (index == itemList.Count) // Cancel option
            {
                UI.Message("Cancelled.", wait: true);
                return;
            }
            // Use the selected item
            var item = usableItems[index - 1];
            item.Use(player);

            // Remove the item from inventory if it's a consumable
            if (item.Name.Contains("Potion", StringComparison.OrdinalIgnoreCase)) player.Inventory.RemoveItem(item);
            UI.Message($"You used {item.Name}.", wait: true);
        }
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
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
