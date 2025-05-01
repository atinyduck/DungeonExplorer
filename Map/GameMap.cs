namespace DungeonExplorer;
/// <summary>
/// Enum for all possible directions.
/// </summary>
public enum Direction
{
    North,
    East,
    South,
    West
}

/// <summary>
/// Class representing map of <see cref="Room"/>s."
/// </summary>
public class GameMap
{
    private List<Room> _map { get; set; }
    public IReadOnlyList<Room> Map => _map.AsReadOnly();
    public Room CurrentRoom { get; private set; }
    public List<Room> VisitedRooms { get; private set; } = new List<Room>();

    /// <summary>
    /// Initializes a new instance of the <see cref="GameMap"/> class.
    /// </summary>
    /// <param name="startingRoom">The starting room.</param>
    /// <param name="maxDepth">The maximum depth.</param>
    /// <param name="maxBranching">The maximum branching.</param>
    /// <exception cref="System.ArgumentNullException">startingRoom</exception>
    public GameMap(Room startingRoom, int maxDepth = 5, int maxBranching = 3)
    {
        _map = new List<Room>();
        CurrentRoom = startingRoom ?? throw new ArgumentNullException(nameof(startingRoom));
        AddRoom(startingRoom);
    }

    /// <summary>
    /// Adds the room.
    /// </summary>
    /// <param name="room">The room.</param>
    /// <exception cref="System.ArgumentNullException">room</exception>
    /// <exception cref="System.InvalidOperationException">Room already exists in the map.</exception>
    public void AddRoom(Room room)
    {
        if (room == null) throw new ArgumentNullException(nameof(room));
        if (_map.Contains(room)) throw new InvalidOperationException("Room already exists in the map.");
        _map.Add(room);
    }

    /// <summary>
    /// Moves the specified direction.
    /// </summary>
    /// <param name="direction">The direction.</param>
    /// <returns>If the move was a sucess</returns>
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