using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using static BastiaansHwBotHelper.MainForm;
using System.Windows.Forms;
using System.IO;

namespace BastiaansHwBotHelper
{
    public static class ToolLauncher
    {
        public static async Task Launch(ToolType toolType, ToolLocation toolLocation, int instanceNumber = 1)
        {
            var toolPath = getToolPath(toolType);
            var toolMainWindowMinHeight = getToolMainWindowMinHeight(toolType);

            var process = Process.Start(toolPath);

            bool loop = true;
            while (loop)
            {
                await Task.Delay(250);

                var windows = Win32Interop.GetRootWindowsOfProcess(process.Id);

                foreach (var window in windows)
                {
                    var rect = new Win32Interop.RECT();
                    Win32Interop.GetWindowRect(window, out rect);
                    var width = rect.right - rect.left;
                    var height = rect.bottom - rect.top;

                    Console.WriteLine($"Window {window} Width: {width} Height: {height}");
                    if (height > toolMainWindowMinHeight)
                    {
                        int x = 0, y = 0;
                        if(toolLocation == ToolLocation.TopLeft)
                        {
                            x = width * (instanceNumber - 1);
                            y = 0;
                        }
                        else if(toolLocation == ToolLocation.TopRight)
                        {
                            x = Screen.PrimaryScreen.WorkingArea.Width - (width * instanceNumber);
                            y = 0;
                        }
                        else if(toolLocation == ToolLocation.BottomLeft)
                        {
                            x = width * (instanceNumber - 1);
                            y = Screen.PrimaryScreen.WorkingArea.Height - height;
                        }
                        else if(toolLocation == ToolLocation.BottomRight)
                        {
                            x = Screen.PrimaryScreen.WorkingArea.Width - (width * instanceNumber);
                            y = Screen.PrimaryScreen.WorkingArea.Height - height;
                        }

                        Win32Interop.MoveWindow(window, x, y, width, height, true);
                        loop = false;
                        break;
                    }
                }
            }
        }

        private static int getToolMainWindowMinHeight(ToolType toolType)
        {
            switch(toolType)
            {
                case ToolType.CpuZ: return 400;
                case ToolType.GpuZ: return 600;
                default: return 0;
            }
        }

        private static string getToolPath(ToolType toolType)
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
