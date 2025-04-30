namespace DungeonExplorer;
public enum PotionEffect
{
    Heal,
    Poison,
    StrengthBuff,
    DefenceBuff,
    Invisibility
}
public class Potion : Item, ISaveable
{
    public PotionEffect EffectType { get; private set; }
    public int EffectDuration { get; private set; }
    public int EffectPower { get; private set; }

    public Potion(string name, string description, PotionEffect type, int duration, int power)
            : base(name, description)
    {
        EffectType = type;
        EffectDuration = duration;
        EffectPower = power; // -1 Refers to instant effects, e.g. Heal.
    }

    #region Save Implementation
    public override string GetSaveIdentifier() => $"potion_{EffectType}_{EffectPower}";

    #endregion

    public void ApplyPoison(Creature target)
    {
        if (EffectDuration > 0)
        {
            //Poison for multiple turns
            //Used if status effects are implemented
        }
        else
        {
            target.TakeDamage(EffectPower);
            //Display poison info
        }
    }

    public void ApplyBuff(Creature target, BaseStatistic stat)
    {
        if (EffectDuration > 0)
        {
            //Temporary stat increase
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

    public override string ToString()
    {
        return base.ToString();
    }
}