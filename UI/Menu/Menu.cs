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
        
        // padding each side
        private const int MenuPadding = 2;
        private const int MenuTextPadding = 4;

        /// <summary>
        /// a menu component
        /// </summary>
        public Menu()
        {
            _menuTitle = null;
            _menuWidth = 0;
        }
        
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
        /// Add an item to menu
        /// </summary>
        /// <param name="menuItemName"></param>
        public void AddItem(string menuItemName)
        {
            AddItem(new MenuItem(menuItemName));
        }
        
        /// <summary>
        /// add an item to menu
        /// </summary>
        /// <param name="menuItemName"></param>
        /// <param name="menuItemMenu"></param>
        public void AddItem(string menuItemName, Menu menuItemMenu)
        {
            AddItem(new MenuItem(menuItemName, menuItemMenu));
        }
        
        /// <summary>
        /// add an item to menu
        /// </summary>
        /// <param name="menuItemName"></param>
        /// <param name="menuItemCallback"></param>
        public void AddItem(string menuItemName, MenuCallback menuItemCallback)
        {
            AddItem(new MenuItem(menuItemName, menuItemCallback));
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
            // calculate the size of the menus
            int menuBackgroundWidth = _menuWidth + (2 * MenuPadding) + (2 * MenuTextPadding);
            int menuBackgroundHeight = (_menuItems.Count * 2) + (_menuTitle == null ? 1 : 3);
            
            // menu starting locations
            int startY = (ScreenBuffer.BufferHeight / 2) - (menuBackgroundHeight / 2);
            int startX = (ScreenBuffer.BufferWidth / 2) - (menuBackgroundWidth / 2);
            
            // style pixels
            var menuBackgroundPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Grayscale240 };
            var menuTitlePixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Grayscale240, ForegroundColor = Style.ForegroundColor.White };
            var menuItemPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Grayscale245, ForegroundColor = Style.ForegroundColor.Black };
            var menuItemSelectedPixel = new Pixel() { BackgroundColor = Style.BackgroundColor.Grayscale250, ForegroundColor = Style.ForegroundColor.Black };
            
            // draw the background
            screenBuffer.DrawBox(menuBackgroundPixel, startX, startY, menuBackgroundWidth, menuBackgroundHeight);
            
            // calculate the available characters for the menu elements
            int menuItemAvailChars = _menuWidth + (2 * MenuTextPadding);
            
            // draw the menu title
            if (_menuTitle != null)
            {
                screenBuffer.DrawText(menuTitlePixel, Utils.CenterText(_menuTitle, menuItemAvailChars), startX + MenuPadding, startY + 1);   
            }
            
            // draw the menu items
            var index = 0;
            for (var i = 0; i < _menuItems.Count; i++)
            {
                screenBuffer.DrawText((i == _selectedItem ? menuItemSelectedPixel : menuItemPixel), Utils.CenterText(_menuItems[i].Text, menuItemAvailChars), startX + MenuPadding, startY + index + (_menuTitle==null?1:3));
                index += 2;
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
            if (!_menuItems[_selectedItem].HasCallback) return;
            
            _isRunning = false;
            _updateNeeded = true;

            screenBuffer.ClearBuffer();
            _menuItems[_selectedItem].Select(screenBuffer);
        }
    }
}