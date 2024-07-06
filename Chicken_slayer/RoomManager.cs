using System;
using System.Collections.Generic;

namespace Chicken_slayer
{
    public class Room
    {
        public string RoomID { get; private set; }
        public string Player1Name { get; private set; }
        public string Player2Name { get; private set; }
        public bool IsPlayer1Ready { get; set; }
        public bool IsPlayer2Ready { get; set; }

        public Room(string roomId, string player1Name)
        {
            RoomID = roomId;
            Player1Name = player1Name;
            Player2Name = null;
            IsPlayer1Ready = false;
            IsPlayer2Ready = false;
        }

        public bool AddPlayer(string playerName)
        {
            if (Player2Name == null)
            {
                Player2Name = playerName;
                return true;
            }
            return false;
        }
    }


    public class RoomManager
    {
        private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        private Random random = new Random();

        public string CreateRoom(string player1Name)
        {
            string roomId;
            do
            {
                roomId = random.Next(100000, 999999).ToString();
            } while (rooms.ContainsKey(roomId));

            rooms[roomId] = new Room(roomId, player1Name);
            return roomId;
        }

        public Room JoinRoom(string roomId, string playerName)
        {
            if (rooms.ContainsKey(roomId))
            {
                Room room = rooms[roomId];
                if (room.AddPlayer(playerName))
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
