using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.HadrData;
using Microsoft.SqlServer.Management.Smo;

namespace Microsoft.SqlServer.Management.HadrModel;

public static class HadrModelUtilities
{
	public static string diskpartLogFileSuffix = "_diskpart.log";

	public static string wizardAlwaysonStr = "AlwaysOn";

	public static string SanitizeFileName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		string text = name;
		string text2 = "\\/:*?\"<>|";
		foreach (char oldChar in text2)
		{
			text = text.Replace(oldChar, '_');
		}
		return text;
	}

	public static Microsoft.SqlServer.Management.Smo.Server GetNewSmoServerObject(ServerConnection primaryConnection)
	{
		primaryConnection.Connect();
		primaryConnection.Disconnect();
		return new Microsoft.SqlServer.Management.Smo.Server(primaryConnection);
	}

	public static string GetDatabaseBackupFileFullName(this AvailabilityGroupData agData, Microsoft.SqlServer.Management.Smo.Server server, string databaseName)
	{
		return agData.GetBackupFileFullName(server, databaseName, ".bak");
	}

	public static string GetLogBackupFileFullName(this AvailabilityGroupData agData, Microsoft.SqlServer.Management.Smo.Server server, string databaseName)
	{
		return agData.GetBackupFileFullName(server, databaseName, ".trn");
	}

	private static string GetBackupFileFullName(this AvailabilityGroupData agData, Microsoft.SqlServer.Management.Smo.Server server, string databaseName, string fileExtension)
	{
		return PathWrapper.Combine(agData.GetBackupPathForServer(server), Path.ChangeExtension(SanitizeFileName(databaseName), fileExtension));
	}
}
