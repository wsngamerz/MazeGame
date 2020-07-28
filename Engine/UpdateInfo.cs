using System;
using System.Collections.Generic;

namespace MazeGame.Engine
{
    /// <summary>
    /// Data struct that holds information passed to all the render objects about what has happened between updates
    /// </summary>
    public struct UpdateInfo
    {
        public List<ConsoleKey> PressedKeys { get; set; }
        public bool HasResized { get; set; }
    }
}