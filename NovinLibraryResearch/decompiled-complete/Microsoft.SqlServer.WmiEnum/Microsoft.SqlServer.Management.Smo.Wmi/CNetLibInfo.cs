namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class CNetLibInfo : WmiBase
{
	public CNetLibInfo()
		: base("ClientNetLibInfo")
	{
		WmiProperty wmiProperty = null;
		wmiProperty = new WmiProperty("FileName", "System.String");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("Version", "System.String");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("Date", "System.UInt32");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("Size", "System.UInt32");
		AddProperty(wmiProperty);
		wmiProperty = new WmiProperty("ProtocolName", "System.String");
		AddProperty(wmiProperty);
	}
}
