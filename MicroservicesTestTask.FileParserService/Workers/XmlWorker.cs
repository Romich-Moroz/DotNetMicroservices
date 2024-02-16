using System.Text;
using System.Xml;

using MicroservicesTestTask.Data.Models;
using MicroservicesTestTask.Serialization;

namespace MicroservicesTestTask.FileParserService.Workers
{
    internal class XmlWorker(ISerializer xmlSerializer)
    {
        private readonly Dictionary<string, Type> _supportedDerivedTypes = new()
        {
            ["CombinedOvenStatus"] = typeof(CombinedOvenStatus),
            ["CombinedPumpStatus"] = typeof(CombinedPumpStatus),
            ["CombinedSamplerStatus"] = typeof(CombinedSamplerStatus)
        };

        public InstrumentStatus ParseXml(string file)
        {
            using var reader = new StreamReader(file);
            InstrumentStatus? instrumentalStatus = xmlSerializer.Deserialize<InstrumentStatus>(reader.BaseStream);

            if (instrumentalStatus != null)
            {
                foreach (DeviceStatus status in instrumentalStatus.DeviceStatuses)
                {
                    status.RapidControlStatus = (RapidControlStatus)(xmlSerializer.Deserialize(
                        GetDerivedTypeFromDeviceStatusXml(status),
                        status.RapidControlStatusXml.Trim(),
                        Encoding.Unicode
                    ) ?? new CombinedOvenStatus());
                }

                return instrumentalStatus;
            }

            return new InstrumentStatus();
        }

        private Type GetDerivedTypeFromDeviceStatusXml(DeviceStatus deviceStatus)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(deviceStatus.RapidControlStatusXml.Trim());

            foreach (XmlNode node in xmlDoc.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    return _supportedDerivedTypes[node.Name];
                }
            }

            return typeof(RapidControlStatus);
        }
    }
}
