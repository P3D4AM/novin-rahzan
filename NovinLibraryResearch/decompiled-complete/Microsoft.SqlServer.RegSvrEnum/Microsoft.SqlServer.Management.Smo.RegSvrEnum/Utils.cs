using System;
using System.IO;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

internal class Utils
{
	private const string yukonSavePathSuffixFormat = "Microsoft\\Microsoft SQL Server\\{0}\\Tools\\Shell";

	public static string GetYukonSettingsDirName(string version)
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"Microsoft\\Microsoft SQL Server\\{version}\\Tools\\Shell");
	}

	public static void EnsureSettingsDirExists(string version)
	{
		Directory.CreateDirectory(GetYukonSettingsDirName(version));
	}
}
