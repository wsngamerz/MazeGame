namespace MazeGame.UI.Menu
{
    public class MenuItem
    {
        private MenuCallback _menuCallback;
        public string Text { get; }

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
            _menuCallback(screenBuffer);
        }
    }

    public delegate void MenuCallback(ScreenBuffer screenBuffer);
}