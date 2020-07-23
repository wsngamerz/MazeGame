using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MazeGame.Maze
{
    public class MazeEditor
    {
        private readonly Maze _maze;
        private int _mazeOffsetX;
        private int _mazeOffsetY;

        private Pixel[] _mapPixels;

        private bool _cursorEnabled;
        private int _cursorX;
        private int _cursorY;
        private int _cursorSpeed;
        private Tuple<int, int, int, int> _cursorBoundaries;
        private Direction _cursorDirection;
        private Dictionary<Direction, Tuple<int, int>> _cursorDirectionDeltaMap;

        private MazeTileType _currentTileTool;

        private bool _isBackgroundColourSelector;
        private int _currentBackgroundColour;
        private int _currentForegroundColour;
        private string[] _backgroundColours;
        private string[] _foregroundColours;

        private Pixel _cursorPixel;
        private Pixel _blankMapPixel;

        private string[] _uiOutline;
        
        private bool _isEditorRunning;
        private bool _mazeUpdated;
        
        /// <summary>
        /// Maze editor with creating a new maze
        /// </summary>
        /// <param name="mazeName"></param>
        public MazeEditor(string mazeName)
        {
            _maze = new Maze(mazeName);
        }

        /// <summary>
        /// Maze editor editing an existing maze
        /// </summary>
        /// <param name="maze"></param>
        public MazeEditor(Maze maze)
        {
            _maze = maze;
        }

        /// <summary>
        /// Sets some default values
        /// </summary>
        private void SetupMazeEditor(ScreenBuffer screenBuffer)
        {
            
            // maze properties
            _mazeOffsetX = 1;
            _mazeOffsetY = 6;
            
            // cursor properties
            _cursorEnabled = true;
            _cursorX = _mazeOffsetX;
            _cursorY = _mazeOffsetY;
            _cursorSpeed = 1;
            _cursorDirection = Direction.Down;
            
            // trigger updates
            _mazeUpdated = true;
            
            // minX, maxX, minY, maxY
            _cursorBoundaries = new Tuple<int, int, int, int>(_mazeOffsetX, screenBuffer.BufferWidth - 2, _mazeOffsetY, screenBuffer.BufferHeight - 2);
            
             // map a direction to a set of delta x,y values
             _cursorDirectionDeltaMap = new Dictionary<Direction, Tuple<int, int>>()
            {
                { Direction.Down, new Tuple<int, int>(0, 1) },
                { Direction.Left, new Tuple<int, int>(-1, 0) },
                { Direction.Right, new Tuple<int, int>(1, 0) },
                { Direction.Up, new Tuple<int, int>(0, -1) }
            };

            // pre-calculate ui outline strings
            const int uiTopSplitA = 72;
            const int uiTopSplitB = 80;
            _uiOutline = new string[screenBuffer.BufferHeight];
            
            string uiLineTop = $"{Character.TopLeft}{Utils.Repeat(Character.Horizontal, uiTopSplitA)}{Character.TopCentre}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - uiTopSplitA - uiTopSplitB - 4)}{Character.TopCentre}{Utils.Repeat(Character.Horizontal, uiTopSplitB)}{Character.TopRight}";
            string uiLineTopMiddle = $"{Character.Vertical}{Utils.Repeat(" ", uiTopSplitA)}{Character.Vertical}{Utils.Repeat(" ", screenBuffer.BufferWidth - uiTopSplitA - uiTopSplitB - 4)}{Character.Vertical}{Utils.Repeat(" ", uiTopSplitB)}{Character.Vertical}";
            string uiLineTopSplit = $"{Character.MiddleLeft}{Utils.Repeat(Character.Horizontal, uiTopSplitA)}{Character.BottomCentre}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - uiTopSplitA - uiTopSplitB - 4)}{Character.BottomCentre}{Utils.Repeat(Character.Horizontal, uiTopSplitB)}{Character.MiddleRight}";
            string uiLineMiddle = $"{Character.Vertical}{Utils.Repeat(" ", screenBuffer.BufferWidth - 2)}{Character.Vertical}";
            string uiLineBottom = $"{Character.BottomLeft}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - 2)}{Character.BottomRight}";
            
            // draw screen outline
            for (var y = 0; y < screenBuffer.BufferHeight; y++)
            {
                switch (y)
                {
                    case 0:
                        _uiOutline[y] = uiLineTop;
                        break;
                    
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        _uiOutline[y] = uiLineTopMiddle;
                        break;
                    
                    case 5:
                        _uiOutline[y] = uiLineTopSplit;
                        break;
                    
                    default:
                        _uiOutline[y] = y == screenBuffer.BufferHeight - 1 ? uiLineBottom : uiLineMiddle;
                        break;
                }
            }
            
            // create the pixel styles
            _cursorPixel = new Pixel(Character.MediumBlock, Style.ForegroundColor.Cyan, Style.BackgroundColor.Grayscale235);
            _blankMapPixel = new Pixel(Style.ForegroundColor.White, Style.BackgroundColor.Grayscale235);

            // set the current pixel
            _currentTileTool = MazeTileType.None;
            
            // get the background colours
            _isBackgroundColourSelector = true;
            _currentBackgroundColour = 0;
            _currentForegroundColour = 0;
            _backgroundColours = typeof(Style.BackgroundColor).GetFields().Select(fi => fi.GetValue(fi)?.ToString()).ToArray();
            _foregroundColours = typeof(Style.ForegroundColor).GetFields().Select(fi => fi.GetValue(fi)?.ToString()).ToArray();
        }
        
        /// <summary>
        /// Display the maze along with the editor UI and handle all of the User Inputs
        /// </summary>
        /// <param name="screenBuffer"></param>
        public void Show(ScreenBuffer screenBuffer)
        {
            _isEditorRunning = true;
            
            SetupMazeEditor(screenBuffer);

            // add the ui border as a constant render
            screenBuffer.AddConstantRender(0, 0, _uiOutline);

            while (_isEditorRunning)
            {
                // we shouldn't need to check whether we need to update as this loop will only continue
                // on user input
                Draw(screenBuffer);

                // read input
                var consoleKeyInfo = Console.ReadKey(true);
                var consoleKey = consoleKeyInfo.Key;

                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (consoleKey)
                {
                    /*
                     * Cursor Movement
                     */
                    case ConsoleKey.UpArrow:
                        // only move if cursor enabled
                        if (!_cursorEnabled) break;
                        
                        MoveCursor(0, -1);
                        _cursorDirection = Direction.Up;
                        break;
                        
                    case ConsoleKey.DownArrow:
                        // only move if cursor enabled
                        if (!_cursorEnabled) break;
                        
                        MoveCursor(0, 1);
                        _cursorDirection = Direction.Down;
                        break;
                        
                    case ConsoleKey.LeftArrow:
                        // only move if cursor enabled
                        if (!_cursorEnabled) break;
                        
                        MoveCursor(-1, 0);
                        _cursorDirection = Direction.Left;
                        break;
                        
                    case ConsoleKey.RightArrow:
                        // only move if cursor enabled
                        if (!_cursorEnabled) break;
                        
                        MoveCursor(1, 0);
                        _cursorDirection = Direction.Right;
                        break;
                        
                    /*
                     * Cursor Speed
                     */
                    case ConsoleKey.Add:
                        ModifyCursorSpeed(+1);
                        break;
                    
                    case ConsoleKey.Subtract:
                        ModifyCursorSpeed(-1);
                        break;
                    
                    /*
                     * Draw a Map tile
                     */
                    case ConsoleKey.Spacebar:
                        // only draw if the cursor is enabled
                        if (!_cursorEnabled) break;
                        
                        DrawTile();
                        break;
                            
                    /*
                     * Map Tile Selection
                     */
                    case ConsoleKey.D1:
                        _currentTileTool = MazeTileType.None;
                        break;
                        
                    case ConsoleKey.D2:
                        _currentTileTool = MazeTileType.Wall;
                        break;
                        
                    case ConsoleKey.D3:
                        _currentTileTool = MazeTileType.Start;
                        break;
                        
                    case ConsoleKey.D4:
                        _currentTileTool = MazeTileType.Finish;
                        break;
                        
                    case ConsoleKey.D5:
                        _currentTileTool = MazeTileType.Player;
                        break;

                    /*
                     * Colour selector
                     */
                    case ConsoleKey.Tab:
                        CycleColourSelector();
                        break;
                    
                    case ConsoleKey.Oem8:
                        _isBackgroundColourSelector = !_isBackgroundColourSelector;
                        break;
                    
                    /*
                     * Debug Keys
                     */
                    
                    // force resize
                    case ConsoleKey.F1:
                        _maze.ResizeMaze();
                        
                        // force the maze to be drawn
                        _mazeUpdated = true;
                        DrawMaze(screenBuffer);
                        
                        break;
                    
                    // Toggle Cursor Disabled
                    case ConsoleKey.F2:
                        _cursorEnabled = !_cursorEnabled;
                        break;
                    
                    case ConsoleKey.F3:
                        _maze.Save();
                        break;
                }
                
                // clear any other input
                // NOTE: This isn't ideal but this prevents a queue of keys to be read
                //       if the user holds down a key which would cause "input lag" of
                //       considerable effect.
                while (Console.KeyAvailable) Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Draw a single frame
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void Draw(ScreenBuffer screenBuffer)
        {
            // clear the buffer
            screenBuffer.ClearBuffer();

            DrawEditorUi(screenBuffer);
            DrawMaze(screenBuffer);
            DrawCursor(screenBuffer);
            DrawColourSelector(screenBuffer);
            
            // display the screen buffer
            screenBuffer.Show();
        }

        /// <summary>
        /// Draw the editors UI
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawEditorUi(ScreenBuffer screenBuffer)
        {
            // draw maze info
            string toolName = _currentTileTool == MazeTileType.None ? "Eraser" : $"{_currentTileTool}";
            
            screenBuffer.DrawText($"Editing Maze: {_maze.Name}", 2, 1);
            screenBuffer.DrawText($"Tile count: {_maze.Map.Count.ToString()}", 2, 2);
            screenBuffer.DrawText($"Current Tool: {toolName}", 2, 3);
            screenBuffer.DrawText("1 - Eraser : 2 - Wall : 3 - StartPos : 4 - FinishPos : 5 - PlayerPos", 2, 4);
            
            // draw current tile info
            var currentTile = GetCursorMazeTile();
            var currentTileName = "None";
            if (currentTile != null)
            {
                currentTileName = currentTile.TileType.ToString();
            }
            
            screenBuffer.DrawText($"Current tile: {currentTileName}", 75, 1);
            screenBuffer.DrawText($"X: {(_cursorX - _mazeOffsetX).ToString()}", 75, 2);
            screenBuffer.DrawText($"Y: {(_cursorY - _mazeOffsetY).ToString()}", 75, 3);
            screenBuffer.DrawText($"Cursor speed: {_cursorSpeed.ToString()}", 75, 4);
            
            screenBuffer.DrawText(new Pixel(Style.ForegroundColor.White, _cursorEnabled ? Style.BackgroundColor.Green : Style.BackgroundColor.Red), $"Cursor {(_cursorEnabled ? "Enabled": "Disabled")}", 82, 2);
        }

        /// <summary>
        /// draw the maze
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawMaze(ScreenBuffer screenBuffer)
        {
            if (_mazeUpdated)
            {
                _mazeUpdated = false;

                _mapPixels = new Pixel[_maze.Map.Count];
                for (var i = 0; i < _mapPixels.Length; i++)
                {
                    var mapTile = _maze.Map[i];
                    _mapPixels[i] = new Pixel(mapTile.Char, mapTile.X + _mazeOffsetX, mapTile.Y + _mazeOffsetY, mapTile.ForegroundColour, mapTile.BackgroundColour, true);
                }    
            }
            
            screenBuffer.DrawBox(_blankMapPixel, _mazeOffsetX, _mazeOffsetY, screenBuffer.BufferWidth - _mazeOffsetX - 1, screenBuffer.BufferHeight - _mazeOffsetY - 1);
            screenBuffer.DrawPixels(_mapPixels);
        }

        /// <summary>
        /// Draw the cursor
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawCursor(ScreenBuffer screenBuffer)
        {
            // only render if the cursor is enabled
            if (!_cursorEnabled) return;
            
            var tile = GetCursorMazeTile();
            
            // draw the cursor
            _cursorPixel.X = _cursorX;
            _cursorPixel.Y = _cursorY;
            _cursorPixel.BackgroundColor = tile != null ? Style.BackgroundColor.FromForeground(tile.ForegroundColour) : Style.BackgroundColor.Grayscale235;
            
            screenBuffer.DrawPixel(_cursorPixel);
        }

        /// <summary>
        /// Draw the colour selector which relates to the currently selected tool
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawColourSelector(ScreenBuffer screenBuffer)
        {
            // TODO: compare the selected colour indicator against background to check contrast
            
            if (_isBackgroundColourSelector)
            {
                screenBuffer.DrawText("Colour selector - Background", 100, 1);
                
                int itemsPerRow = (_backgroundColours.Length / 3);
                var row = 0;
                var index = 0;
                foreach (string backgroundColour in _backgroundColours)
                {
                    if (index > itemsPerRow)
                    {
                        row++;
                        index = 0;
                    }
                
                    screenBuffer.DrawBox(new Pixel(Style.ForegroundColor.White, backgroundColour),
                        100 + index * 3,
                        2 + row,
                        3,
                        1);

                    if (index + (row * itemsPerRow) + row == _currentBackgroundColour)
                    {
                        screenBuffer.DrawPixel(new Pixel(Character.SolidSquare, 101 + index * 3, 2 + row, Style.ForegroundColor.White, backgroundColour, true));
                    }
                
                    index++;
                }
            }
            else
            {
                screenBuffer.DrawText("Colour selector - Foreground", 100, 1);
            
                int itemsPerRow = (_foregroundColours.Length / 3);
                var row = 0;
                var index = 0;
                foreach (string foregroundColour in _foregroundColours)
                {
                    if (index > itemsPerRow)
                    {
                        row++;
                        index = 0;
                    }
            
                    screenBuffer.DrawText(new Pixel(foregroundColour, Style.BackgroundColor.Black), Utils.Repeat(Character.SolidBlock, 3), 100 + index * 3, 2 + row);
                    
                    if (index + (row * itemsPerRow) + row == _currentForegroundColour)
                    {
                        screenBuffer.DrawPixel(new Pixel(Character.SolidSquare, 101 + index * 3, 2 + row, Style.ForegroundColor.White, Style.BackgroundColor.FromForeground(foregroundColour), true));
                    }
            
                    index++;
                }
            }
        }

        /// <summary>
        /// Cycles round the available colours
        /// </summary>
        private void CycleColourSelector()
        {
            if (_isBackgroundColourSelector)
            {
                _currentBackgroundColour++;
                if (_currentBackgroundColour == _backgroundColours.Length) _currentBackgroundColour = 0;
            }
            else
            {
                _currentForegroundColour++;
                if (_currentForegroundColour == _foregroundColours.Length) _currentForegroundColour = 0;
            }
        }
        
        /// <summary>
        /// Move the cursor and trigger an update
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        private void MoveCursor(int dx, int dy)
        {
            int newCursorX = _cursorX + (dx * _cursorSpeed);
            int newCursorY = _cursorY + (dy * _cursorSpeed);

            if (newCursorX > _cursorBoundaries.Item2 || newCursorX < _cursorBoundaries.Item1) return;
            if (newCursorY > _cursorBoundaries.Item4 || newCursorY < _cursorBoundaries.Item3) return;

            _cursorX = newCursorX;
            _cursorY = newCursorY;
        }

        /// <summary>
        /// Change the speed of the cursor
        /// </summary>
        /// <param name="dv">the cursors velocity</param>
        private void ModifyCursorSpeed(int dv)
        {
            // prevent going under 1
            if (_cursorSpeed + dv < 1) return;
            
            _cursorSpeed += dv;
        }

        /// <summary>
        /// called when the used edits the maze
        /// </summary>
        private void DrawTile()
        {
            // remove the existing pixel if it already exists
            var mapTile = GetCursorMazeTile();
            if (mapTile != null) _maze.Map.Remove(mapTile);

            // if not the eraser tool
            if (_currentTileTool != MazeTileType.None)
            {
                mapTile = MapTile.GetTile(_currentTileTool);
                mapTile.X = _cursorX - _mazeOffsetX;
                mapTile.Y = _cursorY - _mazeOffsetY;
                mapTile.ForegroundColour = _foregroundColours[_currentForegroundColour];
                mapTile.BackgroundColour = _backgroundColours[_currentBackgroundColour];
                _maze.Map.Add(mapTile);
            }
            
            MoveCurrentDirection();
            _mazeUpdated = true;
        }

        /// <summary>
        /// Move the cursor in the current direction
        /// </summary>
        private void MoveCurrentDirection()
        {
            // move the cursor in the direction it was moving
            (int dx, int dy) = _cursorDirectionDeltaMap[_cursorDirection];
            MoveCursor(dx, dy);
        }

        /// <summary>
        /// Get the tile that the cursor is currently over
        /// </summary>
        /// <returns></returns>
        private MapTile GetCursorMazeTile()
        {
            return _maze.Map.FirstOrDefault(mapTile => mapTile.X == _cursorX - _mazeOffsetX && mapTile.Y == _cursorY - _mazeOffsetY);
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}