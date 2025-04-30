using System.Reflection.Emit;

namespace DungeonExplorer;
public class Item : ICollectable, ISaveable
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    #region Save Implementation
    public virtual string GetSaveIdentifier() =>
        $"item_{Name.GetHashCode().ToString("X")}";

    #endregion

    public static Item GenerateItem(int seed = 0)
    {
        // Generate a random item
        // This could be overridden in derived classes for specific item types
        return new Item("Generic Item", "A generic item with no special properties.");
    }

    public virtual void Use(Creature target)
    {
        // Display description as if player was looking stating it has no use.
    }
}