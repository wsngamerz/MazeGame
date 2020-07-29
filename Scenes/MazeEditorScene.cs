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
        public MazeEditorScene()
        {
            AddRenderObject(new Border());
            AddRenderObject(new Label("Maze editor scene", Vector2.One));
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
        /// Scene render method
        /// </summary>
        public override void Render()
        {
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
                renderObject.Render();
        }
    }
}
