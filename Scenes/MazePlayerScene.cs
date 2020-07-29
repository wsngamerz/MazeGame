using System.Linq;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    /// <summary>
    /// Maze player scene
    /// </summary>
    public class MazePlayerScene : Scene
    {
        public MazePlayerScene()
        {
            AddRenderObject(new Border());
            AddRenderObject(new Label("Maze player scene", Vector2.One));
        }
        
        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
                renderObject.Update(updateInfo);
        }

        /// <summary>
        /// Scene render method`
        /// </summary>
        public override void Render()
        {
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
                renderObject.Render();
        }
    }
}
