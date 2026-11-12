namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ServerProtocolProperties : ProtocolPropertiesBase
{
	public ServerProtocolProperties()
		: base("ServerNetworkProtocolProperty")
	{
	}

	protected override string[] GetParentFields()
	{
		return new string[2] { "Name", "InstanceName" };
	}

	protected override string GetWhereClauseTemplate()
	{
		return "(IPAddressName=\"\" and ProtocolName=\"{0}\" and InstanceName=\"{1}\")";
	}
}
