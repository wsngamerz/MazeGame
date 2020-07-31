using System;
using System.Collections.Generic;
using System.Linq;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    public class MainMenuScene : Scene
    {
        // menus
        private Menu _mainMenu;
        private Menu _playMenu;
        private Menu _editorMenu;
        private Menu _optionsMenu;

        /// <summary>
        /// Scene start method
        /// </summary>
        public override void Start()
        {
            base.Start();
            
            AddMenus();
            AddRenderObject(new Border());
        }

        /// <summary>
        /// Scene restart method
        /// </summary>
        public override void Restart()
        {
            base.Restart();
            
            // get list of loaded menus and remove them all (should only be 1 returned but just in case)
            List<RenderObject> menusLoaded = SceneObjects.Where(so => so.GetType() == typeof(Menu)).ToList();
            menusLoaded.ForEach(RemoveRenderObject);
            
            // add the main menu
            AddRenderObject(_mainMenu);
        }

        /// <summary>
        /// Scene update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            base.Update(updateInfo);
            
            // back to main menu on escape
            if (updateInfo.PressedKeys.Select(pk => pk.Key).Contains(ConsoleKey.Escape)) Display.SwitchScene("mainMenu");
        }

        /// <summary>
        /// add all of the main menus
        /// </summary>
        private void AddMenus()
        {
            // main menu
            _mainMenu = new Menu("Main Menu");
            _mainMenu.AddItem("Play", () => SwitchMenus(_mainMenu, _playMenu));
            _mainMenu.AddItem("Editor", () => SwitchMenus(_mainMenu, _editorMenu));
            _mainMenu.AddItem("Options", () => SwitchMenus(_mainMenu, _optionsMenu));
            _mainMenu.AddItem("Quit", QuitCallback);
            
            // play menu
            _playMenu = new Menu("Play");
            _playMenu.AddItem("Select Level", () => Display.SwitchScene("mazePlayer"));
            _playMenu.AddItem("Back", () => SwitchMenus(_playMenu, _mainMenu));
            
            // editor menu
            _editorMenu = new Menu("Editor");
            _editorMenu.AddItem("Create new Maze", () => Display.SwitchScene("mazeEditor"));
            _editorMenu.AddItem("Edit maze");
            _editorMenu.AddItem("Back", () => SwitchMenus(_editorMenu, _mainMenu));
            
            // options menu
            _optionsMenu = new Menu("Options");
            _optionsMenu.AddItem("Test Scene", () => Display.SwitchScene("testScene"));
            _optionsMenu.AddItem("Back", () => SwitchMenus(_optionsMenu, _mainMenu));
            
            // add the main menu as the starting menu
            AddRenderObject(_mainMenu);
        }

        /// <summary>
        /// Switch menu method
        /// </summary>
        /// <param name="prevMenu"></param>
        /// <param name="nextMenu"></param>
        private void SwitchMenus(Menu prevMenu, Menu nextMenu)
        {
            RemoveRenderObject(prevMenu);
            
            // reset the menu to its initial values before render
            nextMenu.Reset();
            AddRenderObject(nextMenu);
        }

        /// <summary>
        /// exit the application
        /// </summary>
        private void QuitCallback()
        {
            Display.StopRendering();
        }
    }
}
