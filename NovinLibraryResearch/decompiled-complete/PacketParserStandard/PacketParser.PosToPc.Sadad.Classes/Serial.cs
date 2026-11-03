using System.Xml.Serialization;

namespace PacketParser.PosToPc.Sadad.Classes;

[XmlType(AnonymousType = true)]
public class Serial
{
	[XmlElement(ElementName = "BaudRate")]
	public int BaudRate { get; set; }

	[XmlElement(ElementName = "DataBits")]
	public int DataBits { get; set; }

	[XmlElement(ElementName = "DeviceType")]
	public int DeviceType { get; set; }

	[XmlElement(ElementName = "Handshake")]
	public string Handshake { get; set; }

	[XmlElement(ElementName = "Name")]
	public string Name { get; set; }

	[XmlElement(ElementName = "Parity")]
	public string Parity { get; set; }

	[XmlElement(ElementName = "StopBits")]
	public string StopBits { get; set; }

	[XmlElement(ElementName = "Timeout")]
	public int Timeout { get; set; }

	[XmlElement(ElementName = "Ttd")]
	public int TTD { get; set; }
}
