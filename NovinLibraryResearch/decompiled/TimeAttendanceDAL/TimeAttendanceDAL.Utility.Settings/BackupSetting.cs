using System;
using System.IO;

namespace TimeAttendanceDAL.Utility.Settings;

public class BackupSetting
{
	public static string DefaultPath
	{
		get
		{
			if (Directory.Exists(ClsRegistry.Read("B120U")))
			{
				return ClsRegistry.Read("B120U");
			}
			if (!Directory.Exists("c:\\AutoBack"))
			{
				Directory.CreateDirectory("c:\\AutoBack");
			}
			return "c:\\AutoBack";
		}
		set
		{
			if (Directory.Exists(value))
			{
				ClsRegistry.Write("B120U", value.Trim());
				return;
			}
			try
			{
				Directory.CreateDirectory(value);
				ClsRegistry.Write("B120U", value.Trim());
			}
			catch (Exception)
			{
				Directory.CreateDirectory(DefaultPath);
				ClsRegistry.Write("B120U", DefaultPath);
			}
		}
	}

	public static bool EnableAutoBackupAllPartions
	{
		get
		{
			bool result = true;
			try
			{
				string value = ClsRegistry.Read("B128XU");
				if (!string.IsNullOrEmpty(value))
				{
					bool.TryParse(value, out result);
				}
			}
			catch (Exception)
			{
				result = true;
			}
			return result;
		}
		set
		{
			ClsRegistry.Write("B128XU", value.ToString());
		}
	}
}
