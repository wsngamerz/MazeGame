using System;
using System.Collections.Generic;

namespace MazeGame.Engine.RenderObjects
{
    /// <summary>
    /// A menu render object
    /// </summary>
    public class Menu : RenderObject
    {
        private readonly List<MenuItem> _menuItems;
        private readonly string _menuTitle;
        
        private int _menuContentWidth;

        // padding each side in chars
        private const int MenuPadding = 2;
        private const int MenuItemPadding = 2;

        // consts for menu styling
        private const string MenuBg = Style.BackgroundColor.Grayscale250;
        private const string MenuFg = Style.ForegroundColor.Black;
        private const string MenuItemBg = Style.BackgroundColor.Grayscale245;
        private const string MenuItemSelectedBg = Style.BackgroundColor.Grayscale240;

        private bool _shouldRender;
        private int _selectedItem;

        /// <summary>
        /// Menu component
        /// </summary>
        /// <param name="menuTitle"></param>
        public Menu(string menuTitle)
        {
            _menuItems = new List<MenuItem>();
            _menuTitle = menuTitle;
            _menuContentWidth = _menuTitle.Length;
            _shouldRender = true;
            
            Size = Vector2.Zero;
            Position = Vector2.Zero;
        }

        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="menuItem"></param>
        public void AddItem(MenuItem menuItem)
        {
            _menuItems.Add(menuItem);
            if (menuItem.Text.Length > _menuContentWidth) _menuContentWidth = menuItem.Text.Length;
        }

        /// <summary>
        /// Add Menu Item
        /// </summary>
        /// <param name="itemText"></param>
        /// <param name="menuCallback"></param>
        public void AddItem(string itemText, MenuCallback menuCallback = null)
        {
            AddItem(new MenuItem(itemText, menuCallback));
        }

        /// <summary>
        /// Run the callback for the menuItem that is currently selected
        /// </summary>
        private void SelectItem()
        {
            _menuItems[_selectedItem].RunCallback();
        }

        /// <summary>
        /// Move the cursor for the currently selected item up
        /// </summary>
        private void MoveUp()
        {
            _selectedItem--;
            _shouldRender = true;
            if (_selectedItem < 0) _selectedItem = _menuItems.Count - 1;
        }

        /// <summary>
        /// move the cursor for the currently selected item down
        /// </summary>
        private void MoveDown()
        {
            _selectedItem++;
            _shouldRender = true;
            if (_selectedItem >= _menuItems.Count) _selectedItem = 0;
        }

        /// <summary>
        /// called to update the size and position of the menu to always stay centred
        /// </summary>
        private void CalculateSizes()
        {
            int menuWidth = _menuContentWidth + (2 * MenuPadding) + (2 * MenuItemPadding);
            int menuHeight = 4 + _menuItems.Count;
            int x = (Scene.Display.Width / 2) - (menuWidth / 2);
            int y = (Scene.Display.Height / 2) - (menuHeight / 2);
            Position = new Vector2(x, y);
            Size = new Vector2(menuWidth, menuHeight);
        }

        /// <summary>
        /// Called on update
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            // check whether any of the keys have been pressed
            // Advantage of only checking whether they exist means only being called once
            if (updateInfo.PressedKeys.Contains(ConsoleKey.DownArrow)) MoveDown();
            if (updateInfo.PressedKeys.Contains(ConsoleKey.UpArrow)) MoveUp();
            if (updateInfo.PressedKeys.Contains(ConsoleKey.Enter)) SelectItem();
            
            // check for a resize or initial value so that we know we need to calculate some new sizes and positions
            if (updateInfo.HasResized || Size.Equals(Vector2.Zero)) CalculateSizes();
        }

        /// <summary>
        /// Called on render
        /// </summary>
        public override void Render()
        {
            // only render again if needed, otherwise the value stored in the content variable should be the same
            // result
            if (!_shouldRender) return;

            // blank string array for new the render
            var render = new string[Size.Y];
            
            // render blank box
            string blankLine = $"{MenuBg}{Utils.Repeat(Character.Empty, Size.X)}{Style.Reset}"; 
            render[0] = blankLine;
            render[2] = blankLine;
            render[Size.Y - 1] = blankLine;
            
            // add menu title
            render[1] = $"{MenuBg}{MenuFg}{Utils.CenterText(_menuTitle, Size.X)}{Style.Reset}";

            // draw all of the items
            for (var i = 0; i < _menuItems.Count; i++)
            {
                var menuItem = _menuItems[i];
                render[i + 3] = $"{MenuBg}{MenuFg}{Utils.Repeat(Character.Empty, MenuPadding)}{(_selectedItem == i ? MenuItemSelectedBg : MenuItemBg)}{Utils.Repeat(Character.Empty, MenuItemPadding)}{Utils.CenterText(menuItem.Text, _menuContentWidth)}{Utils.Repeat(Character.Empty, MenuItemPadding)}{MenuBg}{Utils.Repeat(Character.Empty, MenuPadding)}{Style.Reset}";
            }
            
            // unless there is a key input next update, rendering doesn't need to occur
            _shouldRender = false;
            Content = render;
        }
    }
}