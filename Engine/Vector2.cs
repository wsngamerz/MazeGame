using System;

namespace MazeGame.Engine
{
    public class Vector2
    {
        public readonly int X;
        public readonly int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly Vector2 Zero = new Vector2(0, 0);

        public override string ToString() => $"({X.ToString()}, {Y.ToString()})";
        
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