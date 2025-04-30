namespace DungeonExplorer;
public enum BaseStatistic
{
    Defence,
    AttackPower,
    MaxHealth
}
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

    protected void SetInitialState(string name, int maxHealth, int health, int defence, int attackPower)
    {
        Name = name;
        MaxHealth = maxHealth;
        BaseDefence = defence;
        BaseAttackPower = attackPower;
        if (Health > MaxHealth) Health = maxHealth;
        else Health = health;
        RecalculateStats();
    }
    public abstract void Attack(IDamageable target);

    public void TakeDamage(int damage)
    {
        int actualDamage = Math.Max(0, damage - Defence);
        Health = Math.Max(0, Health - actualDamage);
        
        UI.Message($"{Name} takes {actualDamage} damage!", ConsoleColor.Red, true);
    }

    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
        
        UI.Message($"{Name} heals for {amount} HP!", ConsoleColor.Green, true);
    }

    public void EquipArmour(Armour armour)
    {
        EquippedArmour = armour;
        RecalculateStats();
    }

    public void EquipWeapon(Weapon weapon)
    {
        EquippedWeapon = weapon;
        RecalculateStats();
    }

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

    public void RecalculateStats()
    {
        AttackPower = BaseAttackPower + (EquippedWeapon?.DamageModifier ?? 0);
        Defence = BaseDefence + (EquippedArmour?.DefenceModifier ?? 0);
    }

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

    public override string ToString()
    {
        // Display Creature
        return "";
    }
}