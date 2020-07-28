using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MazeGame.Engine.RenderObjects
{
    public class FPSTimer : RenderObject
    {
        private Stopwatch _fpsTimer = Stopwatch.StartNew();
        private int _numberUpdates = 0;
        private double _averageFps = 0.0;
        
        public override void Update(UpdateInfo updateInfo)
        {
            _numberUpdates++;

            if (_numberUpdates % 10 != 0) _averageFps = _numberUpdates / (_fpsTimer.ElapsedMilliseconds / 1000f);
            
            if (_fpsTimer.ElapsedMilliseconds <= 5000) return;
            
            _fpsTimer.Restart();
            _numberUpdates = 0;
            Debug.WriteLine($"Avg FPS: {_averageFps:F2}");
        }

        public override void Render()
        {
            Content = new [] {$"FPS: Check console"};
        }
    }
}