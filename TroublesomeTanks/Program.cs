using System;

namespace TroublesomeTanks
{

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = (TroublesomeTanks)TroublesomeTanks.Instance())
                game.Run();
        }
    }
}
