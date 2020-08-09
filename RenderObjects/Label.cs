using MazeGame.Engine;

namespace MazeGame.RenderObjects
{
    /// <summary>
    /// A simple label render object
    /// </summary>
    public class Label : RenderObject
    {
        public string Text;
        public string ForegroundColour;
        public string BackgroundColour;
        
        /// <summary>
        /// create a label
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        public Label(string text, Vector2 position)
        {
            Text = text;
            Position = position;
            Enabled = true;
            Size = new Vector2(text.Length, 1);
        }

        /// <summary>
        /// sets the foreground colour
        /// </summary>
        /// <param name="foregroundColour"></param>
        public void SetForegroundColour(string foregroundColour)
        {
            ForegroundColour = foregroundColour;
        }

        /// <summary>
        /// sets the background colour
        /// </summary>
        /// <param name="backgroundColour"></param>
        public void SetBackgroundColour(string backgroundColour)
        {
            BackgroundColour = backgroundColour;
        }

        /// <summary>
        /// an update method which isnt used as a label
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo) { }

        /// <summary>
        /// the render method of the label
        /// </summary>
        public override void Render()
        {
            Content = new[] {$"{BackgroundColour}{ForegroundColour}{Text}{Style.Reset}"};
        }
    }
}
