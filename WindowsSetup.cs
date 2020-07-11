using System;
using System.Runtime.InteropServices;

namespace MazeGame
{
    /// <summary>
    /// Setup the windows console to accept ansi escape sequences
    /// </summary>
    public static class WindowsSetup
    {
        private const int StdOutputHandle = -11;
        private const uint EnableVirtualTerminalProcessing = 0x0004;
        private const uint DisableNewlineAutoReturn = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        public static void SetupConsole()
        {
            var iStdOut = GetStdHandle(StdOutputHandle);
            if (!GetConsoleMode(iStdOut, out var outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= EnableVirtualTerminalProcessing | DisableNewlineAutoReturn;
            if (SetConsoleMode(iStdOut, outConsoleMode)) return;
            
            Console.WriteLine($"failed to set output console mode, error code: {GetLastError().ToString()}");
            Console.ReadKey();
            return;
        }
    }
}