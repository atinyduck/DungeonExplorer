namespace DungeonExplorer;
/// <summary>
/// Base class for the player character.
/// </summary>
/// <seealso cref="DungeonExplorer.Creature" />
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Player : Creature, ISaveable
{
    public int Experience { get; private set; }
    public int Level { get; private set; }
    public Statistics Statistics { get; private set; } = new Statistics();

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="level">The level.</param>
    /// <param name="experience">The experience.</param>
    /// <param name="maxHealth">The maximum health.</param>
    /// <param name="defence">The defence.</param>
    /// <param name="attackPower">The attack power.</param>
    public Player(string name, int level = 1, int experience = 0, int maxHealth = 100, int defence = 3, int attackPower = 10) 
        : base(name, maxHealth: maxHealth, defence: defence, attackPower: attackPower)
    { 
        Experience = experience;
        Level = level;
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    public string GetSaveIdentifier() => $"player_{Name}";

    /// <summary>
    /// Gains experience for the player.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void GainExperience(int amount)
    {
        Experience += amount;
        UI.Message($"You gained {amount} experience!", ConsoleColor.Green, true);

        while (Experience >= GetRequiredXP())
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Levels up.
    /// </summary>
    private void LevelUp()
    {
        // Increase Stats
        Level++;
        ModifyBaseStat(BaseStatistic.MaxHealth, MaxHealth + 10);
        ModifyBaseStat(BaseStatistic.AttackPower, BaseAttackPower + 2);
        ModifyBaseStat(BaseStatistic.Defence, BaseDefence + 1);
        Heal(MaxHealth);

        //Display level up
        UI.Message($"You leveled up to level {Level}!", ConsoleColor.Green, true);
        UI.Message($"Your stats have increased:\r\n" +
            $"Max Health: {MaxHealth} from {MaxHealth - 10}\r\n" +
            $"Attack Power: {AttackPower} from {AttackPower - 2}\r\n" +
            $"Defence: {Defence} from {Defence - 1}", ConsoleColor.Green, true);
    }

    /// <summary>
    /// Gets the required xp.
    /// </summary>
    /// <returns>The value of xp required</returns>
    public int GetRequiredXP() => Level * 100;

    /// <summary>
    /// Gets the inventory.
    /// </summary>
    /// <returns>The inventory.</returns>
    public Inventory GetInventory() => Inventory;

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        const string Title = "\r\n=================================================" +
            "\r\n                  PLAYER STATS                  \r\n" +
            "=================================================";

        string stats = $"\r\nName: {Name} \r\nHP: {Health} \r\n\r\nInventory:\r\n{GetInventory()}\r\n";

        return Title + stats;
    }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);

        if (target.Health <= 0)
        {
            UI.Message($"You defeated the {target.Name}!", ConsoleColor.Green, true);
        }
    }
}