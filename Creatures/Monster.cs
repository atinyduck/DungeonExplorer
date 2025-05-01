namespace DungeonExplorer;
/// <summary>
/// Base class for all monsters.
/// </summary>
/// <seealso cref="DungeonExplorer.Creature" />
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Monster : Creature, ISaveable
{
    public int RewardXP { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Monster"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="maxHealth">The maximum health.</param>
    /// <param name="defence">The defence.</param>
    /// <param name="attackPower">The attack power.</param>
    /// <param name="rewardXP">The reward xp.</param>
    protected Monster(string name, int maxHealth, int defence, int attackPower, int rewardXP)
        : base(name, maxHealth, defence, attackPower)
    {
        RewardXP = rewardXP;
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier string.</returns>
    public string GetSaveIdentifier() => $"monster_{GetType().Name}_{Health}";

    /// <summary>
    /// Generates the drops.
    /// </summary>
    /// <returns>The drops.</returns>
    public virtual List<ICollectable> GenerateDrops()
    {
        var drops = new List<ICollectable>();
        var random = new Random();
        if (random.Next(0, 2) == 0)
        {
            drops.Add(new Potion("Health Potion", "A potion that restores health.", PotionEffect.Heal, -1, 20));
        }
        return drops;
    }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        if (EquippedWeapon != null)
        {
            damage += EquippedWeapon.DamageModifier;
        }
        target.TakeDamage(damage);
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"A {Name} stands before you!");
        builder.AppendLine($"Health: {Health}/{MaxHealth}");

        return builder.ToString();
    }
}