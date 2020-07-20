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

        public Maze(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
            Map = new List<MapTile>();
        }
    }
}