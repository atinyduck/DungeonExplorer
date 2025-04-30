namespace DungeonExplorer;
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

    public Weapon(string name, string description, int damageModifier)
            : base(name, description)
    {
        DamageModifier = damageModifier;
    }

    #region Save Implementation
    public override string GetSaveIdentifier() => $"weapon_{Name}_{DamageModifier}";

    #endregion

    public override string ToString()
    {
        // Display the weapon name, description and stats.
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{Name} :: {DamageModifier}");
        builder.AppendLine($"{Description}\n");

        return builder.ToString();
    }
    
    public static ICollectable GenerateItem(int seed = 0)
    {
        if (seed != 0)
        {
            return _weapons[seed];
        }
        else
        {
            var random = new Random();
            List<Weapon> armours = new(_weapons);
            for (int i = 0; i < armours.Count; i++)
            {
                int j = random.Next(armours.Count);
                while (j == i) { random.Next(armours.Count); }
                (armours[i], armours[j]) = (armours[j], armours[i]);
            }

            return armours[0];
        }
    }

    public override void Use(Creature target)
    {
        target.EquipWeapon(this);
        //Display equipped weapon
    }
}