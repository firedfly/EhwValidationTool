using static XtremeCapture.CpuzLaunchInfo;

namespace XtremeCapture
{
    public class CpuzSaveInfo : ToolSaveInfo
    {
        public override ToolType ToolType => ToolType.CpuZ;

        public CpuzTabType TabType { get; set; }
        public int? SpdSlot { get; set; }
    }
}
