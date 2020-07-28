using System.Collections.Generic;

namespace MazeGame.Engine
{
    public class Scene
    {
        public List<RenderObject> SceneObjects = new List<RenderObject>();
        public Display Display { get; set; }
        public string Name { get; set; }

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
    }
}