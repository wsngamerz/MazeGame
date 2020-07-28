using System;
using System.Diagnostics;
using MazeGame.Engine;
using MazeGame.Engine.RenderObjects;

namespace MazeGame.Scenes
{
    public class MainMenuScene : Scene
    {
        private Menu _mainMenu;
        private Menu _playMenu;
        private Menu _editorMenu;
        private Menu _optionsMenu;
        
        public MainMenuScene()
        {
            AddMenus();
        }

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

        private void DisplayPlayMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_playMenu);
            Debug.WriteLine("Play option selected");
        }

        private void DisplayEditorMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_editorMenu);
        }

        private void DisplayOptionsMenu()
        {
            RemoveRenderObject(_mainMenu);
            AddRenderObject(_optionsMenu);
        }

        private void DisplayMainMenu(Menu previousMenu)
        {
            RemoveRenderObject(previousMenu);
            AddRenderObject(_mainMenu);
        }

        private void SwitchTestScene()
        {
            Display.SwitchScene("testScene");
        }
        
        private void QuitCallback()
        {
            Environment.Exit(0);
        }
    }
}