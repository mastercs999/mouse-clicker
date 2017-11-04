using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MouseClicker
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);




        static void Main(string[] args)
        {
            // We will be infinitely making click if needed
            while (true)
            {
                if (CheckClick())
                    DoMouseClick(1);

                Thread.Sleep(1);
            }
        }

        private static bool CheckClick()
        {
            // Was middle mouse button clicked?
            short status = GetAsyncKeyState((int)Keys.MButton);
            
            return status == -32767 || status == -32768;
        }

        private static void DoMouseClick(int count)
        {
            // Get cursor's position
            GetCursorPos(out Point lpPoint);

            // Find window where we want to send mouse click
            IntPtr parentWindow = FindWindow(null, "Crusader");

            // Prepare click argument
            IntPtr lParam = (IntPtr)((lpPoint.Y << 16) | lpPoint.X);

            for (int i = 0; i < count; ++i)
            {
                // Send button push down message
                PostMessage(parentWindow, 0x0201, (IntPtr)1, lParam);
                Thread.Sleep(5);

                // Send button released message
                PostMessage(parentWindow, 0x0202, IntPtr.Zero, lParam);
                Thread.Sleep(5);
            }
        }
    }
}
