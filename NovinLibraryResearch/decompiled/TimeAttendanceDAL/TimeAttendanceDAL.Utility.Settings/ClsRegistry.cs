using Microsoft.Win32;

namespace TimeAttendanceDAL.Utility.Settings;

public class ClsRegistry
{
	public static string Read(string key)
	{
		return Registry.GetValue("HKEY_CURRENT_USER\\Software\\Novin", key, "")?.ToString() ?? "";
	}

	public static int ReadInt(string key)
	{
		int.TryParse(Read(key), out var result);
		return result;
	}

	public static bool Write(string key, object value)
	{
		Registry.SetValue("HKEY_CURRENT_USER\\Software\\Novin", key, value);
		return true;
	}
}
