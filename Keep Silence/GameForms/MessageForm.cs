using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Keep_Silence.GameForms
{
    public class MessageForm : Form
    {
        public MessageForm(string message, Dictionary<string, Bitmap> menuBitmaps)
        {            
            DoubleBuffered = true;
            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width/ 2, Screen.PrimaryScreen.Bounds.Height/2);
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImage = menuBitmaps["MessageBackground.png"];
            BackgroundImageLayout = ImageLayout.Stretch;
            AllowTransparency = true;
            
            var text = new Label()
            {
                Font = new Font("Comic Sans MS", 12),
                ForeColor = Color.Black,
                Location = new Point(30, ClientSize.Height/7),
                Size = new Size(ClientSize.Width - 50, ClientSize.Height/2),
                Text = Wrap(message, ClientSize.Width - 50),
                BackColor = Color.Transparent,
            };
            Controls.Add(text);
            
            var okButton = new Button
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Image = menuBitmaps["OkButton.png"],
                Location = new Point(ClientSize.Width / 3, ClientSize.Height - 2 * ClientSize.Height / 6),
                Size = new Size(ClientSize.Width / 3, ClientSize.Height / 6)
            };
            okButton.Click += (sender, args) => Close();
            Controls.Add(okButton);
        }

        private static string Wrap(string text, int max)
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
