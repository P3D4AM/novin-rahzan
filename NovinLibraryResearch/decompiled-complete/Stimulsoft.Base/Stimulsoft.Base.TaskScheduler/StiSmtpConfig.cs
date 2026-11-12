using System;
using System.IO;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.TaskScheduler;

public sealed class StiSmtpConfig
{
	private static StiSmtpConfig defaultConfig;

	public int Timeout = 100000;

	public string SenderEmail { get; set; }

	public string Host { get; set; }

	public int Port { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public bool EnableSsl { get; set; }

	private StiSmtpConfig()
	{
	}

	public static StiSmtpConfig Get()
	{
		if (defaultConfig == null)
		{
			defaultConfig = TryLoad();
		}
		return defaultConfig;
	}

	internal static void Clear()
	{
		defaultConfig = null;
	}

	public static StiSmtpConfig GetEmpty()
	{
		defaultConfig = null;
		return new StiSmtpConfig();
	}

	internal static string GetSettingsPath()
	{
		string text = $"Stimulsoft{Path.DirectorySeparatorChar}Scheduler{Path.DirectorySeparatorChar}Settings{Path.DirectorySeparatorChar}SmtpConfig.json";
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
				text = $"Stimulsoft{Path.DirectorySeparatorChar}Scheduler{Path.DirectorySeparatorChar}Settings{Path.DirectorySeparatorChar}SmtpConfig.json";
				string folderPath = $"Stimulsoft{Path.DirectorySeparatorChar}Scheduler{Path.DirectorySeparatorChar}Settings";
				if (StiBaseOptions.FullTrust && !Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
			}
			catch
			{
				text = "SmtpConfig.json";
			}
		}
		return text;
	}

	public void TrySave()
	{
		try
		{
			string settingsPath = GetSettingsPath();
			string directoryName = Path.GetDirectoryName(settingsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string contents = JsonConvert.SerializeObject(this, Formatting.Indented);
			StiFileUtils.ProcessReadOnly(settingsPath);
			File.WriteAllText(settingsPath, contents);
		}
		catch
		{
		}
	}

	private static StiSmtpConfig TryLoad()
	{
		try
		{
			string settingsPath = GetSettingsPath();
			if (File.Exists(settingsPath))
			{
				string value = File.ReadAllText(settingsPath);
				return JsonConvert.DeserializeObject<StiSmtpConfig>(value);
			}
		}
		catch
		{
		}
		return new StiSmtpConfig();
	}

	public bool IsValid()
	{
		if (string.IsNullOrWhiteSpace(SenderEmail))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(Host))
		{
			return false;
		}
		if (Port == 0)
		{
			return false;
		}
		return true;
	}
}
