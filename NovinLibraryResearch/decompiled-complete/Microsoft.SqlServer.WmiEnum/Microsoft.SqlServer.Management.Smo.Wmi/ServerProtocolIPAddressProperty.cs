namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServerProtocolIPAddressProperty : ProtocolPropertiesBase
{
	public ServerProtocolIPAddressProperty()
		: base("ServerNetworkProtocolProperty")
	{
	}

	protected override string[] GetParentFields()
	{
		return new string[3] { "Name", "ProtocolName", "InstanceName" };
	}

	protected override string GetWhereClauseTemplate()
	{
		return "(IPAddressName=\"{0}\" and ProtocolName=\"{1}\" and InstanceName=\"{2}\")";
	}
}
