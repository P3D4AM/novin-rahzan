namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ClientProtocolProperties : ProtocolPropertiesBase
{
	public ClientProtocolProperties()
		: base("ClientNetworkProtocolProperty")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Idx", "System.UInt32")
		{
			PhysicalName = "PropertyIdx"
		});
	}

	protected override string[] GetParentFields()
	{
		return new string[1] { "Name" };
	}

	protected override string GetWhereClauseTemplate()
	{
		return "(ProtocolName=\"{0}\")";
	}
}
