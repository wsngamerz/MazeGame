using System;
using System.Reflection;

namespace MazeGame
{
    public class TestScreen
    {
        private readonly ScreenBuffer _screenBuffer;
        private bool _testScreenRunning;
        private int _screenNumber;
        
        public TestScreen(ScreenBuffer screenBuffer)
        {
            _screenBuffer = screenBuffer;
            _screenNumber = 1;
            _testScreenRunning = true;
            
            _screenBuffer.AddConstantRender(5, 2, $"{Style.Bold}{Style.Underline}Test Screen - (Press ESC to exit) [1 - Characters, 2 - BackgroundColours, 3 - ForegroundColours]{Style.Reset}");
        }

        public void Show()
        {
            while (_testScreenRunning)
            {
                switch (_screenNumber)
                {
                    case 1:
                        DisplayCharacters();
                        break;
                    
                    case 2:
                        DisplayBackgroundColours();
                        break;
                    
                    case 3:
                        DisplayForegroundColours();
                        break;
                }
                _screenBuffer.Show();
                
                // hang until user input
                Console.SetCursorPosition(0, 0);
                var consoleKeyInfo = Console.ReadKey(true);
                var consoleKey = consoleKeyInfo.Key;

                _screenNumber = consoleKey switch
                {
                    ConsoleKey.D1 => 1,
                    ConsoleKey.D2 => 2,
                    ConsoleKey.D3 => 3,
                    ConsoleKey.Escape => -1,
                    _ => _screenNumber
                };

                if (_screenNumber == -1)
                {
                    _testScreenRunning = false;
                }
                
                _screenBuffer.ClearBuffer();
            }
            
            // go back to the main menu
            new MazeGame().Start();
        }

        /// <summary>
        /// display all of the characters
        /// </summary>
        private void DisplayCharacters()
        {
            _screenBuffer.DrawText($"Characters", 5, 4);
            
            FieldInfo[] fields = typeof(Character).GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fieldInfo = fields[i];
                _screenBuffer.DrawText($"{fieldInfo.Name.PadRight(25)}: {fieldInfo.GetValue(fieldInfo)}", 5, i + 5);
            }
        }

        /// <summary>
        /// display all of the background colours
        /// </summary>
        private void DisplayBackgroundColours()
        {
            _screenBuffer.DrawText($"Background Colours", 5, 4);
            
            FieldInfo[] fields = typeof(Style.BackgroundColor).GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fieldInfo = fields[i];
                _screenBuffer.DrawText(fieldInfo.Name, 5, i + 5);
                
                var backgroundPixel = new Pixel(fieldInfo.GetValue(fieldInfo)?.ToString(), Style.ForegroundColor.White);
                _screenBuffer.DrawBox(backgroundPixel, 30, i+5, 30, 1);
            }
        }

        /// <summary>
        /// display all of the foreground colours
        /// </summary>
        private void DisplayForegroundColours()
        {
            _screenBuffer.DrawText($"Foreground Colours", 5, 4);
            
            
            FieldInfo[] fields = typeof(Style.ForegroundColor).GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var fieldInfo = fields[i];
                _screenBuffer.DrawText(fieldInfo.Name, 5, i + 5);

                var foregroundPixel = new Pixel(Style.BackgroundColor.Black, fieldInfo.GetValue(fieldInfo)?.ToString());
                _screenBuffer.DrawText(foregroundPixel, "abcdefghijklmnopqrstuvwxyz", 30, i + 5);
            }
        }
    }
}