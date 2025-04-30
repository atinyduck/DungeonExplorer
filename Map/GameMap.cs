namespace DungeonExplorer;
public enum Direction
{
    North,
    East,
    South,
    West
}

public class GameMap
{
    private List<Room> _map { get; set; }
    public IReadOnlyList<Room> Map => _map.AsReadOnly();
    public Room CurrentRoom { get; private set; }
    public List<Room> VisitedRooms { get; private set; } = new List<Room>();

    public GameMap(Room startingRoom, int maxDepth = 5, int maxBranching = 3)
    {
        _map = new List<Room>();
        CurrentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
        _map.Add(startingRoom);
    }

    public void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        if (_map.Contains(room)) throw new InvalidOperationException("Room already exists in the map.");
        _map.Add(room);
    }

    public bool Move(Direction direction)
    {
        var neighbour = CurrentRoom.GetNeighbour(direction);
        if (neighbour != null)
        {
            CurrentRoom = neighbour;
            if (!VisitedRooms.Contains(CurrentRoom)) VisitedRooms.Add(CurrentRoom);
            if (!_map.Contains(CurrentRoom)) _map.Add(CurrentRoom);
            
            return true;
        }
        return false;
    }
}