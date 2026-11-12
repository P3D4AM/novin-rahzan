using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Helpers;

public static class StiWindowsSettings
{
	public class StiWindowState
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		[DefaultValue(0)]
		public int WindowState { get; set; }
	}

	private class StiContainerInfo
	{
		public Dictionary<string, StiWindowState> Data = new Dictionary<string, StiWindowState>();
	}

	private static Dictionary<string, StiWindowState> Data;

	private static bool isChanged;

	public static StiWindowState Get(string key)
	{
		if (Data == null)
		{
			Load();
		}
		Data.TryGetValue(key, out var value);
		return value;
	}

	public static void Set(string key, StiWindowState state)
	{
		if (Data == null)
		{
			Load();
		}
		if (Data.ContainsKey(key))
		{
			Data.Remove(key);
		}
		Data.Add(key, state);
		isChanged = true;
	}

	public static string GetSettingsPath()
	{
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		string text = "Stimulsoft" + directorySeparatorChar + "WindowsSettings.json";
		try
		{
			string folderPath;
			if (StiBaseOptions.FullTrust)
			{
				folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				if (folderPath.Length != 0)
				{
					text = Path.Combine(folderPath, text);
				}
			}
			folderPath = Path.GetDirectoryName(text);
			if (StiBaseOptions.FullTrust && !string.IsNullOrWhiteSpace(folderPath) && !Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
		}
		catch
		{
			try
			{
				directorySeparatorChar = Path.DirectorySeparatorChar;
				text = "Stimulsoft" + directorySeparatorChar + "WindowsSettings.json";
				string folderPath = "Stimulsoft";
				if (StiBaseOptions.FullTrust && !Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
			}
			catch
			{
				text = "WindowsSettings.json";
			}
		}
		return text;
	}

	public static void Load()
	{
		try
		{
			string settingsPath = GetSettingsPath();
			if (File.Exists(settingsPath))
			{
				string value = File.ReadAllText(settingsPath);
				StiContainerInfo stiContainerInfo = JsonConvert.DeserializeObject<StiContainerInfo>(value);
				Data = stiContainerInfo.Data;
			}
		}
		catch
		{
		}
		if (Data == null)
		{
			Data = new Dictionary<string, StiWindowState>();
		}
	}

	public static void Save()
	{
		string settingsPath = GetSettingsPath();
		if (!isChanged && File.Exists(settingsPath))
		{
			return;
		}
		try
		{
			StiContainerInfo value = new StiContainerInfo
			{
				Data = Data
			};
			string directoryName = Path.GetDirectoryName(settingsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
			StiFileUtils.ProcessReadOnly(settingsPath);
			File.WriteAllText(settingsPath, contents);
		}
		catch
		{
		}
	}
}
