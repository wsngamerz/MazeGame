using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace MazeGame.Maze
{
    /// <summary>
    /// The maze data class
    /// </summary>
    [Serializable]
    public class Maze
    {
        public string ID;
        public readonly string Name;
        public int Width;
        public int Height;
        public List<MapTile> Map;

        public Maze(string name)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Map = new List<MapTile>();

            if (string.IsNullOrWhiteSpace(Name)) Name = ID;
        }

        /// <summary>
        /// Save the maze
        /// </summary>
        public void Save()
        {
            if (!Directory.Exists("mazes")) Directory.CreateDirectory("mazes");
            
            var binaryFormatter = new BinaryFormatter();
            var fileStream = File.Create($"mazes/{ID}.maze");
            binaryFormatter.Serialize(fileStream, this);
            fileStream.Close();
        }

        /// <summary>
        /// Gets the boundary tiles and calculates the minimum width and height the maze has to be to hold the maze and
        /// then will shrink the maze coordinates appropriately.
        /// </summary>
        public void ResizeMaze()
        {
            // break if there is no map
            if (Map.Count == 0) return;
            
            // sort the tiles into 2 lists based on coordinates
            List<MapTile> sortedTilesX = Map.OrderBy(t => t.X).ToList();
            List<MapTile> sortedTilesY = Map.OrderBy(t => t.Y).ToList();

            // grab the coordinate values for the max and min for X and Y
            int minX = sortedTilesX.First().X;
            int maxX = sortedTilesX.Last().X;
            int minY = sortedTilesY.First().Y;
            int maxY = sortedTilesY.Last().Y;

            // calculate the shortest width and height for this current maze
            int shortestWidth = maxX - minX + 1;
            int shortestHeight = maxY - minY + 1;
            
            Debug.WriteLine($"Width: {shortestWidth.ToString()} Height: {shortestHeight.ToString()}"); // tmp debug

            // shift all the coordinates to (0, 0)
            List<MapTile> resizedMap = Map.Select(t => {
                t.X -= minX;
                t.Y -= minY;
                return t;
            }).ToList();

            Map = resizedMap;
            Width = shortestWidth;
            Height = shortestHeight;
        }
    }
}