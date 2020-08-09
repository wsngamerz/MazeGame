using System;
using System.Linq;
using System.Numerics;
using MazeGame.Engine;
using MazeGame.RenderObjects;

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
            if (updateInfo.PressedKeys.Select(pk => pk.Key).Contains(ConsoleKey.Escape)) Display.SwitchScene("mainMenu");
        }
    }
}
