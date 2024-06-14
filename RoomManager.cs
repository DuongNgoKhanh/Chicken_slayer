using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer
{
    public class Room
    {
        public string RoomID { get; private set; }
        public bool Player1Ready { get; set; }
        public bool Player2Ready { get; set; }
        public int PlayerCount { get; private set; }

        public Room(string roomId)
        {
            RoomID = roomId;
            Player1Ready = false;
            Player2Ready = false;
            PlayerCount = 1;
        }

        public bool AddPlayer()
        {
            if (PlayerCount < 2)
            {
                PlayerCount++;
                return true;
            }
            return false;
        }
    }

    public class RoomManager
    {
        private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private Random random = new Random();

        public string CreateRoom()
        {
            string roomId;
            do
            {
                roomId = random.Next(100000, 999999).ToString();
            } while (rooms.ContainsKey(roomId));

            rooms[roomId] = new Room(roomId);
            return roomId;
        }

        public Room JoinRoom(string roomId)
        {
            if (rooms.ContainsKey(roomId))
            {
                Room room = rooms[roomId];
                if (room.AddPlayer())
                {
                    return room;
                }
            }
            return null;
        }

        public Room GetRoom(string roomId)
        {
            rooms.TryGetValue(roomId, out Room room);
            return room;
        }
    }
}
