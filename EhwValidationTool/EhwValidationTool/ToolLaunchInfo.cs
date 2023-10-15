namespace EhwValidationTool
{
    public class ToolLaunchInfo
    {
        public ToolType ToolType { get; set; }
        public ToolLocation ToolLocation { get; set; }
        public int InstanceNumber { get; set; } = 1;
        public bool DisplayUserInfoAboveWindow { get; set; } = false;
        public int? SelectTabIndex { get; set; }
    }
}
