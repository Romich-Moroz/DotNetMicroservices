using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MicroservicesTestTask.Data.Models
{
    public class DeviceStatus
    {
        [XmlElement("ModuleCategoryID")]
        public string ModuleCategoryId { get; set; } = string.Empty;

        [XmlElement("IndexWithinRole")]
        public int IndexWithinRole { get; set; }

        [XmlElement("RapidControlStatus")]
        [JsonIgnore]
        public string RapidControlStatusXml { get; set; } = string.Empty;

        [XmlElement("RapidControlStatusData")]
        public required RapidControlStatus RapidControlStatus { get; set; }
    }
}
