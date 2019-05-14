using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Keep_Silence
{
    public sealed class MainMenuForm : Form 
    {
        public MainMenuForm(Dictionary<string,Bitmap> menuBitmaps, Form KSF)
        {
            DoubleBuffered = true;
            ClientSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            WindowState = FormWindowState.Maximized;
            BackgroundImage = menuBitmaps["MainMenuBackground.png"];

            var startGameButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = menuBitmaps["StartGameButton.png"],
                Location = new Point(2 * ClientSize.Width / 5, 3 * ClientSize.Height / 9),
                Size = new Size(ClientSize.Width / 5, ClientSize.Height / 10)
            };
            startGameButton.Click += (sender, args) => KSF.ShowDialog();

            var exitButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = menuBitmaps["ExitButton.png"],
                Location = new Point(startGameButton.Left, startGameButton.Bottom + ClientSize.Height / 9),
                Size = startGameButton.Size
            };
            exitButton.Click += (sender, args) => Close();

            Controls.Add(startGameButton);
            Controls.Add(exitButton);
        }
    }
}
