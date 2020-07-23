using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MazeGame
{
    internal static class Program
    {
        private const int ApplicationWidth = 180;
        private const int ApplicationHeight = 50;

        /// <summary>
        /// Entry-point into the application
        ///
        /// bootstraps some basic settings and sets up some platform specific features aka
        /// fixes windows!
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // change some windows specific options to enable ansi escape sequences
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                WindowsSetup.SetupConsole();
            }

            // disable the cursor visibility
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Default;
            
            // set the window and the buffer size to be the same to disable the scrollbar
            Console.SetWindowSize(ApplicationWidth, ApplicationHeight);
            Console.SetBufferSize(ApplicationWidth, ApplicationHeight);

            // create directory if it doesn't exist
            if (!Directory.Exists("mazes")) Directory.CreateDirectory("mazes");
            
            // start the actual game
            var mazeGame = new MazeGame();
            mazeGame.Start();
        }
    }
}