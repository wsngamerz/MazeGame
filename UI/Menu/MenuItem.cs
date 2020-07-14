namespace MazeGame.UI.Menu
{
    public class MenuItem
    {
        private readonly MenuCallback _menuCallback;
        public string Text { get; }
        public bool HasCallback => _menuCallback != null;

        public MenuItem(string menuText)
        {
            Text = menuText;
        }
        
        public MenuItem(string menuText, MenuCallback menuCallback)
        {
            Text = menuText;
            _menuCallback = menuCallback;
        }

        public MenuItem(string menuText, Menu menu)
        {
            Text = menuText;
            _menuCallback = menu.Show;
        }

        public void Select(ScreenBuffer screenBuffer)
        {
            _menuCallback?.Invoke(screenBuffer);
        }
    }

    public delegate void MenuCallback(ScreenBuffer screenBuffer);
}