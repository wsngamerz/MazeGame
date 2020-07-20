using System;
using MazeGame.Maze;
using MazeGame.UI.Menu;

namespace MazeGame
{
    public class MazeGame
    {
        private const string Version = "0.0.1.0 ALPHA";
        private static readonly string[] MenuLogo = new[]
        {
            "███╗   ███╗ █████╗ ███████╗███████╗      ██████╗  █████╗ ███╗   ███╗███████╗",
            "████╗ ████║██╔══██╗╚══███╔╝██╔════╝     ██╔════╝ ██╔══██╗████╗ ████║██╔════╝",
            "██╔████╔██║███████║  ███╔╝ █████╗       ██║  ███╗███████║██╔████╔██║█████╗  ",
            "██║╚██╔╝██║██╔══██║ ███╔╝  ██╔══╝       ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  ",
            "██║ ╚═╝ ██║██║  ██║███████╗███████╗     ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗",
            "╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝      ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝"
        };
        
        private readonly ScreenBuffer _screenBuffer = new ScreenBuffer();
        
        // menus
        private readonly Menu _mainMenu = new Menu();
        private readonly Menu _playMenu = new Menu("Play");
        private readonly Menu _editorMenu = new Menu("Editor");
        private readonly Menu _optionsMenu = new Menu("Options");
        
        // Sections of the game
        private MazeEditor _mazeEditor;
        private MazePlayer _mazePlayer;

        /// <summary>
        /// Main maze game class
        /// </summary>
        public MazeGame()
        {
            // setup all of the menus
            SetupMainMenu();
            SetupPlayMenu();
            SetupEditorMenu();
            SetupOptionsMenu();
        }

        /// <summary>
        /// add the items for the main menu
        /// </summary>
        private void SetupMainMenu()
        {
            _mainMenu.AddItem("Play", _playMenu);
            _mainMenu.AddItem("Editor", _editorMenu);
            _mainMenu.AddItem("Options", _optionsMenu);
            _mainMenu.AddItem("!!! Test Screen !!!", TestScreen);
            _mainMenu.AddItem("Quit", Quit);
        }

        /// <summary>
        /// add the items for the play menu
        /// </summary>
        private void SetupPlayMenu()
        {
            _playMenu.AddItem("Select Level"); // built in levels
            _playMenu.AddItem("Custom Levels"); // users own levels made in the editor
            _playMenu.AddItem("Back", _mainMenu);
        }

        /// <summary>
        /// add the items for the editor menu
        /// </summary>
        private void SetupEditorMenu()
        {
            _editorMenu.AddItem("Create new Maze", CreateNewMazeScreen);
            _editorMenu.AddItem("Edit Maze");
            _editorMenu.AddItem(new MenuItem("Back", _mainMenu));
        }

        /// <summary>
        /// add the items for the options menu
        /// </summary>
        private void SetupOptionsMenu()
        {
            _optionsMenu.AddItem("Display Options");
            _optionsMenu.AddItem(new MenuItem("Back", _mainMenu));
        }

        /// <summary>
        /// start the game
        /// </summary>
        public void Start()
        {
            _screenBuffer.AddConstantRender(_screenBuffer.BufferWidth / 2 - MenuLogo[0].Length / 2, 10, MenuLogo);
            _screenBuffer.AddConstantRender(2, _screenBuffer.BufferHeight - 2, $"v{Version} - Created by William Neild");
            
            _mainMenu.Show(_screenBuffer);
        }

        /// <summary>
        /// Displays the test screen
        /// </summary>
        /// <param name="screenBuffer"></param>
        private static void TestScreen(ScreenBuffer screenBuffer)
        {
            // clear buffer and the constant render queue
            screenBuffer.ClearRenderQueue();
            screenBuffer.ClearBuffer();
            
            // show the test screen
            new TestScreen(screenBuffer).Show();
        }

        /// <summary>
        /// opens the map editor for a new maze
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void CreateNewMazeScreen(ScreenBuffer screenBuffer)
        {
            // Stop the menu from rendering
            screenBuffer.ClearRenderQueue();
            screenBuffer.ClearBuffer();
            
            // TODO: Add a user input for the name of a maze
            var mazeName = Guid.NewGuid().ToString();
            
            // create a new maze editor with a new maze
            // and show it
            _mazeEditor = new MazeEditor(mazeName);
            _mazeEditor.Show(screenBuffer);
        }

        /// <summary>
        /// quit the game
        /// </summary>
        /// <param name="screenBuffer">screen buffer passed from the menu</param>
        private static void Quit(ScreenBuffer screenBuffer)
        {
            Environment.Exit(0);
        } 
    }
}