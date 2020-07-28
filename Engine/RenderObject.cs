namespace MazeGame.Engine
{
    /// <summary>
    /// Abstract RenderObject class which all objects inherit from
    /// </summary>
    public abstract class RenderObject
    {
        /// <summary>
        /// The X,Y Coordinates of the render object
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// The Width and Height of the render object 
        /// </summary>
        public Vector2 Size { get; set; }
        
        /// <summary>
        /// The ZIndex of the render object. A larger indexed object will render on top of a smaller indexed object
        /// </summary>
        public int ZIndex { get; set; }
        
        /// <summary>
        /// A name given to a render object. Can be used to get the object later.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Whether the update and render methods will be called.
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// The render result: Used internally
        /// </summary>
        public string[] Content { get; set; }
        
        /// <summary>
        /// the scene that the render object is in
        /// </summary>
        public Scene Scene { get; set; }

        /// <summary>
        /// Method called before a render
        ///
        /// Includes all keypresses that have occured between frames
        /// </summary>
        public abstract void Update(UpdateInfo updateInfo);
        
        /// <summary>
        /// Method called to render the object into a string
        /// </summary>
        public abstract void Render();
    }
}