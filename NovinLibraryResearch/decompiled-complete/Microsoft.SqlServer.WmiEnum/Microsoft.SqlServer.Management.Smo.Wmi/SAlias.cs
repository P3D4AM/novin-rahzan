namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class SAlias : WmiBase
{
	public SAlias()
		: base("SqlServerAlias")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "AliasName"
		});
		wmiProperty = new WmiProperty("ServerName", "System.String");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("ProtocolName", "System.String");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("ConnectionString", "System.String");
		AddProperty(wmiProperty);
	}
}
