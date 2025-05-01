namespace DungeonExplorer;
/// <summary>
/// Enum for the base statistics of a creature.
/// </summary>
public enum BaseStatistic
{
    Defence,
    AttackPower,
    MaxHealth
}
/// <summary>
/// Base class for all creatures.
/// </summary>
/// <seealso cref="DungeonExplorer.IDamageable" />
public abstract class Creature : IDamageable
{
    public string Name { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public Inventory Inventory { get; private set; }
    public int BaseDefence { get; private set; }
    public int BaseAttackPower { get; private set; }
    public int Defence { get; private set; }
    public int AttackPower { get; private set; }
    public Weapon EquippedWeapon { get; private set; }
    public Armour EquippedArmour { get; private set; }

    public List<(PotionEffect effect, int duration, int power)> ActiveEffects { get; private set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Creature"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="maxHealth">The maximum health.</param>
    /// <param name="defence">The defence.</param>
    /// <param name="attackPower">The attack power.</param>
    protected Creature(string name, int maxHealth, int defence, int attackPower)
    {
        Name = name;
        Health = maxHealth;
        MaxHealth = maxHealth;
        Inventory = new Inventory();
        BaseDefence = defence;
        BaseAttackPower = attackPower;
        Defence = defence;
        AttackPower = attackPower;
    }

    /// <summary>
    /// Attacks the specified target.
    /// </summary>
    /// <param name="target">The target.</param>
    public abstract void Attack(IDamageable target);

    /// <summary>
    /// Damages the specified amount.
    /// </summary>
    /// <param name="damage">The damage.</param>
    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(0, damage - Defence);
        Health = Math.Max(0, Health - actualDamage);
        
        UI.Message($"{Name} takes {actualDamage} damage!", ConsoleColor.Red, true);
    }

    /// <summary>
    /// Heals the specified amount.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
        
        UI.Message($"{Name} heals for {amount} HP!", ConsoleColor.Green, true);
    }

    /// <summary>
    /// Equips the armour.
    /// </summary>
    /// <param name="armour">The armour.</param>
    public void EquipArmour(Armour armour)
    {
        EquippedArmour = armour;
        RecalculateStats();
    }

    /// <summary>
    /// Equips the weapon.
    /// </summary>
    /// <param name="weapon">The weapon.</param>
    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        RecalculateStats();
    }

    /// <summary>
    /// Modifies the base stat.
    /// </summary>
    /// <param name="stat">The stat.</param>
    /// <param name="newValue">The new value.</param>
    public void ModifyBaseStat(BaseStatistic stat, int newValue)
    {
        switch (stat)
        {
            case BaseStatistic.Defence:
                BaseDefence = newValue;
                break;

            case BaseStatistic.AttackPower:
                BaseAttackPower = newValue;
                break;

            case BaseStatistic.MaxHealth:
                MaxHealth = newValue;
                // If the max is reduced, ensure that health does not exceed it.
                Health = Math.Min(Health, MaxHealth);
                break;
        }
        RecalculateStats();
    }

    /// <summary>
    /// Recalculates the stats.
    /// </summary>
    public void RecalculateStats()
    {
        AttackPower = BaseAttackPower + (EquippedWeapon?.DamageModifier ?? 0);
        Defence = BaseDefence + (EquippedArmour?.DefenceModifier ?? 0);
    }

    /// <summary>
    /// Processes the effect.
    /// </summary>
    public void ProcessEffect()
    {
        // Process active effects
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            var effect = ActiveEffects[i];
            if (effect.duration > 0)
            {
                // Apply effect
                effect.duration--;
                switch (effect.effect)
                {
                    case PotionEffect.Poison:
                        TakeDamage(effect.power);
                        break;
                    case PotionEffect.StrengthBuff:
                        ModifyBaseStat(BaseStatistic.AttackPower, AttackPower + effect.power);
                        break;
                    case PotionEffect.DefenceBuff:
                        ModifyBaseStat(BaseStatistic.Defence, Defence + effect.power);
                        break;
                }
            }
            else
            {
                // Remove effect
                ActiveEffects.RemoveAt(i);
                UI.Message("The effect has worn off!", ConsoleColor.Yellow, true);
            }
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
        // Display Creature
        return $"{Name} | HP : {Health} | Attack: {AttackPower} | Defence {Defence}";
    }
}