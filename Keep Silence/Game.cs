using System;
using System.Collections.Generic;
using System.Drawing;
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

        public double GetDistanceBetweenPoints(Point first, Point second) =>
            Math.Sqrt((first.X - second.X) * (first.X - second.X) 
                      + (first.Y - second.Y) * (first.Y - second.Y));

        public bool IsStepCorrect(Point current, Point target) =>
            (target.X - current.X == 0 ^ target.Y - current.Y == 0)
            && InBounds(target)
            && CurrentRoom.Map[target.X, target.Y] is Terrain;

        private bool InBounds(Point point) =>
            point.X < CurrentRoom.Width 
            && point.X >= 0 
            && point.Y < CurrentRoom.Height 
            && point.Y >= 0;
    }
}
