namespace Microsoft.SqlServer.Management.HadrModel;

internal class Utils
{
	public static bool IsSql13OrLater(int versionMajor)
	{
		return versionMajor >= 13;
	}

	public static bool IsSql14OrLater(int versionMajor)
	{
		return versionMajor >= 14;
	}
}
