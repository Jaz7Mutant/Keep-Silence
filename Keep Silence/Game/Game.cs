using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Keep_Silence
{
    public class Game
    {
        public Room CurrentRoom;
        public Keys KeyPressed;
        public Player Player;
        public string CurrentMessage;
        public bool IsPaused;
        public bool IsEnd;
        public Dictionary<string, Room> RoomList;

        public void GameOver() => IsEnd = true;

        public void Pause() => IsPaused = true;

        public void LoadRooms()
        {
            RoomList = RoomLoader.GetRoomsFromDirectory();
            CurrentRoom = RoomList.First().Value;
            Player = new Player() {Position = new Point(4,2)};
        }

        public void ChangeRoom(Door door)
        {
            CurrentRoom = RoomList[door.NextRoomName];
            Player.Position = door.NextRoomStartPosition;
            KeyPressed = Keys.None;
        }

        public void ShowMessage(string message)
        {
            CurrentMessage = message;
        }

        public PlayerActions GetPlayerAction()
        {
            switch (KeyPressed)
            {
                case Keys.D:
                    return PlayerActions.MoveRight;
                case Keys.W:
                    return PlayerActions.MoveUp;
                case Keys.A:
                    return PlayerActions.MoveLeft;
                case Keys.S:
                    return PlayerActions.MoveDown;
                case Keys.F:
                    return PlayerActions.Interact;
                case Keys.Space:
                    return PlayerActions.Hit;
                case Keys.Escape:
                    Pause();
                    return PlayerActions.Idle;
                default:
                    return PlayerActions.Idle;
            }
        }

        public static RotateFlipType GetImageRotation(Directions newDirection, Directions Direction)
        {
            if (newDirection == Directions.None) return RotateFlipType.RotateNoneFlipNone;
            switch (newDirection - Direction)
            {
                case 0:
                    return RotateFlipType.RotateNoneFlipNone;
                case 1:
                case -3:
                    return RotateFlipType.Rotate270FlipNone;
                case 2:
                case -2:
                    return Direction == Directions.Down || Direction == Directions.Up
                        ? RotateFlipType.RotateNoneFlipY
                        : RotateFlipType.RotateNoneFlipX;
                case 3:
                case -1:
                    return RotateFlipType.Rotate90FlipNone;
                default:
                    throw new Exception("Incorrect direction");
            }
        }

        public double GetDistanceBetweenPoints(Point first, Point second) =>
            Math.Sqrt((first.X - second.X) * (first.X - second.X) 
                      + (first.Y - second.Y) * (first.Y - second.Y));

        public bool IsStepCorrect(Point current, Point target) =>
            (target.X - current.X == 0 ^ target.Y - current.Y == 0)
            && InBounds(target)
            && CurrentRoom.Map[target.X, target.Y] is Terrain
            && CurrentRoom.Monsters.All(x => x.Position != target);

        public bool InBounds(Point point) =>
            point.X < CurrentRoom.Width 
            && point.X >= 0 
            && point.Y < CurrentRoom.Height 
            && point.Y >= 0;
    }
}
