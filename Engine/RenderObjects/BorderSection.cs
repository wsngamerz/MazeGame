namespace MazeGame.Engine.RenderObjects
{
    /// <summary>
    /// A border section is a specific section within a larger border that can be used to draw out grids of sorts
    /// </summary>
    public struct BorderSection
    {
        /// <summary>
        /// The topleft corner of the border section
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// the width and height of the border section
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Create a border section from a size and position Vector2
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        public BorderSection(Vector2 pos, Vector2 size)
        {
            Position = pos;
            Size = size;
        }
    }
}