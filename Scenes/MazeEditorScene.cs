using System;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    /// <summary>
    /// The maze editor scene
    /// </summary>
    public class MazeEditorScene : Scene
    {
        /// <summary>
        /// scene start method
        /// </summary>
        public override void Start()
        {
            base.Start();
            
            var borderSections = new[]
            {
                new BorderSection(Vector2.One, new Vector2(Display.Width, 6)),
            };
            
            AddRenderObject(new Border(borderSections));
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
            if (updateInfo.PressedKeys.Contains(ConsoleKey.Escape)) Display.SwitchScene("mainMenu");
        }
    }
}
