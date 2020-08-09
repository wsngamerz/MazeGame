using System;
using System.Numerics;
using MazeGame.Engine;

namespace MazeGame.RenderObjects
{
    /// <summary>
    /// A text input box render component
    /// </summary>
    public class TextInput : RenderObject
    {
        public string Value { get; set; }
        
        public TextInputOnSubmit OnSubmit { get; set; }

        public string InputTitle { get; set; }
        
        private const string BgColour = Style.BackgroundColor.Grayscale240;
        private const string InpBgColour = Style.BackgroundColor.Grayscale235;
        private const string FgColour = Style.ForegroundColor.White;

        public const int InputWidth = 40;
        public const int InputPadding = 3;
        private const int CurChangePerSec = 2;

        private readonly int _cursorDisplayLoop;
        private bool _isCursorDisplayed;
        private int _currentCursorLoop;

        public TextInput(string inputTitle, Vector2 pos)
        {
            InputTitle = inputTitle;
            Position = pos;

            Value = "";
            Enabled = true;
            
            _isCursorDisplayed = true;
            _currentCursorLoop = 0;
            _cursorDisplayLoop = Display.Fps / CurChangePerSec; // display 2 times per second
        }
        
        /// <summary>
        /// the update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            foreach (var consoleKeyInfo in updateInfo.PressedKeys)
            {
                // handle character input
                if (char.IsLetterOrDigit(consoleKeyInfo.KeyChar)) Value += consoleKeyInfo.KeyChar;
                
                // handle specific console key inputs
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                else switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.Spacebar:
                        Value += ' ';
                        break;
                    
                    case ConsoleKey.Backspace:
                        // only remove a char if there is actually one there to remove
                        if (Value.Length >= 1) Value = Value.Remove(Value.Length - 1);
                        break;
                    
                    case ConsoleKey.Enter:
                        // call the submit handler if it exists with the value of the textbox
                        OnSubmit?.Invoke(Value);
                        break;
                }
            }
            
            // trigger the cursor to display or not display every frame based upon the value of CursorDisplayLoop
            _currentCursorLoop++;
            if (_currentCursorLoop < _cursorDisplayLoop) return;
            
            _isCursorDisplayed = !_isCursorDisplayed;
            _currentCursorLoop = 0;
        }

        /// <summary>
        /// the render method
        /// </summary>
        public override void Render()
        {
            // the displayed value also contains the cursor
            string inputPadding = Utils.Repeat(Character.Empty, Math.Clamp(InputWidth - (2 * InputPadding) - 1 - Value.Length, 0, InputWidth));
            string dVal = $"{Utils.LastNCharacters(Value, InputWidth - (2 * InputPadding) - 1)}{(_isCursorDisplayed ? Style.ForegroundColor.White : Style.ForegroundColor.FromBackground(InpBgColour))}{Character.SolidBlock}{inputPadding}";
            
            // a padding string that only needs to be calculated once
            string padding = Utils.Repeat(Character.Empty, InputPadding);
            
            // other lines
            string blankBgLine = $"{BgColour}{Utils.Repeat(Character.Empty, InputWidth)}{Style.Reset}";
            string titleLine = $"{BgColour}{Utils.CenterText(InputTitle, InputWidth)}{Style.Reset}";
            string inputLine = $"{BgColour}{padding}{FgColour}{InpBgColour}{dVal}{BgColour}{padding}{Style.Reset}";
            
            // the render
            Content = new[]
            {
                blankBgLine,
                titleLine,
                blankBgLine,
                inputLine,
                blankBgLine
            };
        }
    }

    public delegate void TextInputOnSubmit(string value);
}
