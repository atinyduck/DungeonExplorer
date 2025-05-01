namespace DungeonExplorer;
/// <summary>
/// Monster class representing a Clockwork Mage.
/// </summary>
/// <seealso cref="DungeonExplorer.Monster" />
public class ClockworkMage : Monster
{
    const int AttackNum = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClockworkMage"/> class.
    /// </summary>
    public ClockworkMage()
            : base("Clockwork Mage", maxHealth: 30, defence: 3, attackPower: 4, rewardXP: 25)
    { }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Attack(IDamageable target)
    {
        // Basic Multi-Attack
        for (int i = 1; i <= AttackNum; i++)
        {
            int damage = Convert.ToInt16(AttackPower);
            target.TakeDamage(damage);
            
            UI.Message($"The {Name} attacks you with a bolt of energy!", wait: true);
            UI.Message($"Dealing {damage} to you.");
        }
    }
}

/// <summary>
/// Monster class representing a Rusting Construct.
/// </summary>
/// <seealso cref="DungeonExplorer.Monster" />
public class RustingConstruct : Monster
{
    private const int LootChance = 3;

    /// <summary>
    /// Initializes a new instance of the <see cref="RustingConstruct"/> class.
    /// </summary>
    public RustingConstruct()
            : base("Rusting Construct", maxHealth: 75, defence: 6, attackPower: 8, rewardXP: 32)
    { }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
        
        UI.Message($"The {Name} attacks you with its claws!", wait: true);
        UI.Message($"Dealing {damage} to you.");
    }

    /// <summary>
    /// Generates the drops.
    /// </summary>
    /// <returns>The drops.</returns>
    public override List<ICollectable> GenerateDrops()
    {
        var drops = new List<ICollectable>();
        var random = new Random();
        if (random.Next(0, LootChance) == 0)
        {
            drops.Add(Armour.GenerateItem());
        }

        return drops;
    }
}

/// <summary>
/// Monster class representing a Repair Unit.
/// </summary>
/// <seealso cref="DungeonExplorer.Monster" />
public class RepairUnit : Monster
{
    private const int RepairChance = 4;
    private const float RepairPercent = 0.1f;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepairUnit"/> class.
    /// </summary>
    public RepairUnit()
            : base("Repair Unit", maxHealth: 40, defence: 4, attackPower: 4, rewardXP: 22)
    { }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
       
        UI.Message($"The {Name} attacks you with a wrench!", wait: true);
        UI.Message($"Dealing {damage} to you.");


        var random = new Random();
        if (random.Next(0, RepairChance) == 0)
        {
            int healthIncrease = Convert.ToInt16(MaxHealth * RepairPercent);
            Heal(healthIncrease);

            UI.Message($"The {Name} repairs itself for {healthIncrease} health!");
        }
    }
}