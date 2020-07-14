namespace MazeGame
{
    /// <summary>
    /// Holds all the ansi escape codes for styling text and their colours
    /// </summary>
    public static class Style
    {
        public const string Reset = "\u001b[0m";
        public const string Bold = "\u001b[1m";
        public const string Underline = "\u001b[4m";
        public const string Inverse = "\u001b[7m";
        
        /// <summary>
        /// Foreground Colours
        /// </summary>
        public static class ForegroundColor
        {
            public const string Black = "\u001b[30m";
            public const string Red = "\u001b[31m";
            public const string Green = "\u001b[32m";
            public const string Yellow = "\u001b[33m";
            public const string Blue = "\u001b[34m";
            public const string Magenta = "\u001b[35m";
            public const string Cyan = "\u001b[36m";
            public const string White = "\u001b[37m";
            public const string BrightBlack = "\u001b[30;1m";
            public const string BrightRed = "\u001b[31;1m";
            public const string BrightGreen = "\u001b[32;1m";
            public const string BrightYellow = "\u001b[33;1m";
            public const string BrightBlue = "\u001b[34;1m";
            public const string BrightMagenta = "\u001b[35;1m";
            public const string BrightCyan = "\u001b[36;1m";
            public const string BrightWhite = "\u001b[37;1m";

            public const string Grayscale235 = "\u001b[38;5;235m";
        }

        /// <summary>
        /// background colours
        /// </summary>
        public static class BackgroundColor
        {
            public const string Black = "\u001b[40m";
            public const string Red = "\u001b[41m";
            public const string Green = "\u001b[42m";
            public const string Yellow = "\u001b[43m";
            public const string Blue = "\u001b[44m";
            public const string Magenta = "\u001b[45m";
            public const string Cyan = "\u001b[46m";
            public const string White = "\u001b[47m";
            public const string BrightBlack = "\u001b[40;1m";
            public const string BrightRed = "\u001b[41;1m";
            public const string BrightGreen = "\u001b[42;1m";
            public const string BrightYellow = "\u001b[43;1m";
            public const string BrightBlue = "\u001b[44;1m";
            public const string BrightMagenta = "\u001b[45;1m";
            public const string BrightCyan = "\u001b[46;1m";
            public const string BrightWhite = "\u001b[47;1m";

            public const string Grayscale235 = "\u001b[48;5;235m";
            public const string Grayscale240 = "\u001b[48;5;240m";
            public const string Grayscale245 = "\u001b[48;5;245m";
            public const string Grayscale250 = "\u001b[48;5;250m";

            /// <summary>
            /// convert a foreground colour code to a background colour code
            /// </summary>
            /// <param name="foregroundColour"></param>
            /// <returns></returns>
            public static string FromForeground(string foregroundColour)
            {
                return foregroundColour.Replace("[3", "[4");
            }
        }
    }
}