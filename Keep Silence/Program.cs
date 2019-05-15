using System;
using System.Windows.Forms;

namespace Keep_Silence
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.Run(new MainMenuForm());
        }
    }
}
