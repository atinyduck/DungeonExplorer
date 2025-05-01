namespace DungeonExplorer;

/// <summary>
/// Base class for all items.
/// </summary>
/// <seealso cref="DungeonExplorer.ICollectable" />
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Item : ICollectable, ISaveable
{
    private static List<Item> _items = new List<Item>()
    {
        new Item("Tarnished Cog", "A small, tarnished cog that might have once been part of a larger mechanism."),
        new Item("Frayed Wiring", "A short piece of electrical wire with its insulation coming apart."),
        new Item("Weathered Scrap", "An unidentifiable piece of weathered metal or plastic."),
        new Item("Cracked Lens", "A circular piece of glass with a noticeable crack across it."),
        new Item("Broken Valve", "A small valve that appears to be stuck in the closed position."),
        new Item("Rusted Spring", "A metal spring showing significant signs of rust."),
        new Item("Torn Page", "A single page torn from a book or manual, the text is faded."),
        new Item("Dented Can", "A small metal can with a noticeable dent on its side."),
        new Item("Worn Seal", "A rubber or leather seal that looks brittle and worn."),
        new Item("Bent Pin", "A small metal pin that has been bent out of shape.")
    };

    public static IReadOnlyList<Item> Items => _items.AsReadOnly();

    public string Name { get; private set; }

    public string Description { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Item"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier</returns>
    public virtual string GetSaveIdentifier() =>
        $"item_{Name.GetHashCode().ToString("X")}";

    /// <summary>
    /// Generates the item.
    /// </summary>
    /// <param name="seed">The seed.</param>
    /// <returns>The item.</returns>
    public static ICollectable GenerateItem(int seed = 0)
    {
        // If seed is provided, use it to select the item.
        if (seed > 0 && seed < _items.Count)
        {
            return _items[seed];
        }
        // Otherwise, select a random item.
        else
        {
            var random = new Random();
            return _items[random.Next(0, _items.Count)];
        }
    }

    /// <summary>
    /// Uses the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public virtual void Use(Creature target)
    {
        // Display description as if player was looking stating it has no use.
        UI.Message($"A {Name}, {Description}\nNot of much use to you.");
    }
}