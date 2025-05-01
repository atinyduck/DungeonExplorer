namespace DungeonExplorer;
/// <summary>
/// Represents an item of type <see cref="Armour"/>.
/// </summary>
/// <seealso cref="DungeonExplorer.Item" />
public class Armour : Item
{
    private static List<Armour> _armours = new List<Armour>(){
        new Armour("Tattered Vest", "A worn and ripped vest offering minimal protection.", 1),
        new Armour("Leather Jerkin", "A simple leather vest.", 2),
        new Armour("Makeshift Pads", "Some scavenged padding strapped together.", 1),
        new Armour("Metal Chest Plate", "A heavy, rusted metal plate for the chest.", 3),
        new Armour("Reinforced Jacket", "A sturdy jacket with some added metal plates.", 2),
        new Armour("Scrap Helmet", "A helmet made from various pieces of metal.", 2),
        new Armour("Leather Boots", "Well-worn leather boots.", 1),
        new Armour("Metal Greaves", "Metal plates to protect the lower legs.", 2),
        new Armour("Gauntlets", "Sturdy gloves offering some hand protection.", 1),
        new Armour("Full Body Coveralls", "Thick, protective coveralls.", 1)
    };

    public static IReadOnlyList<Armour> Armours => _armours.AsReadOnly();

    public int DefenceModifier { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Armour"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="defenceModifier">The defence modifier.</param>
    public Armour(string name, string description, int defenceModifier)
            : base(name, description)
    {
        DefenceModifier = defenceModifier;
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The indentifier.</returns>
    public override string GetSaveIdentifier() => $"armour_{Name}_{DefenceModifier}";

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        // Display the armour name, description and stats.
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{Name} :: {DefenceModifier}");
        builder.AppendLine($"{Description}\n");

        return builder.ToString();
    }

    /// <summary>
    /// Generates the item.
    /// </summary>
    /// <param name="seed">The seed.</param>
    /// <returns>The item.</returns>
    public static ICollectable GenerateItem(int seed = 0)
    {
        if (seed != 0) // If seed is provided, use it to select the item.
        {
            return _armours[seed];
        }
        else // Otherwise, select a random item.
        {
            var random = new Random();
            return _armours[random.Next(0, _armours.Count)];
        }
    }

    /// <summary>
    /// Uses the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Use(Creature target)
    {
        target.EquipArmour(this);
        UI.Message("Armour equppied!\n" + ToString(), wait: true);
    }
}