using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace PS4GSCInjector
{
    internal static class WindowServices
    {
        private const int DwmwaUseImmersiveDarkMode = 20;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static void UseImmersiveDarkMode(Window window)
        {
            if (window == null)
            {
                return;
            }

            try
            {
                IntPtr handle = new WindowInteropHelper(window).EnsureHandle();
                int enabled = 1;
                DwmSetWindowAttribute(handle, DwmwaUseImmersiveDarkMode, ref enabled, sizeof(int));
            }
            catch
            {
                // Older Windows versions do not expose this attribute.
            }
        }
    }
}
