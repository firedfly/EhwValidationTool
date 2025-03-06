using Newtonsoft.Json;
using System.Drawing;

namespace XtremeCapture
{
    [JsonConverter(typeof(ToolSaveInfoConverter))]
    public class ToolSaveInfo
    {
        public virtual ToolType ToolType {  get; set; }
        public Rectangle Location { get; set; }
        public int ZOrder { get; set; }
    }
}
