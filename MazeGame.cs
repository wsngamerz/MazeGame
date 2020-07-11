using MazeGame.UI.Menu;

namespace MazeGame
{
    public class MazeGame
    {
        private readonly ScreenBuffer _screenBuffer = new ScreenBuffer();
        private Menu _mainMenu = new Menu("Maze Game");
        private Menu _playMenu = new Menu("Play");
        private Menu _editorMenu = new Menu("Editor");
        private Menu _optionsMenu = new Menu("Options");

        /// <summary>
        /// Setup 
        /// </summary>
        public MazeGame()
        {
            SetupMainMenu();
            SetupPlayMenu();
            SetupEditorMenu();
            SetupOptionsMenu();
        }

        private void SetupMainMenu()
        {
            _mainMenu.AddItem(new MenuItem("Play", _playMenu));
            _mainMenu.AddItem(new MenuItem("Editor", _editorMenu));
            _mainMenu.AddItem(new MenuItem("Options", _optionsMenu));
            _mainMenu.AddItem(new MenuItem("Quit", Quit));
        }

        private void SetupPlayMenu()
        {
            _playMenu.AddItem(new MenuItem("Back", _mainMenu));
        }

        private void SetupEditorMenu()
        {
            _editorMenu.AddItem(new MenuItem("Back", _mainMenu));
        }

        private void SetupOptionsMenu()
        {
            _optionsMenu.AddItem(new MenuItem("Back", _mainMenu));
        }

        public void Start()
        {
            _mainMenu.Show(_screenBuffer);
        }

        private static void Quit(ScreenBuffer screenBuffer)
        {
            System.Environment.Exit(0);
        } 
    }
}