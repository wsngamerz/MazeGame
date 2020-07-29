using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGame.Engine.RenderObjects
{
    /// <summary>
    /// A render component which can draw borders based upon coordinates
    /// </summary>
    public class Border : RenderObject
    {
        private bool _needsRender;
        private BorderSection[] _borderSections;

        public Border(IEnumerable<BorderSection> borderSections = null)
        {
            Position = Vector2.Zero;
            Enabled = true;
            ZIndex = -100; // draw right at the back so that all content will be on top
            _needsRender = true;
            
            if (borderSections != null) _borderSections = borderSections.ToArray();
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            if (!_needsRender && updateInfo.HasResized) _needsRender = true;
        }

        /// <summary>
        /// render method
        /// </summary>
        public override void Render()
        {
            if (!_needsRender) return;
            
            string[] render = Scene.Display.PopulateFrame();
            Size = new Vector2(Scene.Display.Width, Scene.Display.Height);

            // render the basic border outline around the whole screen
            render[0] = $"{Character.TopLeft}{Utils.Repeat(Character.Horizontal, Size.X - 2)}{Character.TopRight}";
            render[Size.Y - 1] = $"{Character.BottomLeft}{Utils.Repeat(Character.Horizontal, Size.X - 2)}{Character.BottomRight}";
            for (var i = 0; i < Size.Y - 2; i++)
            {
                render[i + 1] = $"{Character.Vertical}{Utils.Repeat(Character.Empty, Size.X - 2)}{Character.Vertical}";
            }

            // TODO: Implement border sections
            if (_borderSections != null)
            {
                foreach (var borderSection in _borderSections)
                {
                    
                }   
            }

            _needsRender = false;
            Content = render;
        }
    }
}
