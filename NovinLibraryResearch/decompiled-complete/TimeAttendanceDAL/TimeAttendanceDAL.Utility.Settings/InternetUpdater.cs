using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PacketParser.Entities;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Properties;

namespace TimeAttendanceDAL.Utility.Settings;

public static class InternetUpdater
{
	public static void Start()
	{
		// [SANITIZED] به‌روزرسان اینترنتی غیرفعال شد: این متد فایل Internet_Update.exe را از
		// [SANITIZED] ریسورس استخراج و به‌صورت فرایند جداگانه اجرا می‌کرد (رفتار Phone-Home).
	}

	private static int FileVersionInt(string fileName)
	{
		try
		{
			string text = Path.Combine(Application.StartupPath, fileName);
			if (!File.Exists(text))
			{
				return 0;
			}
			string fileVersion = FileVersionInfo.GetVersionInfo(text).FileVersion;
			if (fileVersion == null)
			{
				return 0;
			}
			fileVersion = fileVersion.Split(new char[1] { '.' }).Aggregate("", (string current, string t) => current + t);
			return int.Parse(fileVersion);
		}
		catch
		{
			return 0;
		}
	}
}
