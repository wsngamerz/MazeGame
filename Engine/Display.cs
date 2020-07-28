using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MazeGame.Engine
{
    public class Display
    {
        public List<Scene> Scenes;
        public Scene CurrentScene;

        public const int Fps = 10;
        public int Width;
        public int Height;

        private string[] _currentFrame;
        private string[] _prevFrame;

        public Display()
        {
            Scenes = new List<Scene>();
            PopulateFrame();
        }
        
        public void AddScene(Scene scene, string sceneName)
        {
            scene.Display = this; // attach display to the scene
            scene.Name = sceneName;
            if (Scenes.Count == 0) CurrentScene = scene;
            Scenes.Add(scene);
        }

        public void SetScene(Scene scene)
        {
            if (!Scenes.Contains(scene)) return;

            CurrentScene = scene;
        }

        public void SwitchScene(string nextSceneName)
        {
            var newScene = Scenes.FirstOrDefault(s => s.Name == nextSceneName);
            if (newScene != null)
            {
                CurrentScene = newScene;
            }
        }
        
        private void SetupSize()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            Console.Clear();
            Console.CursorVisible = false;
            Debug.WriteLine($"Setting display size to ({Width}, {Height})");
        }

        private void PopulateFrame()
        {
            _currentFrame = new string[Height];
            for (var i = 0; i < _currentFrame.Length; i++)
            {
                _currentFrame[i] = new string(' ', Width);
            }
        }
        
        private void Draw()
        {
            // create updateinfo object
            var updateInfo = new UpdateInfo()
            {
                PressedKeys = new List<ConsoleKey>(),
                HasResized = Width != Console.WindowWidth || Height != Console.WindowHeight
            };

            // update if resized
            if (updateInfo.HasResized) SetupSize();
            
            // get all keys pressed
            while (Console.KeyAvailable) updateInfo.PressedKeys.Add(Console.ReadKey(true).Key);

            // move the frame of last draw to prev
            _prevFrame = _currentFrame;
            
            // create an empty new frame
            PopulateFrame();

            // update and render each object
            foreach (var renderObject in CurrentScene.SceneObjects)
            {
                // call update and render on each object
                renderObject.Update(updateInfo);
                renderObject.Render();
                
                // get the renders for all the objects and combine into 1 2d array
                for (var dy = 0; dy < renderObject.Content.Length; dy++)
                {
                    _currentFrame[renderObject.Position.Y + dy] = _currentFrame[renderObject.Position.Y + dy]
                        .Remove(renderObject.Position.X, Regex.Replace(renderObject.Content[dy], "\\u001b[^m]*m", "").Length)
                        .Insert(renderObject.Position.X, renderObject.Content[dy]);
                }
            }
            
            // let the scene add and remove any objects now
            CurrentScene.ApplySceneChanges();
            
            // check whether we actually need to write to the console as if the frame is identical, we can just
            // skip the call
            if (_currentFrame.SequenceEqual(_prevFrame)) return;
            
            // 1 draw call
            Console.SetCursorPosition(0, 0);
            Console.Write(string.Join("", _currentFrame));
        }

        public void StartRendering()
        {
            var drawTimer = new Stopwatch();
            
            while (true)
            {
                // call the draw method and time it
                drawTimer.Restart();
                Draw();
                drawTimer.Stop();
                
                // calculate the time taken to draw this "frame"
                float freeTime = (1000f / Fps) - drawTimer.ElapsedMilliseconds;
                if (freeTime >= 0)
                {
                    Thread.Sleep((int) freeTime);
                }
                else
                {
                    Debug.WriteLine($"Running behind by {Convert.ToString(-freeTime)}ms");
                }
            }
        }
    }
}