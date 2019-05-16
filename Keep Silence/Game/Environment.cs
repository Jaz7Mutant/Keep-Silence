using System;
using System.Drawing;
using System.Windows.Forms;

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
            game.KeyPressed = Keys.None;
            if (IsOpen)
                game.ChangeRoom(this);
            else
                game.ShowMessage("Дверь закрыта");
        }

        public string GetImageFileName() => "Door.png";
    }

    public class Chest : IEnvironment
    {
        public double Illumination { get; set; }
        public int DeltaPlayerHealthPoints;
        public string RoomNameWhereUnlockDoor;
        public Point DoorToUnlock;
        public string Message;
        public int DeltaPlayerFlashlightPoints;
        public int DeltaFlashlightRadius;
        private string chestName = "Chest.png";

        public void InteractWithPlayer(Game game)
        {
            game.ShowMessage(Message);
            Message = null;
            game.Player.ChangeHealthPoints(DeltaPlayerHealthPoints, game);
            game.Player.ChangeFlashlightPoints(DeltaPlayerFlashlightPoints);
            game.Player.ChangeLightningRadius(DeltaFlashlightRadius);
            DeltaPlayerHealthPoints = 0;
            DeltaPlayerFlashlightPoints = 0;
            DeltaFlashlightRadius = 0;
            DeltaPlayerHealthPoints = 0;
            game.KeyPressed = Keys.None;
            if (DoorToUnlock.X != -1 && DoorToUnlock.Y != -1)
            {
                ((Door) game.RoomList[RoomNameWhereUnlockDoor].Map[DoorToUnlock.X, DoorToUnlock.Y]).IsOpen = true;
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
