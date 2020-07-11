using System;
using System.Runtime.InteropServices;

namespace MazeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // change some windows specific options to enable ansi escape sequences
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                WindowsSetup.SetupConsole();
            }

            // disable the cursor visibility
            Console.CursorVisible = false;
            Console.SetWindowSize(180, 50);
            
            var mazeGame = new MazeGame();
            mazeGame.Start();
        }
    }
}