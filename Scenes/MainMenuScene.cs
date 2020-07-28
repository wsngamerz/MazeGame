using System;
using System.Diagnostics;
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
        /// Main menu scene
        /// </summary>
        public MainMenuScene()
        {
            AddMenus();
        }

        /// <summary>
        /// add all of the main menus
        /// </summary>
        private void AddMenus()
        {
            // main menu
            _mainMenu = new Menu("Main Menu");
            _mainMenu.AddItem("Play", DisplayPlayMenu);
            _mainMenu.AddItem("Editor", DisplayEditorMenu);
            _mainMenu.AddItem("Options", DisplayOptionsMenu);
            _mainMenu.AddItem("Quit", QuitCallback);
            
            // play menu
            _playMenu = new Menu("Play");
            _playMenu.AddItem("Select Level");
            _playMenu.AddItem("Back", () => DisplayMainMenu(_playMenu));
            
            // editor menu
            _editorMenu = new Menu("Editor");
            _editorMenu.AddItem("Create new Maze");
            _editorMenu.AddItem("Edit maze");
            _editorMenu.AddItem("Back", () => DisplayMainMenu(_editorMenu));
            
            // options menu
            _optionsMenu = new Menu("Options");
            _optionsMenu.AddItem("Test Scene", SwitchTestScene);
            _optionsMenu.AddItem("Back", () => DisplayMainMenu(_optionsMenu));
            
            // add the main menu as the starting menu
            AddRenderObject(_mainMenu);
        }

        /// <summary>
        /// display the play menu and hide the main menu
        /// </summary>
        private void DisplayPlayMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_playMenu);
            Debug.WriteLine("Play option selected");
        }

        /// <summary>
        /// display the editor menu and hide the main menu
        /// </summary>
        private void DisplayEditorMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_editorMenu);
        }

        /// <summary>
        /// display the options menu and hide the main menu
        /// </summary>
        private void DisplayOptionsMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_optionsMenu);
        }

        /// <summary>
        /// display the main menu and hide the previous menu
        /// </summary>
        /// <param name="previousMenu"></param>
        private void DisplayMainMenu(Menu previousMenu)
        {
            RemoveRenderObject(previousMenu);
            AddRenderObject(_mainMenu);
        }

        /// <summary>
        /// switch scenes to the test scene
        /// </summary>
        private void SwitchTestScene()
        {
            Display.SwitchScene("testScene");
        }
        
        /// <summary>
        /// exit the application
        /// </summary>
        private void QuitCallback()
        {
            Environment.Exit(0);
        }
    }
}