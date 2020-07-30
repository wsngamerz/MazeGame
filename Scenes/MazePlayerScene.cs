using System;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    /// <summary>
    /// Maze player scene
    /// </summary>
    public class MazePlayerScene : Scene
    {
        /// <summary>
        /// Scene start method
        /// </summary>
        public override void Start()
        {
            base.Start();
            
            AddRenderObject(new Border());
            AddRenderObject(new Label("Maze player scene", Vector2.One));
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
