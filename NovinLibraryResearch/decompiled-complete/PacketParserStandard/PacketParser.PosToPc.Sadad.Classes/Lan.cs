using System.Xml.Serialization;

namespace PacketParser.PosToPc.Sadad.Classes;

[XmlType(AnonymousType = true)]
public class Lan
{
	[XmlElement(ElementName = "DeviceType")]
	public int DeviceType { get; set; }

	[XmlElement(ElementName = "IP")]
	public string IP { get; set; }

	[XmlElement(ElementName = "Port")]
	public int Port { get; set; }

	[XmlElement(ElementName = "Timeout")]
	public int Timeout { get; set; }
}
