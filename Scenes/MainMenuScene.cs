using System;
using System.Collections.Generic;
using System.Linq;
using MazeGame.Engine;
using MazeGame.RenderObjects;

namespace MazeGame.Scenes
{
    public class MainMenuScene : Scene
    {
        // menus
        private Menu _mainMenu;
        private Menu _playMenu;
        private Menu _editorMenu;
        private Menu _optionsMenu;

        private Image _mainLogo;
        private bool _logoPositioned;

        /// <summary>
        /// Scene start method
        /// </summary>
        public override void Start()
        {
            base.Start();
            
            AddMenus();
            AddLogo();
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

            if (_logoPositioned && !updateInfo.HasResized) return;
            _mainLogo.Position = new Vector2((Display.Width / 2) - (_mainLogo.Size.X / 2),1);
            _logoPositioned = true;
        }

        /// <summary>
        /// Add the logo
        /// </summary>
        private void AddLogo()
        {
            _mainLogo = new Image("main_menu_logo.json", Vector2.Zero);
            AddRenderObject(_mainLogo);
            _logoPositioned = false;
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
            _editorMenu.AddItem("Create new Maze", CreateNewMaze);
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
        /// Called when the "Create new maze" option is selected
        /// asks for user input and then switches scenes to the maze editor
        /// </summary>
        private void CreateNewMaze()
        {
            // stop rendering the editor menu
            RemoveRenderObject(_editorMenu);

            // calculate the position of where to draw the input box
            var nameInputPos = new Vector2((Display.Width/2) - (TextInput.InputWidth/2), (Display.Height / 2) - 3);
            
            // text input with a submit handler
            var nameInput = new TextInput("New Maze Name", nameInputPos);
            nameInput.OnSubmit = (value) =>
            {
                // switch to the mazeEditorScene and pass it the name of the new maze
                Display.SwitchScene("mazeEditor");
                var scene = (MazeEditorScene) Display.CurrentScene;
                scene.MazeName = value;
                
                // remove the name input from the scene render list
                RemoveRenderObject(nameInput);
            };
            
            // render the object
            AddRenderObject(nameInput);
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
