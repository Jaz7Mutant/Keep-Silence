using System;
using System.Drawing;

namespace Keep_Silence
{
    public class Door : IEnvironment
    {
        public double Illumination { get; set; }
        public bool IsOpen;
        public string nextRoomName;

        public void InteractWithPlayer(Game game)
        {
            if (IsOpen)
            {
                game.ChangeRoom(nextRoomName);
            }
        }
    }

    public class Chest : IEnvironment
    {
        public double Illumination { get; set; }
        public double DeltaPlayerHealthPoints;
        public Point DoorToUnlock;
        public string Message;

        public void InteractWithPlayer(Game game)
        {
            game.Player.ChangeHealthPoints(DeltaPlayerHealthPoints, game);
            if (game.CurrentRoom.Map[DoorToUnlock.X, DoorToUnlock.Y] is Door)
            {
                ((Door) game.CurrentRoom.Map[DoorToUnlock.X, DoorToUnlock.Y]).IsOpen = true;
            }
            game.ShowMessage(Message);
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
