using CefSharp;
using CefSharp.OffScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolUpdateChecker.Updaters
{
    internal class CpuzUpdater
    {
        internal static CpuzUpdater Instance { get; } = new CpuzUpdater();


        private List<CpuzVersionInfo> OrderedVersions;

        public CpuzUpdater() { }

        public async Task<CpuzVersionInfo> GetLatestAvailableVersion()
        {
            await updateAvailableVersions();
            return OrderedVersions.First();
        }

        private async Task updateAvailableVersions()
        {
            if (OrderedVersions == null)
            {
                var browser = MainForm.WebBrowser;
                browser.Load("https://www.cpuid.com/softwares/cpu-z.html");

                var initialLoadResponse = await browser.WaitForInitialLoadAsync();
                if (!initialLoadResponse.Success)
                {
                    throw new Exception(string.Format("Page load failed with ErrorCode:{0}, HttpStatusCode:{1}", initialLoadResponse.ErrorCode, initialLoadResponse.HttpStatusCode));
                }


                var html = await browser.GetSourceAsync();

                // get all cpu-z portable download links
                var cpuzRegex = new Regex("/downloads/cpu-z/cpu-z_(?<version>[0-9]+[.]([0-9]+[.])*[0-9]+)-en.zip");
                var matches = cpuzRegex.Matches(html);
                var cpuzVersions = new List<CpuzVersionInfo>();
                foreach (Match match in matches)
                {
                    var groups = match.Groups;
                    cpuzVersions.Add(new CpuzVersionInfo(groups["version"].Value, match.Value));
                }

                OrderedVersions = cpuzVersions.Distinct().OrderByDescending(x => new Version(x.Version)).ToList();
            }
        }

        internal class CpuzVersionInfo
        {
            public string Version { get; private set; }
            public string DownloadPath { get; private set; }

            public CpuzVersionInfo(string version, string downloadPath)
            {
                Version = version;
                DownloadPath = downloadPath;
            }
        }
    }
}
