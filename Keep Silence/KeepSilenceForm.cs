using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private readonly Timer timer = new Timer {Interval = 100};

        public KeepSilenceForm(Game game, DirectoryInfo imagesDirectory = null)
        {
            this.game = game;
            gameState = new GameState();
            ClientSize = new Size(
                GameState.CellSize * game.CurrentRoom.Width,
                GameState.CellSize * game.CurrentRoom.Height + GameState.CellSize);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "Images")); ;
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name] = (Bitmap) Image.FromFile(e.FullName);

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
            pressedKeys.Remove(e.KeyCode);
            game.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e) => Drawer.DrawGame(e, game, gameState, bitmaps, timer, tickCount);

        private void TimerTick(object sender, EventArgs args)
        {
            gameState.PerformAct(game);
            tickCount++;
            tickCount %= 100;
            Invalidate();
        }
    }
}
