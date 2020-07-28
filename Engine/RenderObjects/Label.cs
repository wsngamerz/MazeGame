using System;
using System.Collections.Generic;

namespace MazeGame.Engine.RenderObjects
{
    public class Label : RenderObject
    {
        public string Text;
        public string ForegroundColour;
        public string BackgroundColour;
        
        public Label(string text, Vector2 position)
        {
            Text = text;
            Position = position;
            Size = new Vector2(text.Length, 1);
        }

        public void SetForegroundColour(string foregroundColour)
        {
            ForegroundColour = foregroundColour;
        }

        public void SetBackgroundColour(string backgroundColour)
        {
            BackgroundColour = backgroundColour;
        }

        public override void Update(UpdateInfo updateInfo) { }

        public override void Render()
        {
            Content = new[] {$"{BackgroundColour}{ForegroundColour}{Text}{Style.Reset}"};
        }
    }
}