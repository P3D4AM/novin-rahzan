using System.Xml;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public class GroupRegistrationInfo : ParentRegistrationInfo
{
	internal override void SaveNodeToXml(XmlWriter writer, bool saveName)
	{
		writer.WriteStartElement("Group", null);
		writer.WriteAttributeString("name", null, base.FriendlyName);
		writer.WriteAttributeString("description", null, base.Description);
		foreach (RegistrationInfo child in base.Children)
		{
			child.SaveNodeToXml(writer, saveName);
		}
		writer.WriteEndElement();
	}
}
