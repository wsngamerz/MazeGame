using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    public class TestScene : Scene
    {
        /// <summary>
        /// the test scene
        /// </summary>
        public TestScene()
        {
            var testSceneLabel = new Label("Test scene", Vector2.Zero);
            testSceneLabel.SetBackgroundColour(Style.BackgroundColor.Grayscale240);
            testSceneLabel.SetForegroundColour(Style.ForegroundColor.White);
            AddRenderObject(testSceneLabel);
            AddRenderObject(new Image("main_menu_logo.json", new Vector2(10, 5)));
        }
    }
}