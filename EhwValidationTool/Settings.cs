using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace EhwValidationTool
{
    public class Settings
    {
        private const string SettingsFileName = "settings.json";

        public static Settings Default { get; set; }
        public static void LoadSettings()
        {
            if(File.Exists(SettingsFileName))
                Default = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFileName));
            else
                Default = new Settings();
        }
        public static void SaveSettings()
        {
            File.WriteAllText(SettingsFileName, JsonConvert.SerializeObject(Default));
        }
        static Settings()
        {
            LoadSettings();
        }


        public string CpuzLocation { get; set; } = @"C:\Program Files\CPUID\CPU-Z\cpuz.exe";
        public string GpuzLocation { get; set; } = @"C:\Program Files (x86)\GPU-Z\gpu-z.exe";
        public string ScreenshotFolder { get; set; } = @"%USERPROFILE%\Desktop\HWBOT Screenshots\";
        public string HwbotUserName { get; set; }
        public string HwbotTeamName { get; set; }
        public bool EnableSlowMode { get;set; }
        public bool EnableSpdTabsSlot1Slot2 { get; set; }
        public bool EnableSpdTabsSlot2Slot4 { get; set; }

        public int UserInfoZOrder { get; set; }
        public Rectangle UserInfoSavedLayout { get; set; }
        public List<ToolSaveInfo> SavedLayout { get; set; }

        public bool Validate()
        {
            if (!File.Exists(CpuzLocation))
                return false;

            if (!File.Exists(GpuzLocation))
                return false;

            return true;
        }
    }
}
