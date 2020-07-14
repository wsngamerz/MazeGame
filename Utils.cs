using System;
using System.Text;

namespace MazeGame
{
    /// <summary>
    /// A utility class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Centers the string given the length of the section it needs to bve centered in
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string CenterText(string text, int characters) {
            if (text.Length > characters) {
                throw new ArgumentException("Parameter is too small for the given text", nameof(characters));
            }

            return text.PadLeft(((characters - text.Length) / 2) + text.Length).PadRight(characters);
        }
        
        /// <summary>
        /// repeat a string a set number of times
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat(string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }
    }
}