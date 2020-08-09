using System.Numerics;
using MazeGame.Engine;

namespace MazeGame.RenderObjects
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

        /// <summary>
        /// Create a border section from x,y coords and width, height values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public BorderSection(int x, int y, int dx, int dy) : this(new Vector2(x, y), new Vector2(dx, dy)) { }

        public override string ToString()
        {
            return $"BorderSection(x: {Position.X} y: {Position.Y} w: {Size.X} h: {Size.Y})";
        }
    }
}
