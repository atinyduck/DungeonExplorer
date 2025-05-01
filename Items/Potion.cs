namespace DungeonExplorer;
/// <summary>
/// Emum for the different types of potion effects.
/// </summary>
public enum PotionEffect
{
    Heal,
    Poison,
    StrengthBuff,
    DefenceBuff,
    Invisibility
}

/// <summary>
/// Represents an item of type <see cref="Potion"/>.
/// </summary>
/// <seealso cref="DungeonExplorer.Item" />
/// <seealso cref="DungeonExplorer.ISaveable" />
public class Potion : Item, ISaveable
{
    public PotionEffect EffectType { get; private set; }
    public int EffectDuration { get; private set; }
    public int EffectPower { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Potion"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="type">The type.</param>
    /// <param name="duration">The duration.</param>
    /// <param name="power">The power.</param>
    public Potion(string name, string description, PotionEffect type, int duration, int power)
            : base(name, description)
    {
        EffectType = type;
        EffectDuration = duration;
        EffectPower = power; // -1 Refers to instant effects, e.g. Heal.
    }

    /// <summary>
    /// Gets the save identifier.
    /// </summary>
    /// <returns>
    /// The identifier
    /// </returns>
    public override string GetSaveIdentifier() => $"potion_{EffectType}_{EffectPower}";

    /// <summary>
    /// Applies the poison.
    /// </summary>
    /// <param name="target">The target.</param>
    public void ApplyPoison(Creature target)
    {
        if (EffectDuration > 0) // Temporary poison
        {
            target.ActiveEffects.Add((PotionEffect.Poison, EffectDuration, EffectPower));
            target.ProcessEffect();
        }
        else // Instant poison
        {
            target.TakeDamage(EffectPower);
            UI.Message($"The {target.Name} has been poisoned for {EffectPower} damage!", ConsoleColor.Red, wait: true);
        }
    }

    /// <summary>
    /// Applies the buff.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="stat">The stat.</param>
    public void ApplyBuff(Creature target, BaseStatistic stat)
    {
        if (EffectDuration > 0) // Temporary buff
        {
            target.ActiveEffects.Add((PotionEffect.Poison, EffectDuration, EffectPower));
            target.ProcessEffect();
        }
        else //Permanent buff
        {
            int amount = EffectPower;
            switch (stat)
            {
                case BaseStatistic.AttackPower:
                    amount += target.BaseAttackPower;
                    break;

                case BaseStatistic.Defence:
                    amount += target.BaseDefence;
                    break;

                case BaseStatistic.MaxHealth:
                    amount += target.MaxHealth;
                    break;
            }

            target.ModifyBaseStat(stat, amount);
        }
    }

    /// <summary>
    /// Uses the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public override void Use(Creature target)
    {
        switch (EffectType)
        {
            case PotionEffect.Heal:
                target.Heal(EffectPower);
                UI.Message($"{target.Name} heals for {EffectPower} HP!", ConsoleColor.Green, wait: true);
                break;
            case PotionEffect.Poison:
                ApplyPoison(target);
                break;
            case PotionEffect.StrengthBuff:
                ApplyBuff(target, BaseStatistic.AttackPower);
                UI.Message($"{target.Name}'s attack power increased by {EffectPower}!", ConsoleColor.Green, wait: true);
                break;
            case PotionEffect.DefenceBuff:
                ApplyBuff(target, BaseStatistic.Defence);
                UI.Message($"{target.Name}'s defence increased by {EffectPower}!", ConsoleColor.Green, wait: true);
                break;
            case PotionEffect.Invisibility:
                // Implement invisibility effect
                break;
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
        return $"{Name} (Effect: {EffectType}, Duration: {EffectDuration}, Power: {EffectPower})";
    }
}