using System.Management;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

internal class Service : WmiBase
{
	public Service()
		: base("SqlService")
	{
		WmiProperty wmiProperty = null;
		AddProperty(new WmiProperty("Name", "System.String")
		{
			PhysicalName = "ServiceName"
		});
		AddProperty(new WmiProperty("Type", "Microsoft.SqlServer.Management.Smo.Wmi.ManagedServiceType")
		{
			BaseType = "System.Int32",
			PhysicalName = "SQLServiceType",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("StartMode", "Microsoft.SqlServer.Management.Smo.Wmi.ServiceStartMode")
		{
			BaseType = "System.Int32"
		});
		AddProperty(new WmiProperty("AcceptsPause", "System.Boolean")
		{
			PhysicalName = "AcceptPause",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("AcceptsStop", "System.Boolean")
		{
			PhysicalName = "AcceptStop",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("DisplayName", "System.String")
		{
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ErrorControl", "Microsoft.SqlServer.Management.Smo.Wmi.ServiceErrorControl")
		{
			BaseType = "System.Int32",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("PathName", "System.String")
		{
			PhysicalName = "BinaryPath",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("Description", "System.String")
		{
			ReadOnly = true
		});
		AddProperty(new WmiProperty("Dependencies", "System.String")
		{
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ServiceAccount", "System.String")
		{
			PhysicalName = "StartName",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ServiceState", "Microsoft.SqlServer.Management.Smo.Wmi.ServiceState")
		{
			PhysicalName = "State",
			BaseType = "System.Int32",
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ExitCode", "System.Int32")
		{
			ReadOnly = true
		});
		AddProperty(new WmiProperty("ProcessId", "System.Int32")
		{
			ReadOnly = true
		});
	}

	private int StringToEnum(string[] strList, ManagementObject mo, string propPhysName)
	{
		string str = (string)base.GetTranslatedValue(mo, propPhysName);
		return GetIndex(strList, str);
	}

	private int GetIndex(string[] strList, string str)
	{
		int num = 0;
		for (int i = 0; i < strList.Length && !(strList[i] == str); i++)
		{
			num++;
		}
		return num;
	}
}
