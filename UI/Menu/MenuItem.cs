namespace MazeGame.UI.Menu
{
    /// <summary>
    /// an item displayed in a menu
    /// </summary>
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

        /// <summary>
        /// Called when the menu item is selected
        /// </summary>
        /// <param name="screenBuffer"></param>
        public void Select(ScreenBuffer screenBuffer)
        {
            _menuCallback?.Invoke(screenBuffer);
        }
    }

    /// <summary>
    /// called when the menu item os selected
    /// </summary>
    /// <param name="screenBuffer"></param>
    public delegate void MenuCallback(ScreenBuffer screenBuffer);
}