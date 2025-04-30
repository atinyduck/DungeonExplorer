namespace DungeonExplorer;
public interface IDamageable
{
    string Name { get; }
    int Health { get; }
    int MaxHealth { get; }
    int AttackPower { get; }
    int Defence { get; }
    void TakeDamage(int damage);
    void Heal(int amount);
}
