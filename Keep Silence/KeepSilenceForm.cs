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
            var timer = new Timer();
            timer.Interval = 15;
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
            pressedKeys.Remove(e.KeyCode);
            game.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }
        

        //TODO AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, GameState.CellSize);
            e.Graphics.FillRectangle(
                Brushes.Black, 0, 0, GameState.CellSize * game.CurrentRoom.Width,
                GameState.CellSize * game.CurrentRoom.Height);
            foreach (var a in gameState.Animations)
                e.Graphics.DrawImage(bitmaps[a.Creature.GetImageFileName()], a.Location);
            e.Graphics.ResetTransform();
            e.Graphics.DrawString(game.Scores.ToString(), new Font("Arial", 16), Brushes.Green, 0, 0);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            //Todo вызывать gameState и делать магию
            Invalidate();
        }
    }
}
