using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keep_Silence
{
    public class Wall : IEnvironment
    {
        public double Illumination { get; set; }
        public bool InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }

    public class Chest : IEnvironment
    {
        public double Illumination { get; set; }
        public bool InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }

    public class Darkness : IEnvironment
    {
        public double Illumination { get; set; }
        public bool InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }

    public class Door : IEnvironment
    {
        public double Illumination { get; set; }
        public bool InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
