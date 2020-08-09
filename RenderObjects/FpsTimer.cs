using System.Diagnostics;
using System.Numerics;
using MazeGame.Engine;

namespace MazeGame.RenderObjects
{
    /// <summary>
    /// An FPS timer
    /// </summary>
    public class FpsTimer : RenderObject
    {
        private readonly Stopwatch _fpsTimer = Stopwatch.StartNew();
        private int _numberUpdates;
        private double _averageFps;

        public FpsTimer(Vector2 pos)
        {
            Position = pos;
            Enabled = true;
        }
        
        /// <summary>
        /// update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            _numberUpdates++;

            // only calculate the average fps every 10 frames
            if (_numberUpdates % 10 != 0) _averageFps = _numberUpdates / (_fpsTimer.ElapsedMilliseconds / 1000f);
            
            // restart the timer every 5 seconds to get a 5 second average
            if (_fpsTimer.ElapsedMilliseconds <= 5000) return;
            _fpsTimer.Restart();
            _numberUpdates = 0;
        }

        /// <summary>
        /// render method
        /// </summary>
        public override void Render()
        {
            Content = new [] {$"FPS: {_averageFps:F2}  "};
        }
    }
}
