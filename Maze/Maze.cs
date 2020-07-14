using System.Collections.Generic;

namespace MazeGame.Maze
{
    /// <summary>
    /// The maze data class
    /// </summary>
    public class Maze
    {
        public string Name;
        public int Width;
        public int Height;
        public readonly List<MapTile> Map;

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new List<MapTile>();
        }
    }
}