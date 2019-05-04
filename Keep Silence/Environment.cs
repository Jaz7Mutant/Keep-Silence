using System;

namespace Keep_Silence
{
    public class Door : IEnvironment
    {
        public double Illumination { get; set; }
        public void InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }

    public class Chest : IEnvironment
    {
        public double Illumination { get; set; }
        public void InteractWithPlayer(Game game)
        {
            throw new NotImplementedException();
        }
    }

    public class Terrain : IEnvironment
    {
        public double Illumination { get; set; }

        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");
    }

    public class Wall : IEnvironment
    {
        public double Illumination { get; set; }
        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");
    }

    public class Darkness : IEnvironment
    {
        public double Illumination { get; set; } = 0;

        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");
    }
}
