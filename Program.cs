using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MazeGame.Engine;
using MazeGame.Scenes;

namespace MazeGame
{
    internal static class Program
    {
        private static Display _display;
        
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

            // create directory if it doesn't exist
            if (!Directory.Exists("mazes")) Directory.CreateDirectory("mazes");
            
            // setup the display
            _display = new Display();
            _display.AddScene(new MainMenuScene(), "mainMenuScene"); // first scene added will be the current scene
            _display.AddScene(new TestScene(), "testScene");
            _display.AddScene(new MazePlayerScene(), "mazePlayer");
            _display.AddScene(new MazeEditorScene(), "mazeEditor");
            
            // start rendering the first scene
            _display.StartRendering();
        }
    }
}
