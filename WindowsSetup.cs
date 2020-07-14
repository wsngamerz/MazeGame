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
        private const int StdInputHandle = -10;

        private const uint EnableExtendedFlags = 0x0080;
        private const uint EnableQuickEditMode = 0x0040;
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
            SetupInput();
            SetupOutput();
        }

        /// <summary>
        /// Setup the console input
        /// </summary>
        private static void SetupInput()
        {
            // get the input handle
            var iStdIn = GetStdHandle(StdInputHandle);
            
            // get the current input console mode
            if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
            {
                Console.WriteLine("failed to get input console mode");
                Console.ReadKey();
                return;
            }
            
            // add the flags to disable quick select mode
            inConsoleMode &= ~EnableQuickEditMode;
            inConsoleMode |= EnableExtendedFlags;
            
            // set the input console mode and return if successful
            if (SetConsoleMode(iStdIn, inConsoleMode)) return;
            Console.WriteLine($"failed to set input console mode, error code: {GetLastError().ToString()}");
            Console.ReadKey();
        }

        /// <summary>
        /// setup the console output
        /// </summary>
        private static void SetupOutput()
        {
            // get the output handle
            var iStdOut = GetStdHandle(StdOutputHandle);
            
            // get the current output console mode
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            // add the flags to enable vt support for colours and disable automatic newlines
            outConsoleMode |= EnableVirtualTerminalProcessing | DisableNewlineAutoReturn;

            // set the output console mode and return if successful
            if (SetConsoleMode(iStdOut, outConsoleMode)) return;
            Console.WriteLine($"failed to set output console mode, error code: {GetLastError().ToString()}");
            Console.ReadKey();
        }
    }
}