using System.Xml;

namespace Stimulsoft.Base.Serializing;

public interface IStiSerializable
{
	void Serialize(StiObjectStringConverter converter, XmlTextWriter tw);

	void Deserialize(StiObjectStringConverter converter, XmlTextReader tr);
}
