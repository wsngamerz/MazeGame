using System;
using System.IO;
using Newtonsoft.Json;

namespace MazeGame.Engine.RenderObjects
{
    public class Image : RenderObject
    {
        private ImageData _imageData;
        private bool _hasRendered;
        private ImageColourOptions _foregroundColourOptions;
        private ImageColourOptions _backgroundColourOptions;
        
        private const string DefaultForegroundColour = Style.ForegroundColor.White;
        private const string DefaultBackgroundColour = Style.BackgroundColor.Grayscale235;
        
        public Image(string dataFilePath, Vector2 position)
        {
            _hasRendered = false;
            Position = position;
            LoadDataFile(dataFilePath);
        }

        private void LoadDataFile(string filePath)
        {
            // load the file if it exists
            if (!File.Exists(Path.Combine("Data", filePath))) return;
            _imageData = JsonConvert.DeserializeObject<ImageData>(File.ReadAllText(Path.Combine("Data", filePath)));
            
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
        
        public override void Update(UpdateInfo updateInfo) { }

        public override void Render()
        {
            if (_hasRendered) return;
            
            var render = new string[_imageData.Height];
            for (var i = 0; i < render.Length; i++)
            {
                string line = _imageData.Texture.Substring(i * _imageData.Width, _imageData.Width);

                // TODO: Implement multi-fg colours
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

            _hasRendered = true;
            Content = render;
        }
    }

    public class ImageData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Texture { get; set; }
        public string ForegroundColour { get; set; }
        public string BackgroundColour { get; set; }
        public string[] Colours { get; set; }
    }

    public enum ImageColourOptions
    {
        Default,
        Single,
        Multiple
    }
}