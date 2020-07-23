using System;

namespace MazeGame.Maze
{
    [Serializable]
    public abstract class MapTile
    {
        public abstract string Char { get; }
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
        public abstract MazeTileType TileType { get; }
        public abstract bool IsStatic { get; }
        public abstract int Limit { get; }
        public abstract string BackgroundColour { get; set; }
        public abstract string ForegroundColour { get; set; }

        public static MapTile GetTile(MazeTileType mazeTileType)
        {
            return mazeTileType switch
            {
                MazeTileType.None => new MazeTile(),
                MazeTileType.Start => new StartTile(),
                MazeTileType.Finish => new FinishTile(),
                MazeTileType.Player => new PlayerTile(),
                MazeTileType.Wall => new WallTile(),
                _ => throw new Exception()
            };
        }
    }

    public enum MazeTileType
    {
        None,
        Start,
        Finish,
        Player,
        Wall
    }

    // TODO: Set default foreground and background colours and use them to override the current colour selected
    
    [Serializable]
    public class MazeTile : MapTile
    {
        public override string Char => " ";
        public override int X { get; set; }
        public override int Y { get; set; }
        public override MazeTileType TileType => MazeTileType.None;
        public override bool IsStatic => true;
        public override int Limit => -1;
        public override string BackgroundColour { get; set; }
        public override string ForegroundColour { get; set; }
    }
    
    [Serializable]
    public class StartTile : MapTile
    {
        public override string Char => Character.MediumBlock;
        public override int X { get; set; }
        public override int Y { get; set; }
        public override MazeTileType TileType => MazeTileType.Start;
        public override bool IsStatic => true;
        public override int Limit => 10;
        public override string BackgroundColour { get; set; }
        public override string ForegroundColour { get; set; }
    }

    [Serializable]
    public class FinishTile : MapTile
    {
        public override string Char => Character.LightBlock;
        public override int X { get; set; }
        public override int Y { get; set; }
        public override MazeTileType TileType => MazeTileType.Finish;
        public override bool IsStatic => true;
        public override int Limit => 10;
        public override string BackgroundColour { get; set; }
        public override string ForegroundColour { get; set; }
    }

    [Serializable]
    public class PlayerTile : MapTile
    {
        public override string Char => Character.SolidSquare;
        public override int X { get; set; }
        public override int Y { get; set; }
        public override MazeTileType TileType => MazeTileType.Player;
        public override bool IsStatic => false;
        public override int Limit => 1;
        public override string BackgroundColour { get; set; }
        public override string ForegroundColour { get; set; }
    }
    
    [Serializable]
    public class WallTile : MapTile
    {
        public override string Char => Character.SolidBlock;
        public override int X { get; set; }
        public override int Y { get; set; }
        public override MazeTileType TileType => MazeTileType.Wall;
        public override bool IsStatic => true;
        public override int Limit => -1;
        public override string BackgroundColour { get; set; }
        public override string ForegroundColour { get; set; }
    }
}