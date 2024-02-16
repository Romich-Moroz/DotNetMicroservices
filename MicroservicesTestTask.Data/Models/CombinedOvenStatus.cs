using System.Xml.Serialization;

namespace MicroservicesTestTask.Data.Models
{
    public class CombinedOvenStatus : RapidControlStatus
    {
        [XmlElement("UseTemperatureControl")]
        public bool UseTemperatureControl { get; set; }

        [XmlElement("OvenOn")]
        public bool OvenOn { get; set; }

        [XmlElement("Temperature_Actual")]
        public double TemperatureActual { get; set; }

        [XmlElement("Temperature_Room")]
        public double TemperatureRoom { get; set; }

        [XmlElement("MaximumTemperatureLimit")]
        public int MaximumTemperatureLimit { get; set; }

        [XmlElement("Valve_Position")]
        public int ValvePosition { get; set; }

        [XmlElement("Valve_Rotations")]
        public int ValveRotations { get; set; }

        [XmlElement("Buzzer")]
        public bool Buzzer { get; set; }
    }
}
