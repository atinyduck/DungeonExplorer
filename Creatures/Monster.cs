namespace DungeonExplorer;
public class Monster : Creature, ISaveable
{
    public int RewardXP { get; private set; }

    protected Monster(string name, int maxHealth, int defence, int attackPower, int rewardXP)
        : base(name, maxHealth, defence, attackPower)
    {
        RewardXP = rewardXP;
    }

    #region Save Implementation
    public string GetSaveIdentifier() => $"monster_{GetType().Name}_{Health}";

    #endregion

    public virtual List<Item> GenerateDrops()
    {
        var drops = new List<Item>();
        var random = new Random();
        if (random.Next(0, 2) == 0)
        {
            drops.Add(new Potion("Health Potion", "A potion that restores health.", PotionEffect.Heal, -1, 20));
        }
        return drops;
    }

    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        if (EquippedWeapon != null)
        {
            damage += EquippedWeapon.DamageModifier;
        }
        target.TakeDamage(damage);
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"A {Name} stands before you!");
        builder.AppendLine($"Health: {Health}/{MaxHealth}");

        return builder.ToString();
    }
}