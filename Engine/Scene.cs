using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MazeGame.Engine
{
    public class Scene
    {
        public List<RenderObject> SceneObjects = new List<RenderObject>();
        public Display Display { get; set; }
        public string Name { get; set; }
        public bool Started { get; set; }

        private List<RenderObject> _objectsToAdd = new List<RenderObject>();
        private List<RenderObject> _objectsToRemove = new List<RenderObject>();

        protected void AddRenderObject(RenderObject renderObject)
        {
            renderObject.Scene = this;
            _objectsToAdd.Add(renderObject);
        }

        protected void RemoveRenderObject(RenderObject renderObject)
        {
            _objectsToRemove.Add(renderObject);
        }

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
                renderObject.Update(updateInfo);
        }

        /// <summary>
        /// Scene render method
        /// </summary>
        public virtual void Render()
        {
            // render all components
            foreach (var renderObject in SceneObjects.Where(renderObject => renderObject.Enabled))
                renderObject.Render();
        }
    }
}
