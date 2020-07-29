using System;

namespace MazeGame.Engine
{
    /// <summary>
    /// A Vector2 Class
    /// </summary>
    public class Vector2
    {
        public readonly int X;
        public readonly int Y;

        /// <summary>
        /// create vector2 class with x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Common vectors here so they don't need to be recreated
        
        /// <summary>
        /// A Zero Vector2
        /// </summary>
        public static readonly Vector2 Zero = new Vector2(0, 0);

        /// <summary>
        /// Convert a vector2 object to a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"({X.ToString()}, {Y.ToString()})";
        
        /// <summary>
        /// Whether 2 vectors have the same x and y values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Vector2 other && (X == other.X && Y == other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
