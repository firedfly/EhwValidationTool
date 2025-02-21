using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace EhwValidationTool
{

    public static class ToolLauncher
    {
        public static async Task<List<Process>> LaunchTools(List<ToolLaunchInfo> toolList, bool slowMode)
        {
            if (slowMode)
            {
                var tasks = new List<Process>();
                foreach(ToolLaunchInfo launchInfo in toolList)
                {
                    var process = await Launch(launchInfo);
                    tasks.Add(process);
                }
                return tasks;
            }
            else
            {
                return await TaskEx.Run(() =>
                {
                    var tasks = new List<Task<Process>>();
                    foreach (var launchInfo in toolList)
                    {
                        tasks.Add(Launch(launchInfo));

                    }
                    Task.WaitAll(tasks.ToArray());
                    return tasks.Select(x => x.Result).ToList();
                });
            }
        }

        public static async Task<Process> Launch(ToolLaunchInfo launchInfo)
        {
            var toolPath = GetToolPath(launchInfo.ToolType);

            var process = Process.Start(toolPath);

            bool loop = true;
            while (loop)
            {
                await TaskEx.Delay(250);

                var windows = Win32Interop.GetRootWindowsOfProcess(process.Id);

                //Console.WriteLine($"[{DateTime.Now}] {toolType} ({process.Id})");
                foreach (var window in windows)
                {
                    var windowText = Win32Interop.GetWindowText(window);
                    if(!isMainToolWindow(launchInfo.ToolType, windowText))
                        continue;

                    var rect = new Win32Interop.RECT();
                    Win32Interop.GetWindowRect(window, out rect);
                    var width = rect.right - rect.left;
                    var height = rect.bottom - rect.top;
                    
                    int x = 0, y = 0, userInfoLeft = 0, userInfoTop = 0;
                    if(launchInfo.ToolLocation == ToolLocation.TopLeft)
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
                    break;
                }
            }

            return process;
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

        public static string GetToolPath(ToolType toolType)
        {
            switch (toolType)
            {
                case ToolType.CpuZ: return Settings.Default.CpuzLocation;
                case ToolType.GpuZ: return Settings.Default.GpuzLocation;
                default: return null;
            }
        }
    }
}
