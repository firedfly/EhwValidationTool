﻿using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using static EhwValidationTool.MainForm;
using System.Windows.Forms;
using System.IO;

namespace EhwValidationTool
{
    public static class ToolLauncher
    {
        public static async Task<Process> Launch(ToolType toolType, ToolLocation toolLocation, int instanceNumber = 1, bool displayUserInfoAboveWindow = false)
        {
            var toolPath = getToolPath(toolType);
            var toolWindowTextSearchPattern = getToolWindowTextSearchPattern(toolType);

            var process = Process.Start(toolPath);

            bool loop = true;
            while (loop)
            {
                await Task.Delay(250);

                var windows = Win32Interop.GetRootWindowsOfProcess(process.Id);

                //Console.WriteLine($"[{DateTime.Now}] {toolType} ({process.Id})");
                foreach (var window in windows)
                {
                    var windowText = Win32Interop.GetWindowText(window);
                    if(String.IsNullOrEmpty(windowText) || !windowText.Contains(toolWindowTextSearchPattern))
                        continue;

                    var rect = new Win32Interop.RECT();
                    Win32Interop.GetWindowRect(window, out rect);
                    var width = rect.right - rect.left;
                    var height = rect.bottom - rect.top;
                    
                    int x = 0, y = 0, userInfoLeft = 0, userInfoTop = 0;
                    if(toolLocation == ToolLocation.TopLeft)
                    {
                        x = width * (instanceNumber - 1);
                        y = 0;

                        if (displayUserInfoAboveWindow)
                        {
                            userInfoLeft = x;
                            userInfoTop = height;
                        }
                    }
                    else if(toolLocation == ToolLocation.TopRight)
                    {
                        x = Screen.PrimaryScreen.WorkingArea.Width - (width * instanceNumber);
                        y = 0;

                        if (displayUserInfoAboveWindow)
                        {
                            userInfoLeft = x;
                            userInfoTop = height;
                        }
                    }
                    else if(toolLocation == ToolLocation.BottomLeft)
                    {
                        x = width * (instanceNumber - 1);
                        y = Screen.PrimaryScreen.WorkingArea.Height - height;

                        if (displayUserInfoAboveWindow)
                        {
                            userInfoLeft = x;
                            userInfoTop = y - MainForm.Instance.GetUserInfoFormHeight();
                        }
                    }
                    else if(toolLocation == ToolLocation.BottomRight)
                    {
                        x = Screen.PrimaryScreen.WorkingArea.Width - (width * instanceNumber);
                        y = Screen.PrimaryScreen.WorkingArea.Height - height;

                        if (displayUserInfoAboveWindow)
                        {
                            userInfoLeft = x;
                            userInfoTop = y - MainForm.Instance.GetUserInfoFormHeight();
                        }
                    }

                    if (toolType == ToolType.GpuZ)
                        await Task.Delay(2000);

                    if (displayUserInfoAboveWindow)
                    {
                        MainForm.Instance.ShowUserInfoForm(userInfoLeft, userInfoTop, width);
                    }

                    var moved = Win32Interop.MoveWindow(window, x, y, width, height, true);
                    //Console.WriteLine($"[{DateTime.Now}] {toolType} ({process.Id}) Moved: {moved}");
                    loop = false;
                    break;
                }
            }

            return process;
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