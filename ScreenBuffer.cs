using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGame
{
    public class ScreenBuffer
    {
        private static readonly string[,] ScreenBufferArray = new string[Console.WindowWidth, Console.WindowHeight];

        private List<Pixel> _constantRenderQueue = new List<Pixel>();
        
        public int BufferWidth => ScreenBufferArray.GetLength(0);
        public int BufferHeight => ScreenBufferArray.GetLength(1);

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
            var drawPixel = "";

            drawPixel += pixel.BackgroundColor ?? "";
            drawPixel += pixel.ForegroundColor ?? "";
            drawPixel += pixel.Character ?? " ";
            drawPixel += pixel.ResetAfter ? Style.Reset : "";
            ScreenBufferArray[pixel.X, pixel.Y] = drawPixel;
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
                    pixelList.Add(new Pixel()
                    {
                        Character = " ",
                        X = i,
                        Y = j,
                        BackgroundColor = basePixel.BackgroundColor,
                        ForegroundColor = basePixel.ForegroundColor,
                        ResetAfter = true
                    });
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
            DrawPixels(text.ToCharArray().Select((t, i) => new Pixel()
            {
                Character = t.ToString(),
                X = x + i,
                Y = y,
                BackgroundColor = basePixel.BackgroundColor,
                ForegroundColor = basePixel.ForegroundColor,
                ResetAfter = true
            }));
        }

        public void DrawText(string text, int x, int y)
        {
            var pixel = new Pixel()
            {
                BackgroundColor = Style.BackgroundColor.Black,
                ForegroundColor = Style.ForegroundColor.White
            };
            DrawText(pixel, text, x, y);
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
            for (int dy = 0; dy < multilineText.Length; dy++)
            {
                AddConstantRender(x, y + dy, multilineText[dy]);
            }
        }

        public void AddConstantRender(int x, int y, string text)
        {
            for (int dx = 0; dx < text.Length; dx++)
            {
                _constantRenderQueue.Add(new Pixel()
                {
                    X = x + dx,
                    Y = y,
                    Character = text[dx].ToString()
                });
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
                    ScreenBufferArray[x, y] = " ";
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
                string[] currentRow = Enumerable.Range(0, BufferWidth).Select(x => ScreenBufferArray[x, y]).ToArray();
                Console.SetCursorPosition(0, y);
                Console.Write(string.Join("", currentRow));
            }
        }
    }

    public class Pixel
    {
        public string Character { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool ResetAfter { get; set; }
    }
}