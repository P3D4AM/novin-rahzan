using System.Xml;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public sealed class ServerInstanceRegistrationInfo : RegistrationInfo
{
	private UIConnectionInfo ci;

	public UIConnectionInfo ConnectionInfo
	{
		get
		{
			return ci;
		}
		set
		{
			ci = value;
		}
	}

	public ServerInstanceRegistrationInfo()
	{
		ConnectionInfo = new UIConnectionInfo();
	}

	public ServerInstanceRegistrationInfo(UIConnectionInfo ci)
	{
		ConnectionInfo = ci;
	}

	internal override void SaveNodeToXml(XmlWriter writer, bool saveName)
	{
		writer.WriteStartElement("Server", null);
		writer.WriteAttributeString("name", null, base.FriendlyName);
		writer.WriteAttributeString("description", null, base.Description);
		ConnectionInfo.SaveToStream(writer, saveName);
		if (ConnectionInfo.InMemoryPassword != null)
		{
			WindowsCredential.SetSqlRegSvrCredential(ConnectionInfo.ServerName, ConnectionInfo.AuthenticationType, ConnectionInfo.UserName, base.ServerType, ConnectionInfo.InMemoryPassword, RegistrationProvider.ProviderStoreCredentialVersion);
		}
		writer.WriteEndElement();
	}
}
