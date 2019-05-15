using System.Drawing;
using System.Windows.Forms;

namespace Keep_Silence
{
    public sealed class MainMenuForm : Form 
    {
        public MainMenuForm()
        {
            var game = new Game();
            game.LoadRooms();
            var keepSilenceForm = new KeepSilenceForm(game);
            DoubleBuffered = true;
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackgroundImage = keepSilenceForm.menuBitmaps["MainMenuBackground.png"];
            BackgroundImageLayout = ImageLayout.Stretch;

            var startGameButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = keepSilenceForm.menuBitmaps["StartGameButton.png"],
                Location = new Point(2 * ClientSize.Width / 5, 3 * ClientSize.Height / 9),
                Size = new Size(ClientSize.Width / 5, ClientSize.Height / 10)
            };
            startGameButton.Click += (sender, args) =>
            {
                game = new Game();
                game.LoadRooms();
                keepSilenceForm = new KeepSilenceForm(game);
                keepSilenceForm.ShowDialog();
            };

            var exitButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = keepSilenceForm.menuBitmaps["ExitButton.png"],
                Location = new Point(startGameButton.Left, startGameButton.Bottom + ClientSize.Height / 9),
                Size = startGameButton.Size
            };
            exitButton.Click += (sender, args) => Close();

            Controls.Add(startGameButton);
            Controls.Add(exitButton);
        }
    }
}
