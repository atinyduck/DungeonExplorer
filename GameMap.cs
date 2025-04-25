using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class GameMap
    {
        private List<Room> _rooms { get; set; }
        public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();
        public Room CurrentRoom { get; private set; }

        public GameMap(Room startingRoom)
        {
            _rooms = new List<Room>();
            CurrentRoom = startingRoom;
            AddRoom(startingRoom);
        }

        public void AddRoom(Room room)
        {
            if (room == null) throw new ArgumentNullException(nameof(room));
            _rooms.Add(room);
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