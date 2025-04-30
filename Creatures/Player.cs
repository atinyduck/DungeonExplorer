using System.Numerics;
using System.Threading;

namespace DungeonExplorer;
public class Player : Creature, ISaveable
{
    public int Experience { get; private set; }
    public int Level { get; private set; }
    public Statistics Statistics { get; private set; } = new Statistics();

    public Player(string name, int level = 1, int experience = 0, int maxHealth = 100, int defence = 3, int attackPower = 10) 
        : base(name, maxHealth: maxHealth, defence: defence, attackPower: attackPower)
    { 
        Experience = experience;
        Level = level;
    }

    #region Save Data
    public string GetSaveIdentifier() => $"player_{Name}";

    #endregion


    public void GainExperience(int amount)
    {
        Experience += amount;
        //Display increase

        while (Experience >= GetRequiredXP())
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increase Stats
        Level++;
        ModifyBaseStat(BaseStatistic.MaxHealth, MaxHealth + 10);
        ModifyBaseStat(BaseStatistic.AttackPower, BaseAttackPower + 2);
        ModifyBaseStat(BaseStatistic.Defence, BaseDefence + 1);
        Heal(MaxHealth);

        //Display level up
    }

    public int GetRequiredXP() => Level * 100;

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