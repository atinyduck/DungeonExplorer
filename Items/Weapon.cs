namespace DungeonExplorer;

/// <summary>
/// Represents an item of type <see cref="Weapon"/>.
/// </summary>
/// <seealso cref="DungeonExplorer.Item" />
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Weapon : Item, ISaveable
{
    public int DamageModifier { get; private set; }

    private static List<Weapon> _weapons = new List<Weapon>()
    {
        new Weapon("Rusted Pipe", "A broken rust pipe.", 2),
        new Weapon("Corroded Spear", "A tall spear with a rusted spear head.", 4),
        new Weapon("Sword", "A somehow well preserved sword.", 6)
    };  

    public static IReadOnlyList<Weapon> Weapons => _weapons.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of the <see cref="Weapon"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="damageModifier">The damage modifier.</param>
    public Weapon(string name, string description, int damageModifier)
            : base(name, description)
    {
        DamageModifier = damageModifier;
    }

    public override string GetSaveIdentifier() => $"weapon_{Name}_{DamageModifier}";

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        // Display the weapon name, description and stats.
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{Name} :: {DamageModifier}");
        builder.AppendLine($"{Description}\n");

        return builder.ToString();
    }

    /// <summary>
    /// Generates the item.
    /// </summary>
    /// <param name="seed">The seed.</param>
    /// <returns></returns>
    public static ICollectable GenerateItem(int seed = 0)
    {
        if (seed != 0) // If seed is provided, use it to select the item.
        {
            return _weapons[seed];
        }
        else // Otherwise, select a random item.
        {
            var random = new Random();
            return _weapons[random.Next(0, _weapons.Count)];
        }
    }

    /// <summary>
    /// Uses the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Use(Creature target)
    {
        target.EquipWeapon(this);
    }
}