namespace FreeEcho
{
    namespace FEWindowMoveDetectionMouse
    {
        /// <summary>
        /// NativeMethods
        /// </summary>
        internal class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern System.IntPtr SetWindowsHookEx(int idHook, NativeMethodsDelegate.MouseHookCallback lpfn, System.IntPtr hMod, uint dwThreadId);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern System.IntPtr CallNextHookEx(System.IntPtr hhk, int nCode, uint msg, ref MSLLHOOKSTRUCT msllhookstruct);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool UnhookWindowsHookEx(System.IntPtr hhk);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool GetCursorPos(out POINT lpPoint);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern System.IntPtr WindowFromPoint(POINT pOINT);
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            public static extern bool GetWindowRect(System.IntPtr hwnd, out RECT lpRect);
        }
    }
}
