using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Keep_Silence
{
    public partial class KeepSilenceForm : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Game game;
        private readonly GameState gameState;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private int tickCount;
        private readonly Timer timer = new Timer {Interval = 130};

        public KeepSilenceForm(Game game, DirectoryInfo imagesDirectory = null)
        {
            this.game = game;
            gameState = new GameState();
            ClientSize = new Size(
                GameState.CellSize * game.CurrentRoom.Width,
                GameState.CellSize * game.CurrentRoom.Height + GameState.CellSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Images")); ;
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap)Image.FromFile(e.FullName);
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Keep Silence";
            DoubleBuffered = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            game.KeyPressed = e.KeyCode;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Clear();
            //pressedKeys.Remove(e.KeyCode);
            game.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }


        //TODO AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        protected override void OnPaint(PaintEventArgs e)
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
                if (a.Creature is Player )
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
            }

            e.Graphics.ResetTransform();

            if (game.CurrentMessage != null)
            {
                timer.Enabled = false;
                MessageBox.Show(game.CurrentMessage);
                timer.Enabled = true;
                game.CurrentMessage = null;
            }

        }

        private RotateFlipType GetRotate(Directions direction)
        {
            switch (direction)
            {
                //TODO
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

        private void TimerTick(object sender, EventArgs args)
        {
            //Todo вызывать gameState и делать магию
            gameState.PerformAct(game);
            tickCount++;
            tickCount %= 100;
            Invalidate();
        }
    }
}
