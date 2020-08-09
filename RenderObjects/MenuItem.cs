namespace MazeGame.RenderObjects
{
    /// <summary>
    /// A menu item. Used internally inside the Menu render component
    /// </summary>
    public class MenuItem
    {
        public string Text { get; }
        private MenuCallback Callback { get; }
        
        /// <summary>
        /// Create a menu item
        /// </summary>
        /// <param name="itemText"></param>
        /// <param name="menuCallback"></param>
        public MenuItem(string itemText, MenuCallback menuCallback = null)
        {
            Text = itemText;
            Callback = menuCallback;
        }

        /// <summary>
        /// Run the callback method that is linked to this menu item
        /// </summary>
        public void RunCallback()
        {
            Callback?.Invoke(); // will only invoke if isn't null
        }
    }
    
    /// <summary>
    /// The menu callback delegate
    /// </summary>
    public delegate void MenuCallback();
}
