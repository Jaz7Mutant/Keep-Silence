using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keep_Silence
{
    public class Game
    {
        public Room CurrentRoom;
        public Keys KeyPressed;
        public Player Player;
        private Dictionary<string, Room> roomList;

        public void LoadRooms()
        {
            throw new NotImplementedException();
        }

        public void ChangeRoom(string roomName)
        {
            throw new NotImplementedException();
        }
    }
}
