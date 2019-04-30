using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keep_Silence
{
    public class Room
    {
        public IEnvironment[,] Map;
        public List<Monster> Monsters;
        public List<Point> playerStartPositions;
        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);

        public void LightenArea(int radius, Point center)
        {
            throw new NotImplementedException();
        }
    }
}
