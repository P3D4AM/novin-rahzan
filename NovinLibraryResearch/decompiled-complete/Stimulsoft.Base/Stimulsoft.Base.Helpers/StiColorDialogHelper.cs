using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Stimulsoft.Base.Helpers;

public static class StiColorDialogHelper
{
	private static readonly object sync = new object();

	private static string GetDefaultReportSettingsPath()
	{
		string text = "Stimulsoft\\ColorDialog_CustomColors.json";
		string text2 = null;
		if (StiBaseOptions.FullTrust)
		{
			text2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			if (text2.Length != 0)
			{
				text = Path.Combine(text2, text);
			}
		}
		text2 = Path.GetDirectoryName(text);
		if (StiBaseOptions.FullTrust && !Directory.Exists(text2))
		{
			Directory.CreateDirectory(text2);
		}
		return text;
	}

	public static int[] Load()
	{
		Monitor.Enter(sync);
		try
		{
			string defaultReportSettingsPath = GetDefaultReportSettingsPath();
			if (File.Exists(defaultReportSettingsPath))
			{
				List<int> list = new List<int>();
				string text = File.ReadAllText(defaultReportSettingsPath);
				if (!string.IsNullOrEmpty(text))
				{
					StiJsonHelper.LoadFromJsonString(text, list);
					if (list.Count == 0)
					{
						return null;
					}
					return list.ToArray();
				}
			}
		}
		catch
		{
		}
		finally
		{
			Monitor.Exit(sync);
		}
		return null;
	}

	public static void Save(int[] colors)
	{
		int num = 0;
		string defaultReportSettingsPath = GetDefaultReportSettingsPath();
		while (true)
		{
			bool flag = false;
			Monitor.Enter(sync);
			try
			{
				StiFileUtils.ProcessReadOnly(defaultReportSettingsPath);
				if (File.Exists(defaultReportSettingsPath))
				{
					File.Delete(defaultReportSettingsPath);
				}
				if (colors == null || colors.Length == 0)
				{
					break;
				}
				string directoryName = Path.GetDirectoryName(defaultReportSettingsPath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				string contents = StiJsonHelper.SaveToJsonString(new List<int>(colors));
				File.WriteAllText(defaultReportSettingsPath, contents);
			}
			catch
			{
				flag = true;
			}
			finally
			{
				Monitor.Exit(sync);
			}
			if (flag && num <= 4)
			{
				num++;
				continue;
			}
			break;
		}
	}
}
