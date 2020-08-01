﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MazeGame.Engine
{
    public class Scene
    {
        public readonly List<RenderObject> SceneObjects = new List<RenderObject>();
        public Display Display { get; set; }
        public string Name { get; set; }
        public bool Started { get; set; }

        private readonly List<RenderObject> _objectsToAdd = new List<RenderObject>();
        private readonly List<RenderObject> _objectsToRemove = new List<RenderObject>();

        /// <summary>
        /// Add a render object to the scene objects
        /// </summary>
        /// <param name="renderObject"></param>
        protected void AddRenderObject(RenderObject renderObject)
        {
            renderObject.Scene = this;
            _objectsToAdd.Add(renderObject);
        }

        /// <summary>
        /// Remove a render object from the scene objects
        /// </summary>
        /// <param name="renderObject"></param>
        protected void RemoveRenderObject(RenderObject renderObject)
        {
            _objectsToRemove.Add(renderObject);
        }

        /// <summary>
        /// Apply the additions and removals of render objects all in one go after an update and a render so they will
        /// be reflected next frame and not cause any side effects mid-render / mid-update.
        /// </summary>
        public void ApplySceneChanges()
        {
            _objectsToRemove.ForEach(o => SceneObjects.Remove(o));
            _objectsToRemove.Clear();
            
            _objectsToAdd.ForEach(o => SceneObjects.Add(o));
            _objectsToAdd.Clear();
        }

        /// <summary>
        /// Scene start method. Called when the scene is first loaded
        /// </summary>
        public virtual void Start()
        {
            Debug.WriteLine($"Scene: {Name} Starting");
        }

        /// <summary>
        /// Scene restart method. Called when the scene is re-started after an initial start
        /// </summary>
        public virtual void Restart()
        {
            Debug.WriteLine($"Scene: {Name} Restarting");
        }

        /// <summary>
        /// Scene update method
        /// </summary>
        public virtual void Update(UpdateInfo updateInfo)
        {
            // update all components
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
            {
                // NOTE: Disabled error catching so that i can actually see the error properly in my IDE
                // try
                // {
                    renderObject.Update(updateInfo);
                // }
                // catch (Exception e)
                // {
                //     Debug.WriteLine($"Exception whilst updating: {renderObject} -> {e.Message}");
                //     Debug.WriteLine(e.StackTrace);
                //     throw;
                // }
            }
        }

        /// <summary>
        /// Scene render method
        /// </summary>
        public virtual void Render()
        {
            // render all components
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
            {
                // NOTE: Disabled error catching so that i can actually see the error properly in my IDE
                // try
                // {
                    renderObject.Render();
                // }
                // catch (Exception e)
                // {
                //     Debug.WriteLine($"Exception whilst rendering: {renderObject} -> {e.Message}");
                //     Debug.WriteLine(e.StackTrace);
                //     throw;
                // }
            }
        }
    }
}
