using System.Xml.Serialization;

namespace MicroservicesTestTask.Data.Models
{
    public class CombinedSamplerStatus : RapidControlStatus
    {
        [XmlElement("Status")]
        public int Status { get; set; }

        [XmlElement("Vial")]
        public string Vial { get; set; } = string.Empty;

        [XmlElement("Volume")]
        public int Volume { get; set; }

        [XmlElement("MaximumInjectionVolume")]
        public int MaximumInjectionVolume { get; set; }

        [XmlElement("RackL")]
        public string RackL { get; set; } = string.Empty;

        [XmlElement("RackR")]
        public string RackR { get; set; } = string.Empty;

        [XmlElement("RackInf")]
        public int RackInf { get; set; }

        [XmlElement("Buzzer")]
        public bool Buzzer { get; set; }
    }
}
