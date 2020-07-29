using System;
using System.Linq;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    public class TestScene : Scene
    {
        /// <summary>
        /// Scene start method
        /// </summary>
        public override void Start()
        {
            var testSceneLabel = new Label("Test scene", Vector2.One);
            testSceneLabel.SetBackgroundColour(Style.BackgroundColor.Grayscale240);
            testSceneLabel.SetForegroundColour(Style.ForegroundColor.White);
            AddRenderObject(testSceneLabel);
            
            AddRenderObject(new Border());
            AddRenderObject(new Image("main_menu_logo.json", new Vector2(10, 5)));
        }

        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            // back to main menu
            if (updateInfo.PressedKeys.Contains(ConsoleKey.Escape)) Display.SwitchScene("mainMenu");
            
            // update all components
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
