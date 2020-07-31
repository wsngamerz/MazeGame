﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace MazeGame.Engine
{
    /// <summary>
    /// The display class that handles drawing all of the scenes and their render objects content to the screen. Also
    /// handles passing inputs to each scene.
    /// </summary>
    public class Display
    {
        public List<Scene> Scenes;
        public Scene CurrentScene;
        
        // a rudimentary FPS Limit
        public const int Fps = 20;
        
        // the current size of the display
        public int Width;
        public int Height;

        // frame arrays that are used to compare to see whether we actually need to draw
        private string[] _currentFrame;
        private string[] _prevFrame;

        private bool _isRendering;

        /// <summary>
        /// Create the display object
        /// </summary>
        public Display()
        {
            // cre
            Scenes = new List<Scene>();
            _isRendering = true;
            _currentFrame = PopulateFrame();
        }
        
        /// <summary>
        /// Add a scene for the display to manage
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneName"></param>
        public void AddScene(Scene scene, string sceneName)
        {
            scene.Display = this; // attach display to the scene
            scene.Name = sceneName;
            if (Scenes.Count == 0) CurrentScene = scene;
            Scenes.Add(scene);
        }

        /// <summary>
        /// Set the current scene
        /// </summary>
        /// <param name="scene"></param>
        private void SetScene(Scene scene)
        {
            if (!Scenes.Contains(scene)) return;
            CurrentScene = scene;
        }

        /// <summary>
        /// Switch from a scene to another scene
        /// </summary>
        /// <param name="nextSceneName"></param>
        public void SwitchScene(string nextSceneName)
        {
            SetScene(Scenes.FirstOrDefault(s => s.Name == nextSceneName));

            if (CurrentScene.Started)
            {
                // trigger the restart method if the scene has previously started
                CurrentScene.Restart();
            }
            else
            {
                // otherwise start the scene for the first time
                CurrentScene.Start();
                CurrentScene.Started = true;
            }
        }
        
        /// <summary>
        /// Handle a change in size of the console or setup the initial values
        /// </summary>
        private void SetupSize()
        {
            Width = Console.WindowWidth;
            Height = Console.WindowHeight;
            Console.Clear();
            Console.CursorVisible = false;
            Debug.WriteLine($"Setting display size to ({Width}, {Height})");
        }

        /// <summary>
        /// populates a 1d string array with blank values for a blank frame
        /// </summary>
        public string[] PopulateFrame()
        {
            var newFrame = new string[Height];
            for (var i = 0; i < newFrame.Length; i++)
            {
                newFrame[i] = new string(' ', Width);
            }

            return newFrame;
        }
        
        /// <summary>
        /// Draw a frame
        /// </summary>
        private void Draw()
        {
            // create updateInfo object
            var updateInfo = new UpdateInfo()
            {
                PressedKeys = new List<ConsoleKeyInfo>(),
                HasResized = Width != Console.WindowWidth || Height != Console.WindowHeight
            };

            // update if resized
            if (updateInfo.HasResized) SetupSize();
            
            // get all keys pressed
            while (Console.KeyAvailable) updateInfo.PressedKeys.Add(Console.ReadKey(true));

            // move the frame of last draw to prev
            _prevFrame = _currentFrame;
            
            // create an empty new frame
            _currentFrame = PopulateFrame();

            // trigger an update and a render of the current scene
            CurrentScene.Update(updateInfo);
            CurrentScene.Render();
            
            // get the renders for all the objects and combine into 1 2d array
            foreach (var renderObject in CurrentScene.SceneObjects.OrderBy(s => s.ZIndex))
            {
                // iterate through each line of the render
                for (var dy = 0; dy < renderObject.Content.Length; dy++)
                {
                    // use substrings to remove the required amount of space and fill it with the content
                    try
                    {
                        _currentFrame[renderObject.Position.Y + dy] = _currentFrame[renderObject.Position.Y + dy]
                            .Remove(renderObject.Position.X,
                                Regex.Replace(renderObject.Content[dy], "\\u001b[^m]*m", "").Length)
                            .Insert(renderObject.Position.X, renderObject.Content[dy]);
                    }
                    catch (Exception e)
                    {
                        // exceptions that will be thrown if resize console mid-draw. These can be safely skipped
                        // furthermore, if a render object is too large to fir on screen, that will also trigger this
                        if (e is IndexOutOfRangeException || e is ArgumentOutOfRangeException) continue;
                        
                        // throw if any other exception happens
                        throw;
                    }
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

        /// <summary>
        /// Start rendering for the application. This is basically the main game loop
        /// </summary>
        public void StartRendering(string startingScene)
        {
            // because this will be the first loop, trigger the start method for the current scene manually
            SwitchScene(startingScene);
            
            var drawTimer = new Stopwatch();
            
            while (_isRendering)
            {
                // call the draw method and time it
                drawTimer.Restart();
                Draw();
                drawTimer.Stop();
                
                // calculate the approximate time taken to draw this "frame"
                float freeTime = (1000f / Fps) - drawTimer.ElapsedMilliseconds;
                
                // sleep the current thread if we have enough remaining time to keep the framerate constant
                if (freeTime >= 0) Thread.Sleep((int) freeTime);
                else Debug.WriteLine($"Running behind by {Convert.ToString(-freeTime)}ms");
            }
        }

        /// <summary>
        /// Called on shutdown
        /// </summary>
        public void StopRendering()
        {
            _isRendering = false;
            Debug.WriteLine("Application shutting down");
            Environment.Exit(0);
        }
    }
}
