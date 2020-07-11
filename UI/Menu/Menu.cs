using System;
using System.Collections.Generic;

namespace MazeGame.UI.Menu
{
    public class Menu
    {
        private readonly List<MenuItem> _menuItems = new List<MenuItem>();
        private readonly string _menuTitle;
        
        private int _selectedItem = 0;
        private bool _isRunning = false;
        private bool _updateNeeded = true;
        private int _menuWidth;
        
        private const int MenuPadding = 3;
        private const int MenuTextPadding = 3;

        /// <summary>
        /// a menu component
        /// </summary>
        /// <param name="menuTitle">the menu's title</param>
        public Menu(string menuTitle)
        {
            _menuTitle = menuTitle;
            _menuWidth = _menuTitle.Length;
        }

        /// <summary>
        /// Adds an item to the menu
        /// </summary>
        /// <param name="menuItem">the menu item to add</param>
        public void AddItem(MenuItem menuItem)
        {
            _menuItems.Add(menuItem);

            // check for the widest item to ensure that the menu will resize accordingly
            int menuItemLength = menuItem.Text.Length;
            if (menuItemLength > _menuWidth)
            {
                _menuWidth = menuItemLength;
            }
        }

        /// <summary>
        /// Triggers the menu to be drawn
        /// </summary>
        /// <param name="screenBuffer">the screen buffer to use</param>
        public void Show(ScreenBuffer screenBuffer)
        {
            _isRunning = true;

            while (_isRunning)
            {
                if (_updateNeeded)
                {
                    _updateNeeded = false;
                    Draw(screenBuffer);
                }

                var consoleKeyInfo = Console.ReadKey();
                var consoleKey = consoleKeyInfo.Key;

                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (consoleKey)
                {
                    case ConsoleKey.DownArrow:
                        MoveDown();
                        break;
                
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        break;
                
                    case ConsoleKey.Enter:
                        SelectItem(screenBuffer);
                        break;
                }   
            }
        }

        /// <summary>
        /// Draws each 'frame' of the menu
        /// </summary>
        private void Draw(ScreenBuffer screenBuffer)
        {
            // menu starting locations
            int startY = (screenBuffer.BufferHeight / 2) - (_menuItems.Count / 2);
            int startX = (screenBuffer.BufferWidth / 2) - ((_menuWidth + MenuPadding + MenuTextPadding) / 2);
            
            // style pixels
            var menuBackgroundPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.BrightBlack };
            var menuTitlePixel = new Pixel() { BackgroundColor = Style.BackgroundColor.BrightBlack, ForegroundColor = Style.ForegroundColor.White };
            var menuItemPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Blue, ForegroundColor = Style.ForegroundColor.White };
            var menuItemSelectedPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Cyan, ForegroundColor = Style.ForegroundColor.BrightWhite };
            
            // draw the background
            screenBuffer.DrawBox(menuBackgroundPixel, startX, startY, _menuWidth + (2 * MenuPadding) + (2 * MenuTextPadding), _menuItems.Count + 4);
            
            // draw the menu title
            screenBuffer.DrawText(menuTitlePixel, _menuTitle, startX, startY);
            
            // draw the menu items
            for (var i = 0; i < _menuItems.Count; i++)
            {
                screenBuffer.DrawText((i == _selectedItem ? menuItemSelectedPixel : menuItemPixel), _menuItems[i].Text, startX + MenuTextPadding, startY + i + 2);
            }
            
            // write the screen buffer to console
            screenBuffer.Show();
        }

        /// <summary>
        /// move the index of the currently selected item down
        /// </summary>
        private void MoveDown()
        {
            _selectedItem++;
            _updateNeeded = true;

            // wrap around
            if (_selectedItem >= _menuItems.Count)
            {
                _selectedItem = 0;
            }
        }

        /// <summary>
        /// move the index of the currently selected item up
        /// </summary>
        private void MoveUp()
        {
            _selectedItem--;
            _updateNeeded = true;
            
            // wrap around
            if (_selectedItem < 0)
            {
                _selectedItem = _menuItems.Count - 1;
            }
        }

        /// <summary>
        /// call the callback for the currently selected menu item
        /// </summary>
        private void SelectItem(ScreenBuffer screenBuffer)
        {
            _isRunning = false;
            _updateNeeded = true;

            screenBuffer.ClearBuffer();
            _menuItems[_selectedItem].Select(screenBuffer);
        }
    }
}