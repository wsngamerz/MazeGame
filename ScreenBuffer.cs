using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGame
{
    public class ScreenBuffer
    {
        private readonly string[,] _screenBufferArray = new string[Console.WindowWidth, Console.WindowHeight];

        private List<Pixel> _constantRenderQueue = new List<Pixel>();
        
        public int BufferWidth => _screenBufferArray.GetLength(0);
        public int BufferHeight => _screenBufferArray.GetLength(1);

        /// <summary>
        /// Used to draw a frame all in one go
        /// </summary>
        public ScreenBuffer()
        {
            // initialize with blank screen
            ClearBuffer();
        }

        /// <summary>
        /// add a single pixel to the screen buffer
        /// </summary>
        /// <param name="pixel"></param>
        public void DrawPixel(Pixel pixel)
        {
            _screenBufferArray[pixel.X, pixel.Y] = pixel.ToString();
        }

        /// <summary>
        /// add multiple pixels to the screen buffer
        /// </summary>
        /// <param name="pixels"></param>
        public void DrawPixels(IEnumerable<Pixel> pixels)
        {
            foreach (var pixel in pixels)
            {
                DrawPixel(pixel);
            }
        }

        /// <summary>
        /// draw a box to the screen buffer
        /// </summary>
        /// <param name="basePixel"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public void DrawBox(Pixel basePixel, int x, int y, int dx, int dy)
        {
            var pixelList = new List<Pixel>();

            for (int i = x; i < x + dx; i++)
            {
                for (int j = y; j < y + dy; j++)
                {
                    pixelList.Add(new Pixel(" ", i, j, basePixel.BackgroundColor, basePixel.ForegroundColor, true));
                }
            }
            
            DrawPixels(pixelList);
        }

        /// <summary>
        /// draw text to the screen buffer
        /// </summary>
        /// <param name="basePixel"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawText(Pixel basePixel, string text, int x, int y)
        {
            DrawPixels(text.ToCharArray().Select((t, i) => new Pixel(t.ToString(), x + i, y, basePixel.BackgroundColor, basePixel.ForegroundColor, true)));
        }

        public void DrawText(string text, int x, int y)
        {
            var pixel = new Pixel(Style.ForegroundColor.White, Style.BackgroundColor.Black);
            DrawText(pixel, text, x, y);
        }

        public void DrawText(string[] text, int x, int y)
        {
            for (var i = 0; i < text.Length; i++)
            {
                DrawText(text[i], x, y + i);
            }
        }

        /// <summary>
        /// Add an item that will be constantly drawn to the screen until
        /// the queue is cleared
        /// </summary>
        /// <param name="y"></param>
        /// <param name="multilineText"></param>
        /// <param name="x"></param>
        public void AddConstantRender(int x, int y, string[] multilineText)
        {
            for (var dy = 0; dy < multilineText.Length; dy++)
            {
                AddConstantRender(x, y + dy, multilineText[dy]);
            }
        }

        public void AddConstantRender(int x, int y, string text)
        {
            for (var dx = 0; dx < text.Length; dx++)
            {
                if (text[dx] != ' ')
                {
                    _constantRenderQueue.Add(new Pixel(text[dx].ToString(), x + dx, y, null, null, false));   
                }
            }
        }

        /// <summary>
        /// Blank the screen
        /// </summary>
        public void ClearBuffer()
        {
            for (var x = 0; x < BufferWidth; x++)
            {
                for (var y = 0; y < BufferHeight; y++)
                {
                    _screenBufferArray[x, y] = " ";
                }
            }
        }

        /// <summary>
        /// Clear the render queue
        /// </summary>
        public void ClearRenderQueue()
        {
            _constantRenderQueue = new List<Pixel>();
        }
        
        /// <summary>
        /// draw the internal screen buffer to the console
        /// </summary>
        public void Show()
        {
            DrawPixels(_constantRenderQueue);
            
            for (var y = 0; y < BufferHeight; y++)
            {
                string[] currentRow = Enumerable.Range(0, BufferWidth).Select(x => _screenBufferArray[x, y]).ToArray();
                Console.SetCursorPosition(0, y);
                Console.Write(string.Join("", currentRow));
            }
        }
    }

    public struct Pixel
    {
        public Pixel(string character, int x, int y, string foregroundColor, string backgroundColor, bool resetAfter)
        {
            Character = character;
            X = x;
            Y = y;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            ResetAfter = resetAfter;
        }

        public Pixel(string character, string foregroundColor, string backgroundColor)
        {
            Character = character;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;

            X = 0;
            Y = 0;
            ResetAfter = false;
        }

        public Pixel(string foregroundColor, string backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;

            Character = " ";
            X = 0;
            Y = 0;
            ResetAfter = false;
        }
        
        public string Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool ResetAfter { get; set; }

        public override string ToString() => $"{BackgroundColor}{ForegroundColor}{Character}{(ResetAfter?Style.Reset:"")}";
    }
}