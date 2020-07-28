using System;
using System.IO;
using Newtonsoft.Json;

namespace MazeGame.Engine.RenderObjects
{
    /// <summary>
    /// An image object which will display an "image" made from ascii characters and is loaded from a json file
    /// </summary>
    public class Image : RenderObject
    {
        private ImageData _imageData;
        private bool _hasRendered;
        private ImageColourOptions _foregroundColourOptions;
        private ImageColourOptions _backgroundColourOptions;
        
        // some defaults
        private const string DefaultForegroundColour = Style.ForegroundColor.White;
        private const string DefaultBackgroundColour = Style.BackgroundColor.Grayscale235;
        
        /// <summary>
        /// Initiate the image render object
        /// </summary>
        /// <param name="dataFilePath"></param>
        /// <param name="position"></param>
        public Image(string dataFilePath, Vector2 position)
        {
            _hasRendered = false;
            Position = position;
            LoadDataFile(dataFilePath);
        }

        /// <summary>
        /// Load the image data from the json file
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadDataFile(string filePath)
        {
            // load the file if it exists
            if (!File.Exists(Path.Combine("Data", filePath))) return;
            _imageData = JsonConvert.DeserializeObject<ImageData>(File.ReadAllText(Path.Combine("Data", filePath)));
            
            // grab some config options from the data file
            _foregroundColourOptions = _imageData.ForegroundColour.Length switch
            {
                0 => ImageColourOptions.Default,
                1 => ImageColourOptions.Single,
                _ => ImageColourOptions.Multiple
            };
            
            _backgroundColourOptions = _imageData.BackgroundColour.Length switch
            {
                0 => ImageColourOptions.Default,
                1 => ImageColourOptions.Single,
                _ => ImageColourOptions.Multiple
            };
        }
        
        /// <summary>
        /// An update function. Not used in this case as an image cannot (yet) be updated
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo) { }

        /// <summary>
        /// render method
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Render()
        {
            // only render to a single string array once
            if (_hasRendered) return;
            
            var render = new string[_imageData.Height];
            for (var i = 0; i < render.Length; i++)
            {
                // get a line of the "texture"
                string line = _imageData.Texture.Substring(i * _imageData.Width, _imageData.Width);

                // TODO: Implement multi-fg colours
                // add the foreground colour(s)
                switch (_foregroundColourOptions)
                {
                    case ImageColourOptions.Default:
                        line = $"{DefaultForegroundColour}{line}{Style.Reset}";
                        break;
                    case ImageColourOptions.Single:
                        line = $"{Style.ForegroundColor.FromString(_imageData.Colours[Convert.ToInt32(_imageData.ForegroundColour)])}{line}{Style.Reset}";
                        break;
                    case ImageColourOptions.Multiple:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // TODO: Implement multi-bg colours
                // add the background colour(s)
                switch (_backgroundColourOptions)
                {
                    case ImageColourOptions.Default:
                        line = $"{DefaultBackgroundColour}{line}";
                        break;
                    case ImageColourOptions.Single:
                        line = $"{Style.BackgroundColor.FromString(_imageData.Colours[Convert.ToInt32(_imageData.BackgroundColour)])}{line}";
                        break;
                    case ImageColourOptions.Multiple:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                render[i] = line;
            }

            // apply the render to content
            _hasRendered = true;
            Content = render;
        }
    }

    /// <summary>
    /// a class representation of the data json file
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Width of the image in characters
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// Height of the tetxure in characters
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// A single string containing the texture. The number of characters inside this string needs to be equal to that
        /// of Width * Height for the image to render properly
        /// </summary>
        public string Texture { get; set; }
        
        /// <summary>
        /// The foreground colour of the texture.
        /// - If empty, the default colour is used.
        /// - If is a single character, that is used as an index for the Colours array
        /// - If is multiple characters, the indexed position is used to style the corresponding character in the texture
        ///   with the colour that references the colour index
        /// </summary>
        public string ForegroundColour { get; set; }
        
        /// <summary>
        /// The background colour of the texture.
        /// - If empty, the default colour is used.
        /// - If is a single character, that is used as an index for the Colours array
        /// - If is multiple characters, the indexed position is used to style the corresponding character in the texture
        ///   with the colour that references the colour index
        /// </summary>
        public string BackgroundColour { get; set; }
        
        /// <summary>
        /// An array that holds the name of a colour that corresponds to both <see cref="Style.ForegroundColor"/> and
        /// <see cref="Style.BackgroundColor"/>
        /// </summary>
        public string[] Colours { get; set; }
    }

    /// <summary>
    /// Used internally to describe how to colour the texture
    /// </summary>
    public enum ImageColourOptions
    {
        Default,
        Single,
        Multiple
    }
}