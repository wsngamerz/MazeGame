using System;
using System.Collections.Generic;

namespace MazeGame.Engine
{
    public struct UpdateInfo
    {
        public List<ConsoleKey> PressedKeys { get; set; }
        public bool HasResized { get; set; }
    }
}