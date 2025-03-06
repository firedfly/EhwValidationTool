using static EhwValidationTool.CpuzLaunchInfo;

namespace EhwValidationTool
{
    public class CpuzSaveInfo : ToolSaveInfo
    {
        public override ToolType ToolType => ToolType.CpuZ;

        public CpuzTabType TabType { get; set; }
        public int? SpdSlot { get; set; }
    }
}
