using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
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

        public GameMap(Room startingRoom)
        {
            _map = new List<Room>();
            CurrentRoom = startingRoom;
            AddRoom(startingRoom);
        }

        public void AddRoom(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));
            _map.Add(room);
        }

        private void GenerateRoom()
        {
            var room = new Room();
            AddRoom(room);
        }

        public void GenerateMap(int maxDepth = 0, int depth = 0)
        {
            
        }

        public bool MovePlayer(Direction direction)
        {
            var neighbour = CurrentRoom.GetNeighbour(direction);
            if (neighbour != null)
            {
                CurrentRoom = neighbour;
                if (!_rooms.Contains(CurrentRoom))
                {
                    _rooms.Add(CurrentRoom);
                }
                return true;
            }
            return false;
        }
    }
}