using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EhwValidationTool
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main(string[] args)
        {
            var shouldExit = await processOptions(args);
            if (shouldExit)
                return;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ApplicationExit += (object sender, EventArgs e) => { MainForm.Instance.CloseTools(); };
            Application.Run(new MainForm());
        }

        private static async Task<bool> processOptions(string[] args)
        {
            var options = Parser.Default.ParseArguments<ProgramOptions>(args);
            if (options.Value?.Install == true)
            {
                await installUpdate();
                // launch new version with flag to cleanup install files

                // return true to exit
                return true;
            }
            if (options.Value?.Cleanup == true)
            {
                cleanupUpdateFiles();
            }
            return false;
        }

        /// <summary>
        /// Delete the temporary update files directory
        /// </summary>
        private static void cleanupUpdateFiles()
        {
            var currentDir = Path.GetDirectoryName(Application.ExecutablePath);
            var updateDir = Path.Combine(currentDir, "update");

            if (Directory.Exists(updateDir))
                Directory.Delete(updateDir, true);
        }

        /// <summary>
        /// Deletes existing files, copies new files to the correct location.
        /// </summary>
        /// <returns></returns>
        private static async Task installUpdate()
        {
            await checkForOtherInstances();

            // update files will be in an "update" directory.  install location will be the parent directory of the update directory

            var sourceDir = Path.GetDirectoryName(Application.ExecutablePath);
            var destDir = Path.GetDirectoryName(sourceDir);

            // delete old files
            var filesToIgnore = new string[] { "settings.json" };
            foreach (var fileToDelete in Directory.GetFiles(destDir))
            {
                if (filesToIgnore.Contains(Path.GetFileName(fileToDelete)))
                    continue;

                File.Delete(fileToDelete);
            }

            // copy new files
            foreach (var fileToCopy in Directory.GetFiles(sourceDir))
            {
                if (filesToIgnore.Contains(Path.GetFileName(fileToCopy)))
                    continue;

                File.Copy(fileToCopy, Path.Combine(destDir, Path.GetFileName(fileToCopy)));
            }

            // launch the new version
            await TaskEx.Delay(1000);
            var exePath = Path.Combine(destDir, Path.GetFileName(Application.ExecutablePath));
            var processStartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "--cleanup",
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }

        private static async Task checkForOtherInstances()
        {
            var currentProcess = Process.GetCurrentProcess();
            var otherProcesses = Process.GetProcessesByName(currentProcess.ProcessName).Where(o => o.Id != currentProcess.Id).ToList();

            if(otherProcesses.Count > 0)
            {
                var result = MessageBox.Show($"Another instance of the EHW Validation Tool is running.  Before installing the new update, other instances must be closed.{Environment.NewLine}{Environment.NewLine}Automatically exit other instances?", "Other Instances are Running", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    foreach (var process in otherProcesses)
                    {
                        process.Kill();
                    }

                    while (Process.GetProcessesByName(currentProcess.ProcessName).Where(o => o.Id != currentProcess.Id).Count() > 0)
                        await TaskEx.Delay(250);
                }
                else
                {
                    MessageBox.Show("You chose to keep the other instances of the EHW Validaton Tool running.  Installation has been aborted.");
                    Application.Exit();
                }
            }
        }
    }
}
