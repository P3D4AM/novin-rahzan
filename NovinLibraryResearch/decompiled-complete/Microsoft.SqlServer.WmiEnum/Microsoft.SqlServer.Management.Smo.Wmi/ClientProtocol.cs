using System.Management;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class ClientProtocol : WmiBase
{
	public ClientProtocol()
		: base("ClientNetworkProtocol")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "ProtocolName"
		});
		AddProperty(new WmiProperty("Order", "System.Int32")
		{
			PhysicalName = "ProtocolOrder"
		});
		AddProperty(new WmiProperty("DisplayName", "System.String")
		{
			PhysicalName = "ProtocolDisplayName",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("NetworkLibrary", "System.String")
		{
			PhysicalName = "ProtocolDLL",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("IsEnabled", "System.Boolean")
		{
			PhysicalName = "ProtocolOrder"
		});
	}

	protected override object GetTranslatedValue(ManagementObject mo, string propPhysName)
	{
		if ("IsEnabled" == propPhysName)
		{
			return (int)((ManagementBaseObject)mo)["ProtocolOrder"] > 0;
		}
		return base.GetTranslatedValue(mo, propPhysName);
	}
}
