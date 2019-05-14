using System;
using System.Drawing;

namespace Keep_Silence
{
    public class Door : IEnvironment
    {
        public double Illumination { get; set; }
        public bool IsOpen;
        public string NextRoomName;
        public Point NextRoomStartPosition;

        public void InteractWithPlayer(Game game)
        {
            if (IsOpen)
                game.ChangeRoom(this);
            else
                game.ShowMessage("Door is locked");
        }

        public string GetImageFileName() => "Door.png";
    }

    public class Chest : IEnvironment
    {
        public double Illumination { get; set; }
        public int DeltaPlayerHealthPoints;
        public Point DoorToUnlock;
        public string Message;
        public int DeltaPlayerFlashlightPoints;
        public int NewFlashlightRadius;
        private string chestName = "Chest.png";

        public void InteractWithPlayer(Game game)
        {
            game.ShowMessage(Message);
            Message = null;
            game.Player.ChangeHealthPoints(DeltaPlayerHealthPoints, game);
            game.Player.ChangeFlashlightPoints(DeltaPlayerFlashlightPoints);
            game.Player.ChangeLightningRadius(NewFlashlightRadius);
            DeltaPlayerHealthPoints = 0;
            if (DoorToUnlock.X != -1 && DoorToUnlock.Y != -1)
            {
                ((Door) game.CurrentRoom.Map[DoorToUnlock.X, DoorToUnlock.Y]).IsOpen = true;
            }
            chestName = "OpenChest.png";
        }

        public string GetImageFileName() => chestName;
    }

    public class Terrain : IEnvironment
    {
        public double Illumination { get; set; }

        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");

        public string GetImageFileName() => "Terrain.png";
    }

    public class Wall : IEnvironment
    {
        public double Illumination { get; set; }

        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");

        public string GetImageFileName() => "Wall.png";
    }

    public class Darkness : IEnvironment
    {
        public double Illumination { get; set; } = 0;

        public void InteractWithPlayer(Game game) => throw new Exception("Non Interactive object");

        public string GetImageFileName() => "Darkness.png";
    }
}
