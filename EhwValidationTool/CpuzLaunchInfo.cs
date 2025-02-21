namespace EhwValidationTool
{
    public class CpuzLaunchInfo : ToolLaunchInfo
    {
        public override ToolType ToolType => ToolType.CpuZ;
        public CpuzTabType TabType
        {
            get { return _tabType; }
            set
            {
                _tabType = value;
                SelectTabIndex = (int)value;
            }
        }
        private CpuzTabType _tabType;

        // 1 based index based on the Slot in the CPU-Z UI
        public int? SpdSlot { get; set; }


        public enum CpuzTabType
        {
            CPU = 0,
            Mainboard = 1,
            Memory = 2,
            SPD = 3,
            Graphics = 4,
            Bench = 5,
            About = 6
        }
    }
}
