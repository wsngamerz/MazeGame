namespace MazeGame.Engine
{
    /// <summary>
    /// All the characters used to draw to the screen
    /// </summary>
    public static class Character
    {
        public const char Empty = ' ';
        
        // Block Characters
        public const char LightBlock = '░';
        public const char MediumBlock = '▒';
        public const char HeavyBlock = '▓';
        public const char SolidBlock = '█';
        
        // Box drawing
        public const char Horizontal = '─';
        public const char Vertical = '│';
        public const char TopLeft = '┌';
        public const char TopCentre = '┬';
        public const char TopRight = '┐';
        public const char MiddleLeft = '├';
        public const char Centre = '┼';
        public const char MiddleRight = '┤';
        public const char BottomLeft = '└';
        public const char BottomCentre = '┴';
        public const char BottomRight = '┘';
        
        // Square Characters
        public const char SolidSquare = '■';
        public const char HollowSquare = '□';
        public const char RoundedHollowSquare = '▢';
    }
}