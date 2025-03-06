using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace XtremeCapture
{

    public static class ToolLauncher
    {
        private static List<ToolLaunchInfo> _openTools = new List<ToolLaunchInfo>();


        public static bool HasOpenTools => _openTools.Any(x=>x.Process?.HasExited == false);


        /// <summary>
        /// Launches the tools and updates the launch info for each tool to specify the tools processid.
        /// </summary>
        /// <param name="toolList"></param>
        /// <param name="slowMode"></param>
        public static async Task LaunchTools(List<ToolLaunchInfo> toolList, bool slowMode)
        {
            CloseTools();
            _openTools = toolList;

            if (slowMode)
            {
                foreach(ToolLaunchInfo launchInfo in toolList.OrderByDescending(x=>x.ZOrder))
                {
                    await Launch(launchInfo);
                }
            }
            else
            {
                await TaskEx.Run(() =>
                {
                    var tasks = new List<Task>();
                    foreach (var launchInfo in toolList.OrderByDescending(x => x.ZOrder))
                    {
                        tasks.Add(Launch(launchInfo));

                    }
                    Task.WaitAll(tasks.ToArray());
                });
            }
        }

        public static void CloseTools()
        {
            _openTools.ForEach(x =>
            {
                var process = x.Process;
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }
            });
            _openTools.Clear();
        }

        public static IntPtr GetMainWindowFromProcessId(ToolLaunchInfo launchInfo, int processId)
        {
            var windows = Win32Interop.GetRootWindowsOfProcess(processId);

            foreach (var window in windows)
            {
                var windowText = Win32Interop.GetWindowText(window);
                if (isMainToolWindow(launchInfo.ToolType, windowText))
                    return window;
            }

            return IntPtr.Zero;
        }

        private static async Task Launch(ToolLaunchInfo launchInfo)
        {
            var toolPath = GetToolPath(launchInfo.ToolType);

            var process = Process.Start(toolPath);
            launchInfo.Process = process;

            bool loop = true;
            while (loop)
            {
                await TaskEx.Delay(250);

                var window = GetMainWindowFromProcessId(launchInfo, process.Id);
                if (window == IntPtr.Zero)
                    continue;

                var rect = new Win32Interop.RECT();
                Win32Interop.GetWindowRect(window, out rect);
                var width = rect.right - rect.left;
                var height = rect.bottom - rect.top;

                int x = 0, y = 0, userInfoLeft = 0, userInfoTop = 0;
                if (launchInfo.IsCustomLayout)
                {
                    x = launchInfo.Location.X;
                    y = launchInfo.Location.Y;
                    width = launchInfo.Location.Width;
                    height = launchInfo.Location.Height;
                }
                else if(launchInfo.ToolLocation == ToolLocation.TopLeft)
                {
                    x = width * (launchInfo.InstanceNumber - 1);
                    y = 0;

                    if (launchInfo.DisplayUserInfoAboveWindow)
                    {
                        userInfoLeft = x;
                        userInfoTop = height;
                    }
                }
                else if(launchInfo.ToolLocation == ToolLocation.TopRight)
                {
                    x = Screen.PrimaryScreen.WorkingArea.Width - (width * launchInfo.InstanceNumber);
                    y = 0;

                    if (launchInfo.DisplayUserInfoAboveWindow)
                    {
                        userInfoLeft = x;
                        userInfoTop = height;
                    }
                }
                else if(launchInfo.ToolLocation == ToolLocation.BottomLeft)
                {
                    x = width * (launchInfo.InstanceNumber - 1);
                    y = Screen.PrimaryScreen.WorkingArea.Height - height;

                    if (launchInfo.DisplayUserInfoAboveWindow)
                    {
                        userInfoLeft = x;
                        userInfoTop = y - MainForm.Instance.GetUserInfoFormHeight();
                    }
                }
                else if(launchInfo.ToolLocation == ToolLocation.BottomRight)
                {
                    x = Screen.PrimaryScreen.WorkingArea.Width - (width * launchInfo.InstanceNumber);
                    y = Screen.PrimaryScreen.WorkingArea.Height - height;

                    if (launchInfo.DisplayUserInfoAboveWindow)
                    {
                        userInfoLeft = x;
                        userInfoTop = y - MainForm.Instance.GetUserInfoFormHeight();
                    }
                }

                if (launchInfo.DisplayUserInfoAboveWindow)
                {
                    MainForm.Instance.ShowUserInfoForm(userInfoLeft, userInfoTop, width);
                }

                for(int i = 0; i < 100; i++)
                {
                    var moved = Win32Interop.MoveWindow(window, x, y, width, height, true);
                    await TaskEx.Delay(500);


                    Win32Interop.GetWindowRect(window, out rect);
                    if (rect.left == x && rect.top == y)
                        break;
                }

                //Console.WriteLine($"[{DateTime.Now}] {toolType} ({process.Id}) Moved: {moved}");

                // select the specified tab
                if(launchInfo.SelectTabIndex != null)
                {
                    var tabHandle = Win32Interop.GetFirstTabControl(window);
                    if (tabHandle != IntPtr.Zero)
                    {
                        Win32Interop.SelectTabByIndex(tabHandle, launchInfo.SelectTabIndex.Value);

                        var cpuzLaunchInfo = launchInfo as CpuzLaunchInfo;
                        if (cpuzLaunchInfo != null && cpuzLaunchInfo.TabType == CpuzLaunchInfo.CpuzTabType.SPD && cpuzLaunchInfo.SpdSlot.HasValue)
                        {
                            var tabPageHwnd = Win32Interop.GetTabHwndByIndex(tabHandle, launchInfo.SelectTabIndex.Value);
                            var comboboxHwnd = Win32Interop.GetFirstComboBoxControl(tabPageHwnd);
                            Win32Interop.SelectComboBoxValueByIndex(comboboxHwnd, cpuzLaunchInfo.SpdSlot.Value-1);
                        }
                    }
                }

                loop = false;
            }

            return;
        }

        private static bool isMainToolWindow(ToolType toolType, string windowText)
        {
            if(String.IsNullOrWhiteSpace(windowText))
                return false;

            if (toolType == ToolType.GpuZ)
            {
                if (windowText.Contains("GPU-Z"))
                    return true;
            }
            else if(toolType == ToolType.CpuZ)
            {
                if (windowText.Contains("CPU-Z Update"))
                    return false;
                else if (windowText.Contains("CPU-Z"))
                    return true;
            }

            return false;
        }

        private static string getToolWindowTextSearchPattern(ToolType toolType)
        {
            switch(toolType)
            {
                case ToolType.CpuZ: return "CPU-Z";
                case ToolType.GpuZ: return "GPU-Z";
                default: return null;
            }
        }

        private static ToolSaveInfo createSaveInfo(ToolLaunchInfo tool, Rectangle location, int zOrder)
        {
            if(tool.ToolType == ToolType.CpuZ)
            {
                var cpuzInfo = (CpuzLaunchInfo)tool;
                return new CpuzSaveInfo
                {
                    Location = location,
                    ZOrder = zOrder,
                    TabType = cpuzInfo.TabType,
                    SpdSlot = cpuzInfo.SpdSlot
                };
            }

            return new ToolSaveInfo
            {
                ToolType = tool.ToolType,
                Location = location,
                ZOrder = zOrder
            };
        }

        public static string GetToolPath(ToolType toolType)
        {
            switch (toolType)
            {
                case ToolType.CpuZ: return Settings.Default.CpuzLocation;
                case ToolType.GpuZ: return Settings.Default.GpuzLocation;
                default: return null;
            }
        }


        public static List<ToolSaveInfo> GetSaveInfoForTools()
        {
            var saveInfo = new List<ToolSaveInfo>();

            foreach (var tool in _openTools)
            {
                if(tool.Process == null || tool.Process.HasExited)
                    continue;

                var hwnd = GetMainWindowFromProcessId(tool, tool.Process.Id);
                var rect = new Win32Interop.RECT();
                Win32Interop.GetWindowRect(hwnd, out rect);

                var zOrder = Win32Interop.GetWindowZOrder(hwnd);
                var location = new System.Drawing.Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

                saveInfo.Add(createSaveInfo(tool, location, zOrder));
            }

            return saveInfo;
        }

        public static async Task<List<ToolLaunchInfo>> LaunchToolsFromSavedLayout(List<ToolSaveInfo> saveInfo, bool slowMode)
        {
            CloseTools();
            var launchInfo = new List<ToolLaunchInfo>();
            foreach (var toolSaveInfo in saveInfo)
            {
                if(toolSaveInfo.ToolType == ToolType.CpuZ)
                {
                    var cpuzSaveInfo = (CpuzSaveInfo)toolSaveInfo;
                    launchInfo.Add(new CpuzLaunchInfo
                    {
                        TabType = cpuzSaveInfo.TabType,
                        SpdSlot = cpuzSaveInfo.SpdSlot,
                        IsCustomLayout = true,
                        Location = cpuzSaveInfo.Location,
                        ZOrder = cpuzSaveInfo.ZOrder
                    });
                }
                else
                {
                    launchInfo.Add(new ToolLaunchInfo
                    {
                        IsCustomLayout = true,
                        ToolType = toolSaveInfo.ToolType,
                        Location = toolSaveInfo.Location,
                        ZOrder = toolSaveInfo.ZOrder
                    });
                }
            }

            await LaunchTools(launchInfo, slowMode);
            return _openTools;
        }
    }
}
