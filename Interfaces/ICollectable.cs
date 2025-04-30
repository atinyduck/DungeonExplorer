namespace DungeonExplorer;
public interface ICollectable
{
    string Name { get; }
    string Description { get; }
    void Use(Creature target);
}