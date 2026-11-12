using Microsoft.Win32;

namespace Stimulsoft.Controls.Win;

public static class StiOSVersion
{
	private static bool isWindows11;

	static StiOSVersion()
	{
		try
		{
			RegistryKey val = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion");
			string s = val.GetValue("CurrentBuild").ToString();
			if (int.TryParse(s, out var result) && result >= 22000)
			{
				isWindows11 = true;
			}
		}
		catch
		{
		}
	}

	public static bool IsWindows11()
	{
		return isWindows11;
	}
}
