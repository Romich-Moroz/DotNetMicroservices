using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace MicroservicesTestTask.Data.Models
{
    [JsonDerivedType(typeof(CombinedOvenStatus), "CombinedOvenStatus")]
    [JsonDerivedType(typeof(CombinedPumpStatus), "CombinedPumpStatus")]
    [JsonDerivedType(typeof(CombinedSamplerStatus), "CombinedSamplerStatus")]
    public abstract class RapidControlStatus
    {
        [XmlElement("ModuleState")]
        public string ModuleState { get; set; } = string.Empty;

        [XmlElement("IsBusy")]
        public bool IsBusy { get; set; }

        [XmlElement("IsReady")]
        public bool IsReady { get; set; }

        [XmlElement("IsError")]
        public bool IsError { get; set; }

        [XmlElement("KeyLock")]
        public bool KeyLock { get; set; }
    }
}
