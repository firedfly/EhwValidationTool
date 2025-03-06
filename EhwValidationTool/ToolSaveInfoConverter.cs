using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;

namespace EhwValidationTool
{
    public class ToolSaveInfoConverter : JsonConverter
    {
        private ToolSaveInfo Create(Type objectType, JObject jObject)
        {
            ToolType toolType = default;
            var toolTypeName = jObject.Value<string>("ToolType");
            if (!String.IsNullOrWhiteSpace(toolTypeName) && Enum.IsDefined(typeof(ToolType), toolTypeName))
            {
                // 4/25/16 sag: a new property was added to disambiguate what type of column we need to create.  the property
                // exists and has a value that is defined by the LiveQueryColumnType enum.  lets set the type of column to create
                toolType = (ToolType)Enum.Parse(typeof(ToolType), toolTypeName);
            }

            switch (toolType)
            {
                case ToolType.CpuZ:
                    return new CpuzSaveInfo();
                default:
                    return new ToolSaveInfo();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(objectType, jObject);

            // Populate the object properties 
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ToolSaveInfo).IsAssignableFrom(objectType);
        }
    }
}