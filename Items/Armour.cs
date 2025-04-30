namespace DungeonExplorer;
public class Armour : Item
{
    private static List<Armour> _armours = new List<Armour>(){
        new Armour("Scrap Robes", "Assorted robes thrown together.", 1),
        new Armour("Rusted Chainmail", "Old and clearly very used armour, but it still works.", 2),
        new Armour("Brass Plating", "Sheets of brass repurposed for protection.", 3)
    };

    public static IReadOnlyList<Armour> Armours => _armours.AsReadOnly();

    public int DefenceModifier { get; private set; }

    public Armour(string name, string description, int defenceModifier)
            : base(name, description)
    {
        DefenceModifier = defenceModifier;
    }


    public override string GetSaveIdentifier() => $"armour_{Name}_{DefenceModifier}";

    public override string ToString()
    {
        // Display the armour name, description and stats.
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{Name} :: {DefenceModifier}");
        builder.AppendLine($"{Description}\n");

        return builder.ToString();
    }

    public static ICollectable GenerateItem(int seed = 0)
    {
        if (seed != 0)
        {
            return _armours[seed];
        }
        else
        {
            var random = new Random();
            List<Armour> armours = new(_armours);
            for (int i = 0; i <  armours.Count; i++)
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
        target.EquipArmour(this);
        UI.Message("Armour equppied!\n" + ToString(), wait: true);
    }
}