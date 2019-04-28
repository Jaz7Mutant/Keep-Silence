using System;
using System.Windows.Forms;

namespace Keep_Silence
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var game = new Game();
            game.CreateRoomList();
            Application.Run(new KeepSilenceForm());
        }
    }
}
