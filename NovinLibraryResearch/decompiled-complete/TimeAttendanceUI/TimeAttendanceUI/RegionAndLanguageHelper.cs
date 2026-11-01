using System.Runtime.InteropServices;
using System.Text;

namespace TimeAttendanceUI;

public static class RegionAndLanguageHelper
{
	private enum GeoClass
	{
		Nation = 16,
		Region = 14
	}

	private const int GEO_FRIENDLYNAME = 8;

	[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
	private static extern int GetUserGeoID(GeoClass geoClass);

	[DllImport("kernel32.dll")]
	private static extern int GetUserDefaultLCID();

	[DllImport("kernel32.dll")]
	private static extern int GetGeoInfo(int geoid, int geoType, StringBuilder lpGeoData, int cchData, int langid);

	public static string GetMachineCurrentLocation(int geoFriendlyname)
	{
		int userGeoID = GetUserGeoID(GeoClass.Nation);
		int userDefaultLCID = GetUserDefaultLCID();
		StringBuilder stringBuilder = new StringBuilder(100);
		GetGeoInfo(userGeoID, geoFriendlyname, stringBuilder, stringBuilder.Capacity, userDefaultLCID);
		return stringBuilder.ToString().Trim();
	}
}
