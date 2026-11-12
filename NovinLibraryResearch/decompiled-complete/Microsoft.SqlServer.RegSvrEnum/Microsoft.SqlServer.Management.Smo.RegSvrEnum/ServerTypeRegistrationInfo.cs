using System.Xml;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public class ServerTypeRegistrationInfo : ParentRegistrationInfo
{
	internal override void SaveNodeToXml(XmlWriter writer, bool saveName)
	{
		writer.WriteStartElement("ServerType", null);
		writer.WriteAttributeString("id", null, base.ServerType.ToString());
		writer.WriteAttributeString("name", null, base.FriendlyName);
		foreach (RegistrationInfo child in base.Children)
		{
			child.SaveNodeToXml(writer, saveName);
		}
		writer.WriteEndElement();
	}
}
