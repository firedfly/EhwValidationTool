using System.Diagnostics;
using System.Drawing;

namespace XtremeCapture
{
    public class ToolLaunchInfo
    {
        public virtual ToolType ToolType { get; set; }
        public ToolLocation ToolLocation { get; set; }
        public int InstanceNumber { get; set; } = 1;
        public bool DisplayUserInfoAboveWindow { get; set; } = false;
        public int? SelectTabIndex { get; set; }

        public bool IsCustomLayout { get; set; }
        public Rectangle Location { get; set; }
        public int ZOrder { get; set; }


        // When the tool is launched, the ProcessId will be set
        public Process Process { get; set; }
    }
}
