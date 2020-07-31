using System;
using System.Linq;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    /// <summary>
    /// The maze editor scene
    /// </summary>
    public class MazeEditorScene : Scene
    {
        private Border _editorBorder;
        
        /// <summary>
        /// scene start method
        /// </summary>
        public override void Start()
        {
            base.Start();
            
            var borderSections = new[]
            {
                new BorderSection(Vector2.Zero, new Vector2((Display.Width - 1) / 2, 6)),
                new BorderSection(((Display.Width - 1) / 2) - 1, 0, (Display.Width - (Display.Width - 1) / 2) + 1, 6) 
            };

            _editorBorder = new Border(borderSections);
            
            AddRenderObject(_editorBorder);
            AddRenderObject(new Label("Maze editor scene", Vector2.One));
        }

        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            base.Update(updateInfo);
            
            // back to main menu
            if (updateInfo.PressedKeys.Select(pk => pk.Key).Contains(ConsoleKey.Escape)) Display.SwitchScene("mainMenu");

            // re-calculate sizes for the border information
            if (!updateInfo.HasResized) return;
            _editorBorder.BorderSections[0].Size = new Vector2((Display.Width - 1) / 2, 6);
            _editorBorder.BorderSections[1].Position = new Vector2(((Display.Width - 1) / 2) - 1, 0);
            _editorBorder.BorderSections[1].Size = new Vector2((Display.Width - (Display.Width - 1) / 2) + 1, 6);
        }
    }
}
