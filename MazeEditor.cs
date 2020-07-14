using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGame
{
    public class MazeEditor
    {
        private readonly Maze _maze;
        private int _mazeOffsetX;
        private int _mazeOffsetY;

        private int _cursorX;
        private int _cursorY;
        private Tuple<int, int, int, int> _cursorBoundaries;
        private Direction _cursorDirection;
        private Dictionary<Direction, Tuple<int, int>> _cursorDirectionDeltaMap;

        private MazeTileType _currentTileTool;
        
        private Pixel _cursorPixel;
        private Pixel _blankMapPixel;

        private string _uiLineTop;
        private string _uiLineTopMiddle;
        private string _uiLineTopSplit;
        private string _uiLineMiddle;
        private string _uiLineBottom;
        
        private bool _isEditorRunning;
        private bool _mazeUpdated;
        
        /// <summary>
        /// Maze editor with creating a new maze
        /// </summary>
        /// <param name="mazeName"></param>
        public MazeEditor(string mazeName)
        {
            _maze = new Maze(10, 10);
            _maze.Name = mazeName;
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
            // cursor properties
            _mazeOffsetX = 1;
            _mazeOffsetY = 6;
            _cursorX = _mazeOffsetX;
            _cursorY = _mazeOffsetY;
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
            var uiTopSplitA = 80;
            _uiLineTop = $"{Character.TopLeft}{Utils.Repeat(Character.Horizontal, uiTopSplitA)}{Character.TopCentre}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - uiTopSplitA - 3)}{Character.TopRight}";
            _uiLineTopMiddle = $"{Character.Vertical}{Utils.Repeat(" ", uiTopSplitA)}{Character.Vertical}{Utils.Repeat(" ", screenBuffer.BufferWidth - uiTopSplitA - 3)}{Character.Vertical}";
            _uiLineTopSplit = $"{Character.MiddleLeft}{Utils.Repeat(Character.Horizontal, uiTopSplitA)}{Character.BottomCentre}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - uiTopSplitA - 3)}{Character.MiddleRight}";
            _uiLineMiddle = $"{Character.Vertical}{Utils.Repeat(" ", screenBuffer.BufferWidth - 2)}{Character.Vertical}";
            _uiLineBottom = $"{Character.BottomLeft}{Utils.Repeat(Character.Horizontal, screenBuffer.BufferWidth - 2)}{Character.BottomRight}";
            
            // create all the pixel styles
            _cursorPixel = new Pixel()
            {
                Character = Character.LightBlock,
                ForegroundColor = Style.ForegroundColor.Cyan,
                BackgroundColor = Style.BackgroundColor.Grayscale235
            };
            
            _blankMapPixel = new Pixel()
            {
                Character = " ",
                ForegroundColor = Style.ForegroundColor.White,
                BackgroundColor = Style.BackgroundColor.Grayscale235
            };

            // set the current pixel
            _currentTileTool = MazeTileType.None;
        }
        
        /// <summary>
        /// Display the maze along with the editor UI and handle all of the User Inputs
        /// </summary>
        /// <param name="screenBuffer"></param>
        public void Show(ScreenBuffer screenBuffer)
        {
            _isEditorRunning = true;
            
            SetupMazeEditor(screenBuffer);

            while (_isEditorRunning)
            {
                // TODO: Check whether need to actually update
                Draw(screenBuffer);
                
                Console.SetCursorPosition(0, 0);
                
                // read input
                var consoleKeyInfo = Console.ReadKey();
                var consoleKey = consoleKeyInfo.Key;

                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (consoleKey)
                {
                    // Movement inputs
                    case ConsoleKey.UpArrow:
                        MoveCursor(0, -1);
                        _cursorDirection = Direction.Up;
                        break;
                    
                    case ConsoleKey.DownArrow:
                        MoveCursor(0, 1);
                        _cursorDirection = Direction.Down;
                        break;
                    
                    case ConsoleKey.LeftArrow:
                        MoveCursor(-1, 0);
                        _cursorDirection = Direction.Left;
                        break;
                    
                    case ConsoleKey.RightArrow:
                        MoveCursor(1, 0);
                        _cursorDirection = Direction.Right;
                        break;
                    
                    // Draw a pixel
                    case ConsoleKey.Spacebar:
                        MazeDraw();
                        break;
                        
                    // pixel selection
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
                }
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
            
            // display the screen buffer
            screenBuffer.Show();
        }

        /// <summary>
        /// Draw the editors UI
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawEditorUi(ScreenBuffer screenBuffer)
        {
            // draw screen outline
            for (var y = 0; y < screenBuffer.BufferHeight; y++)
            {
                switch (y)
                {
                    case 0:
                        screenBuffer.DrawText(_uiLineTop, 0, y);
                        break;
                    
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        screenBuffer.DrawText(_uiLineTopMiddle, 0, y);
                        break;
                    
                    case 5:
                        screenBuffer.DrawText(_uiLineTopSplit, 0, y);
                        break;
                    
                    default:
                        screenBuffer.DrawText(y == screenBuffer.BufferHeight - 1 ? _uiLineBottom : _uiLineMiddle, 0, y);
                        break;
                }
            }
            
            // draw maze info
            screenBuffer.DrawText($"Editing Maze: {_maze.Name}", 1, 1);
            screenBuffer.DrawText($"Tile count: {_maze.Map.Count.ToString()}", 1, 2);
            screenBuffer.DrawText($"Current Tool: {_currentTileTool}", 1, 3);
            screenBuffer.DrawText($"1 - Eraser : 2 - Wall : 3 - StartPos : 4 - FinishPos : 5 - PlayerPos", 1, 4);
            
            // draw current tile info
            var currentTile = GetCursorMazeTile();
            var currentTileName = "None";
            if (currentTile != null)
            {
                currentTileName = currentTile.TileType.ToString();
            }
            
            screenBuffer.DrawText($"Current tile: {currentTileName}", 82, 1);
            screenBuffer.DrawText($"X: {(_cursorX - _mazeOffsetX).ToString()}", 82, 2);
            screenBuffer.DrawText($"Y: {(_cursorY - _mazeOffsetY).ToString()}", 82, 3);
        }

        /// <summary>
        /// draw the maze
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawMaze(ScreenBuffer screenBuffer)
        {
            // return early if it doesn't need updating
            // if (!_mazeUpdated) return;
            // _mazeUpdated = false;

            var mapPixels = new Pixel[_maze.Map.Count];
            for (var i = 0; i < mapPixels.Length; i++)
            {
                var mapTile = _maze.Map[i];
                var mapPixel = new Pixel()
                {
                    X = mapTile.X + _mazeOffsetX,
                    Y = mapTile.Y + _mazeOffsetY,
                    Character = mapTile.Char,
                    BackgroundColor = mapTile.BackgroundColour,
                    ForegroundColor = mapTile.ForegroundColour,
                    ResetAfter = false
                };
                mapPixels[i] = mapPixel;
            }
            screenBuffer.DrawBox(_blankMapPixel, _mazeOffsetX, _mazeOffsetY, screenBuffer.BufferWidth - _mazeOffsetX - 1, screenBuffer.BufferHeight - _mazeOffsetY - 1);
            screenBuffer.DrawPixels(mapPixels);
        }

        /// <summary>
        /// Draw the cursor
        /// </summary>
        /// <param name="screenBuffer"></param>
        private void DrawCursor(ScreenBuffer screenBuffer)
        {
            var tile = GetCursorMazeTile();
            
            // draw the cursor
            _cursorPixel.X = _cursorX;
            _cursorPixel.Y = _cursorY;
            _cursorPixel.BackgroundColor = tile != null ? Style.BackgroundColor.FromForeground(tile.ForegroundColour) : Style.BackgroundColor.Grayscale235;
            
            screenBuffer.DrawPixel(_cursorPixel);
        }
        
        /// <summary>
        /// Move the cursor and trigger an update
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        private void MoveCursor(int dx, int dy)
        {
            int newCursorX = _cursorX + dx;
            int newCursorY = _cursorY + dy;

            if (newCursorX > _cursorBoundaries.Item2 || newCursorX < _cursorBoundaries.Item1) return;
            if (newCursorY > _cursorBoundaries.Item4 || newCursorY < _cursorBoundaries.Item3) return;

            _cursorX = newCursorX;
            _cursorY = newCursorY;
        }

        /// <summary>
        /// called when the used edits the maze
        /// </summary>
        private void MazeDraw()
        {
            // remove the existing pixel if it already exists
            var mapTile = GetCursorMazeTile();
            if (mapTile != null) _maze.Map.Remove(mapTile);
            
            mapTile = MapTile.GetTile(_currentTileTool);
            mapTile.X = _cursorX - _mazeOffsetX;
            mapTile.Y = _cursorY - _mazeOffsetY;
            _maze.Map.Add(mapTile);

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

        private MapTile GetCursorMazeTile()
        {
            return _maze.Map.FirstOrDefault(mt => mt.X == _cursorX - _mazeOffsetX && mt.Y == _cursorY - _mazeOffsetY);
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