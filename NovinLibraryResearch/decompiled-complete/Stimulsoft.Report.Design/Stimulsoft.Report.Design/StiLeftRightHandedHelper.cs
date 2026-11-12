using Microsoft.Win32;

namespace Stimulsoft.Report.Design;

public static class StiLeftRightHandedHelper
{
	public static bool IsLeftHanded => !IsRightHanded;

	public static bool IsRightHanded
	{
		get
		{
			RegistryKey val = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows");
			if (val == null)
			{
				return false;
			}
			object value = val.GetValue("MenuDropAlignment");
			if (value != null)
			{
				return value.ToString() == "1";
			}
			return false;
		}
	}
}
