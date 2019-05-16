using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Keep_Silence.GameForms;

namespace Keep_Silence
{
    public static class Drawer
    {
        private static int noiseCircleRadius;
        private static Form pauseMenu;

        public static void DrawGame(PaintEventArgs e, Game game, GameState gameState, Dictionary<string,Bitmap> bitmaps, Dictionary<string,Bitmap> menuBitmaps, Timer timer, int tickCount)
        {
            //todo Collect Bitmaps and draw it
            if (game.IsPaused)
            {
                timer.Enabled = false;
                PauseMenu(menuBitmaps, game);
                timer.Enabled = true;
            }

            //GameState.CellSize = 32;
            GameState.CellSize = game.CurrentRoom.Height < game.CurrentRoom.Width
                ? (Screen.PrimaryScreen.Bounds.Height - 50) / game.CurrentRoom.Height
                : Screen.PrimaryScreen.Bounds.Width / game.CurrentRoom.Width;
            e.Graphics.TranslateTransform(0, 50);
            e.Graphics.FillRectangle(
                Brushes.Black, Screen.PrimaryScreen.WorkingArea);
            DrawEnvironment(e, game, gameState, bitmaps);
            DrawCreatures(e, gameState, bitmaps);

            e.Graphics.ResetTransform();

            if (game.CurrentMessage != null)
            {
                timer.Enabled = false;
                var message = new MessageForm(game.CurrentMessage, menuBitmaps);
                message.StartPosition = FormStartPosition.CenterParent;
                message.ShowDialog();
                timer.Enabled = true;
                game.CurrentMessage = null;
            }

            e.Graphics.DrawString(
                "Здоровье: " + ((int)game.Player.GetHealthPoints()) + "\t" + "Заряд фонарика: " + 
                game.Player.GetFlashlightPoints(), new Font("Vendara", 14), Brushes.DarkMagenta, 0, 0);

        }

        private static void PauseMenu(Dictionary<string,Bitmap> menuBitmaps, Game game)
        {
            game.KeyPressed = Keys.None;
            game.IsPaused = false;
            pauseMenu = new PauseForm(menuBitmaps, game) {StartPosition = FormStartPosition.CenterParent};
            pauseMenu.ShowDialog();
        }

        private static void DrawCreatures(PaintEventArgs e, GameState gameState, Dictionary<string, Bitmap> bitmaps)
        {
            foreach (var a in gameState.Animations)
            {
                bitmaps[a.Creature.GetHitImageFileName()].RotateFlip(a.Command.Rotate);
                bitmaps[a.Creature.GetImageFileName()].RotateFlip(a.Command.Rotate);
                var image = a.HitAnimation
                    ? bitmaps[a.Creature.GetHitImageFileName()]
                    : bitmaps[a.Creature.GetImageFileName()];
                DrawNoiseCircle(e, a);
                if (a.Creature is Monster monster && monster.GetVisibility() < 1)
                    continue;
                e.Graphics.DrawImage(image, a.Location.X, a.Location.Y, GameState.CellSize, GameState.CellSize);
                noiseCircleRadius += a.Creature.GetNoiseLevel()*3;
            }
        }

        private static void DrawEnvironment(PaintEventArgs e, Game game, GameState gameState, Dictionary<string, Bitmap> bitmaps)
        {
            for (var x = 0; x < game.CurrentRoom.Map.GetLength(0); x++)
                for (var y = 0; y < game.CurrentRoom.Map.GetLength(1); y++)
                {
                    var environment = game.CurrentRoom.Map[x, y];
                    var img = bitmaps[environment.GetImageFileName()];
                    if (environment.Illumination < 1)
                        continue;
                    var imgPos = gameState.ConvertPointToImageSize(new Point(x, y));
                    var delta = GameState.CellSize / 32;
                    e.Graphics.DrawImage(img, new Rectangle(imgPos.X, imgPos.Y, GameState.CellSize + delta, GameState.CellSize + delta));
                    if (environment.Illumination < 100)
                    {
                        var solidBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
                        e.Graphics.FillRectangle(solidBrush, imgPos.X, imgPos.Y, GameState.CellSize + delta, GameState.CellSize + delta);
                    }
                }
        }

        
        private static void DrawNoiseCircle(PaintEventArgs e, CreatureAnimation a)
        {
            if (a.Creature.GetNoiseLevel() == 0)
                return;
            var radius = (a.Creature.GetNoiseLevel() + 1) * GameState.CellSize;
            if (noiseCircleRadius <= radius)
                radius = noiseCircleRadius;       
            else
                noiseCircleRadius = 0;

            var circleRectangle = new Rectangle(
                a.Location.X - (radius - GameState.CellSize) / 2,
                a.Location.Y - (radius - GameState.CellSize) / 2,
                radius,
                radius);
            e.Graphics.DrawEllipse(new Pen(Color.LightGray, 3),
                circleRectangle);
        }
    }
}
