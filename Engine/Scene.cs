using System.Collections.Generic;

namespace MazeGame.Engine
{
    public abstract class Scene
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
        public abstract void Start();

        /// <summary>
        /// Scene update method
        /// </summary>
        public abstract void Update(UpdateInfo updateInfo);

        /// <summary>
        /// Scene render method
        /// </summary>
        public abstract void Render();
    }
}
