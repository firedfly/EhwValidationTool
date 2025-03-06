using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static EhwValidationTool.MainForm;

namespace EhwValidationTool
{
    public static class Win32Interop
    {
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        ///     Copies the text of the specified window's title bar (if it has one) into a buffer. If the specified window is a
        ///     control, the text of the control is copied. However, GetWindowText cannot retrieve the text of a control in another
        ///     application.
        ///     <para>
        ///     Go to https://msdn.microsoft.com/en-us/library/windows/desktop/ms633520%28v=vs.85%29.aspx  for more
        ///     information
        ///     </para>
        /// </summary>
        /// <param name="hWnd">
        ///     C++ ( hWnd [in]. Type: HWND )<br />A <see cref="IntPtr" /> handle to the window or control containing the text.
        /// </param>
        /// <param name="lpString">
        ///     C++ ( lpString [out]. Type: LPTSTR )<br />The <see cref="StringBuilder" /> buffer that will receive the text. If
        ///     the string is as long or longer than the buffer, the string is truncated and terminated with a null character.
        /// </param>
        /// <param name="nMaxCount">
        ///     C++ ( nMaxCount [in]. Type: int )<br /> Should be equivalent to
        ///     <see cref="StringBuilder.Length" /> after call returns. The <see cref="int" /> maximum number of characters to copy
        ///     to the buffer, including the null character. If the text exceeds this limit, it is truncated.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the length, in characters, of the copied string, not including
        ///     the terminating null character. If the window has no title bar or text, if the title bar is empty, or if the window
        ///     or control handle is invalid, the return value is zero. To get extended error information, call GetLastError.<br />
        ///     This function cannot retrieve the text of an edit control in another application.
        /// </returns>
        /// <remarks>
        ///     If the target window is owned by the current process, GetWindowText causes a WM_GETTEXT message to be sent to the
        ///     specified window or control. If the target window is owned by another process and has a caption, GetWindowText
        ///     retrieves the window caption text. If the window does not have a caption, the return value is a null string. This
        ///     behavior is by design. It allows applications to call GetWindowText without becoming unresponsive if the process
        ///     that owns the target window is not responding. However, if the target window is not responding and it belongs to
        ///     the calling application, GetWindowText will cause the calling application to become unresponsive. To retrieve the
        ///     text of a control in another process, send a WM_GETTEXT message directly instead of calling GetWindowText.<br />For
        ///     an example go to
        ///     <see cref="!:https://msdn.microsoft.com/en-us/library/windows/desktop/ms644928%28v=vs.85%29.aspx#sending">
        ///     Sending a
        ///     Message.
        ///     </see>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        public static List<IntPtr> GetRootWindowsOfProcess(int pid)
        {
            List<IntPtr> rootWindows = GetChildWindows(IntPtr.Zero);
            List<IntPtr> dsProcRootWindows = new List<IntPtr>();
            foreach (IntPtr hWnd in rootWindows)
            {
                uint lpdwProcessId;
                GetWindowThreadProcessId(hWnd, out lpdwProcessId);
                if (lpdwProcessId == pid)
                    dsProcRootWindows.Add(hWnd);
            }
            return dsProcRootWindows;
        }

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                Win32Callback childProc = new Win32Callback(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);


        /// <summary>
        /// Determines the depth of the specified window handle from the ancestor window handle.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="parent"></param>
        /// <param name="currentDepth"></param>
        /// <returns></returns>
        public static int GetWindowDepth(IntPtr control, IntPtr parent, int currentDepth = 0)
        {
            if (control == parent)
                return currentDepth;

            return GetWindowDepth(GetParent(control), parent, currentDepth + 1);
        }

        public const UInt32 TCM_FIRST = 0x1300;
        public const UInt32 TCM_SETCURFOCUS = (TCM_FIRST + 48);
        public const UInt32 CB_SETCURSEL = 0x014E;
        public const UInt32 WM_COMMAND = 0x0111;       // Windows message for commands
        public const UInt32 CBN_SELCHANGE = 1;         // Notification for combobox selection change
        private const uint GW_HWNDNEXT = 2;            // Get the next window in Z-order


        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);


        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);


        [DllImport("user32.dll")]
        static extern uint RealGetWindowClass(IntPtr hwnd, [Out] StringBuilder pszType,
           uint cchType);


        public static string RealGetWindowClassM(IntPtr hWnd)
        {
            StringBuilder pszType = new StringBuilder();
            pszType.Capacity = 255;
            RealGetWindowClass(hWnd, pszType, (UInt32)pszType.Capacity);
            return pszType.ToString();
        }

        public static IntPtr GetFirstComboBoxControl(IntPtr hwnd)
        {
            var windows = GetChildWindows(hwnd);
            foreach (var window in windows)
            {
                var classNN = RealGetWindowClassM(window);
                if (classNN.StartsWith("ComboBox"))
                {
                    return window;
                }
            }

            return IntPtr.Zero;
        }

        public static int GetWindowZOrder(IntPtr hwndTarget)
        {
            if (hwndTarget == IntPtr.Zero)
            {
                return -1; // Invalid handle
            }

            // Start with the topmost window
            IntPtr hwnd = GetTopWindow(IntPtr.Zero);
            int zOrder = 0;

            // Traverse the Z-order
            while (hwnd != IntPtr.Zero)
            {
                if (hwnd == hwndTarget)
                {
                    return zOrder; // Found the target window
                }

                hwnd = GetWindow(hwnd, GW_HWNDNEXT); // Get the next window in Z-order
                zOrder++;
            }

            return -1; // Target window not found
        }

        public static void SelectComboBoxValueByIndex(IntPtr comboboxHandle, int selectedIndex)
        {
            SendMessage(comboboxHandle, CB_SETCURSEL, selectedIndex, null);
            SendMessage(GetParent(comboboxHandle), (int)WM_COMMAND, MAKEWPARAM(GetDlgCtrlID(comboboxHandle), (int)CBN_SELCHANGE), (int)comboboxHandle);
        }


        public static IntPtr GetFirstTabControl(IntPtr hwnd)
        {
            var windows = GetChildWindows(hwnd);
            List<(IntPtr hwnd, int depthFromRootWindow)> tabList = new List<(IntPtr hwnd, int depthFromRootWindow)>();
            foreach (var window in windows)
            {
                var classNN = RealGetWindowClassM(window);
                if (classNN.StartsWith("SysTabControl"))
                {
                    tabList.Add((window, GetWindowDepth(window, hwnd)));
                }
            }

            return tabList.OrderBy(c => c.depthFromRootWindow).Select(c => c.hwnd).First();
        }

        // assumes hwnd is a SysTabControl
        public static IntPtr GetTabHwndByIndex(IntPtr hwnd, int tabIndex)
        {
            var windows = GetChildWindows(hwnd);

            foreach (var window in windows)
            {
                var classNN = RealGetWindowClassM(window);
                if (string.Equals(classNN, "#32770"))
                    tabIndex--;

                if (tabIndex < 0)
                    return window;
            }

            return IntPtr.Zero;
        }

        public static void SelectTabByIndex(IntPtr tabControlHandle, int tabIndex)
        {
            SendMessage(tabControlHandle, TCM_SETCURFOCUS, tabIndex, null);
        }
        public static int MAKEWPARAM(int low, int high)
        {
            return (low & 0xFFFF) | ((high & 0xFFFF) << 16);
        }



        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetDlgCtrlID(IntPtr hWnd);

        public static readonly IntPtr HWND_TOP = new IntPtr(0);        // Places the window at the top of the Z-order
        public const uint SWP_NOSIZE = 0x0001;       // Retains the current size
        public const uint SWP_NOMOVE = 0x0002;       // Retains the current position;
        public const uint SWP_NOACTIVATE = 0x0010;

            [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


        public static void BringWindowToFront(IntPtr hWnd)
        {
            var a = SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }
    }
}
