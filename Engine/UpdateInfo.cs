using System;
using System.Collections.Generic;

namespace MazeGame.Engine
{
    /// <summary>
    /// Data struct that holds information passed to all the render objects about what has happened between updates
    /// </summary>
    public struct UpdateInfo
    {
        /// <summary>
        /// A list of the keys that were pressed between update calls
        /// </summary>
        public List<ConsoleKey> PressedKeys { get; set; }
        
        /// <summary>
        /// Whether the console has been resized between update calls
        /// </summary>
        public bool HasResized { get; set; }
    }
}
