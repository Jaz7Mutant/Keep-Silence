using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keep_Silence
{
    public static class Drawer
    {
        public static void DrawGame(PaintEventArgs e, Game game, GameState gameState, Dictionary<string,Bitmap> bitmaps, Timer timer)
        {
            e.Graphics.TranslateTransform(0, GameState.CellSize);
            e.Graphics.FillRectangle(
                Brushes.Aqua, 0, 0, GameState.CellSize * game.CurrentRoom.Width,
                GameState.CellSize * game.CurrentRoom.Height);
            for (var x = 0; x < game.CurrentRoom.Map.GetLength(0); x++)
                for (var y = 0; y < game.CurrentRoom.Map.GetLength(1); y++)
                {
                    var environment = game.CurrentRoom.Map[x, y];
                    e.Graphics.DrawImage(bitmaps[environment.GetImageFileName()],
                        gameState.ConvertPointToImageSize(new Point(x, y)));
                }

            foreach (var a in gameState.Animations)
            {
                if (a.Creature is Player)
                {
                    var image = a.HitAnimation
                        ? bitmaps[a.Creature.GetHitImageFileName()]
                        : bitmaps[a.Creature.GetImageFileName()];
                    if (game.Player.ChangedDirection)
                        image.RotateFlip(GetRotate(game.Player.Direction));
                    e.Graphics.DrawImage(image, a.Location);

                }

                if (a.Creature is Monster)
                {
                    var image = a.HitAnimation
                        ? bitmaps[a.Creature.GetHitImageFileName()]
                        : bitmaps[a.Creature.GetImageFileName()];
                    //if (game.Player.ChangedDirection)
                    //    image.RotateFlip(GetRotate(game.Player.Direction)); //TODO
                    e.Graphics.DrawImage(image, a.Location);
                }
                e.Graphics.DrawEllipse(new Pen(Color.AliceBlue, 2),
                    (a.Location.X - a.Creature.GetNoiseLevel() * GameState.CellSize / 2) + 0.5f * GameState.CellSize,
                    (a.Location.Y - a.Creature.GetNoiseLevel() * GameState.CellSize / 2) + 0.5f * GameState.CellSize,
                    a.Creature.GetNoiseLevel() * GameState.CellSize,
                    a.Creature.GetNoiseLevel() * GameState.CellSize);
            }

            e.Graphics.ResetTransform();

            if (game.CurrentMessage != null)
            {
                timer.Enabled = false;
                MessageBox.Show(game.CurrentMessage);
                timer.Enabled = true;
                game.CurrentMessage = null;
            }
            e.Graphics.DrawString(game.Player.HealthPoints.ToString(CultureInfo.InvariantCulture), new Font("Vendara", 15), Brushes.DarkMagenta, 0, 0);

        }

        private static RotateFlipType GetRotate(Directions direction)
        {
            switch (direction)
            {
                //TODO Исправить в зависимости от начального положения
                case Directions.Down:
                    return RotateFlipType.Rotate180FlipNone;
                case Directions.Up:
                    return RotateFlipType.RotateNoneFlipNone;
                case Directions.Right:
                    return RotateFlipType.Rotate90FlipNone;
                case Directions.Left:
                    return RotateFlipType.Rotate270FlipNone;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
