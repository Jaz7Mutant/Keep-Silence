using System.Collections.Generic;
using System.Drawing;

namespace Keep_Silence
{
    public class Room
    {
        public IEnvironment[,] Map;
        public List<Monster> Monsters;
        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);

        public void LightenNewArea(int radius, Point center)
        {
            if (radius == 0)
            {
                ChangeIlluminationInArea(10, center, 0);
            }
            ChangeIlluminationInArea(2*radius, center, 0);
            ChangeIlluminationInArea(radius, center, 100);
        }

        private void ChangeIlluminationInArea(int radius, Point center, double illumination)
        {
            for (var i = -radius; i <= radius; i++)
            {
                for (var j = -radius; j <= radius; j++)
                {
                    var x = center.X + i;
                    var y = center.Y + j;
                    if (x < 0 || y < 0 || x >= Width || y >= Height)
                        continue;
                    Map[x, y].Illumination = illumination;
                }
            }
        }
    }
}
