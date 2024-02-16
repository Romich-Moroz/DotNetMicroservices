using System.Xml.Serialization;

namespace MicroservicesTestTask.Data.Models
{
    public class InstrumentStatus
    {
        [XmlElement("PackageID")]
        public string PackageId { get; set; } = string.Empty;

        [XmlElement("DeviceStatus")]
        public DeviceStatus[] DeviceStatuses { get; set; } = [];
    }
}
