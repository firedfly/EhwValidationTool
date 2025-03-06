using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EhwValidationTool
{
    [JsonConverter(typeof(ToolSaveInfoConverter))]
    public class ToolSaveInfo
    {
        public virtual ToolType ToolType {  get; set; }
        public Rectangle Location { get; set; }
        public int ZOrder { get; set; }
    }
}
