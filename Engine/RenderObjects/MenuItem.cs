namespace MazeGame.Engine.RenderObjects
{
    public class MenuItem
    {
        public string Text { get; }
        private MenuCallback Callback { get; }
        
        public MenuItem(string itemText, MenuCallback menuCallback = null)
        {
            Text = itemText;
            Callback = menuCallback;
        }

        public void RunCallback()
        {
            Callback?.Invoke();
        }
    }
    
    public delegate void MenuCallback();
}