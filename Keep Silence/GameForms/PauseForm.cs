using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Keep_Silence
{
    public sealed class PauseForm : Form
    {
        public PauseForm(Dictionary<string,Bitmap> menuBitmaps, Game game)
        {
            DoubleBuffered = true;
            ClientSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            WindowState = FormWindowState.Maximized;
            BackgroundImage = menuBitmaps["PauseBackground.png"];

            var resumeButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = menuBitmaps["ResumeButton.png"],
                Location = new Point(2 * ClientSize.Width / 5, 3 * ClientSize.Height / 9),
                Size = new Size(ClientSize.Width / 5, ClientSize.Height / 10)
            };
            resumeButton.Click += (sender, args) => Close();

            var backToMainMenuButton = new Button
            {
                Size = resumeButton.Size,
                Image = menuBitmaps["BackToMainMenuButton.png"],
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(resumeButton.Left, resumeButton.Bottom + ClientSize.Height/9),
               
            };
            backToMainMenuButton.Click += (sender, args) =>
            {
                game.GameOver();
                Close();
            };

            Controls.Add(resumeButton);
            Controls.Add(backToMainMenuButton);
        }
    }
}
