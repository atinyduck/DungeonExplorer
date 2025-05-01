namespace DungeonExplorer;
/// <summary>
/// Interface for collectable items.
/// </summary>
public interface ICollectable
{
    string Name { get; }
    string Description { get; }
    void Use(Creature target);
}