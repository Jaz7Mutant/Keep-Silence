using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Keep_Silence.GameForms
{
    public sealed class GameOverForm : Form
    {
        private string deadMessage = "Так заканчивается твоя история. Ты умер и о тебе никто никогда не вспомнит.";

        public GameOverForm(Dictionary<string, Bitmap> menuBitmaps, string message = null)
        {
            DoubleBuffered = true;
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackgroundImage = menuBitmaps["GameOverBackground.png"];
            BackgroundImageLayout = ImageLayout.Stretch;
            if (message == null)
            {
                message = deadMessage;
            }
            var text = new Label()
            {
                Font = new Font("Comic Sans MS", 20),
                ForeColor = Color.Azure,
                Location = new Point(ClientSize.Width/3, ClientSize.Height / 3),
                Size = new Size(ClientSize.Width/3, ClientSize.Height / 3),
                Text = Wrap(message, ClientSize.Width/3),
                BackColor = Color.Transparent,
            };
            Controls.Add(text);

            var backToMainMenuButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = menuBitmaps["BackToMainMenuButton.png"],
                Location = new Point(ClientSize.Width / 9 * 4, text.Bottom),
                Size = new Size(ClientSize.Width / 9, ClientSize.Height / 10),
                FlatStyle = FlatStyle.Popup
            };
            backToMainMenuButton.Click += (sender, args) => Close();
            Controls.Add(backToMainMenuButton);
        }

        public static string Wrap(string text, int max)
        {
            var charCount = 0;
            var lines = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var newLines = lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                                                                ? max - (charCount % max) : 0) + w.Length + 1) / max)
                .Select(g => string.Join(" ", g.ToArray()));
            var result = "";
            foreach (var newLine in newLines)
            {
                result += newLine;
                result += "\r\n";
            }

            return result;
        }
    }
}
