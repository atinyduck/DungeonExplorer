using System.Security.Cryptography.X509Certificates;

namespace DungeonExplorer;
public class ClockworkMage : Monster
{
    const int AttackNum = 2;
    public ClockworkMage()
            : base("Clockwork Mage", maxHealth: 30, defence: 3, attackPower: 4, rewardXP: 25)
    { }

    public override void Attack(IDamageable target)
    {
        for (int i = AttackNum; i < 2; i++)
        {
            int damage = Convert.ToInt16(AttackPower);
            target.TakeDamage(damage);
            
            UI.Message("The Clockwork Mage attacks you with a bolt of energy!", wait: true);
            UI.Message($"Dealing {damage} to you.");
        }
    }
}

public class RustingConstruct : Monster
{
    private const int LootChance = 3;

    public RustingConstruct()
            : base("Rusting Construct", maxHealth: 75, defence: 6, attackPower: 8, rewardXP: 32)
    { }

    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
        
        UI.Message("The Rusting Construct attacks you with its claws!", wait: true);
        UI.Message($"Dealing {damage} to you.");
    }

    public override List<Item> GenerateDrops()
    {
        var drops = new List<Item>();
        var random = new Random();
        if (random.Next(0, LootChance) == 0)
        {
            drops.Add();
        }

        return drops;
    }
}

public class RepairUnit : Monster
{
    private const int RepairChance = 4;
    private const float RepairPercent = 0.1f;

    public RepairUnit()
            : base("Clockwork Mage", maxHealth: 40, defence: 4, attackPower: 4, rewardXP: 22)
    { }

    public override void Attack(IDamageable target)
    {
        int damage = AttackPower;
        target.TakeDamage(damage);
       
        UI.Message("The Repair Unit attacks you with a wrench!", wait: true);
        UI.Message($"Dealing {damage} to you.");


        var random = new Random();
        if (random.Next(0, RepairChance) == 0)
        {
            int healthIncrease = Convert.ToInt16(MaxHealth * RepairPercent);
            Heal(healthIncrease);

            UI.Message($"The Repair Unit repairs itself for {healthIncrease} health!");
        }
    }
}