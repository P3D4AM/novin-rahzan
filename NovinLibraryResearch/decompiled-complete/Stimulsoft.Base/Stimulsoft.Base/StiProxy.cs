using System;
using System.IO;
using System.Net;

namespace Stimulsoft.Base;

public static class StiProxy
{
	public class Settings : StiObject
	{
		public string Address { get; set; }

		public int Port { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public Settings()
		{
		}

		public Settings(string address, int port, string userName, string password)
		{
			Address = address;
			Port = port;
			UserName = userName;
			Password = password;
		}
	}

	public static IWebProxy GetProxy()
	{
		try
		{
			Settings settings = LoadProxySettings();
			if (settings == null || string.IsNullOrWhiteSpace(settings.Address))
			{
				return WebRequest.GetSystemWebProxy();
			}
			WebProxy webProxy = new WebProxy(settings.Address, settings.Port);
			webProxy.Credentials = new NetworkCredential(settings.UserName, settings.Password);
			return webProxy;
		}
		catch
		{
			return WebRequest.GetSystemWebProxy();
		}
	}

	private static string GetProxySettingsPath()
	{
		string stimulsoftFolder = GetStimulsoftFolder();
		return Path.Combine(stimulsoftFolder, "Proxy.json");
	}

	private static string GetStimulsoftFolder()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft");
	}

	public static Settings LoadProxySettings()
	{
		string proxySettingsPath = GetProxySettingsPath();
		if (!File.Exists(proxySettingsPath))
		{
			return null;
		}
		Settings settings = new Settings();
		settings.LoadFromString(File.ReadAllText(proxySettingsPath));
		return settings;
	}

	public static void SaveProxySettings(Settings settings)
	{
		string proxySettingsPath = GetProxySettingsPath();
		string stimulsoftFolder = GetStimulsoftFolder();
		if (!Directory.Exists(stimulsoftFolder))
		{
			Directory.CreateDirectory(stimulsoftFolder);
		}
		File.WriteAllText(proxySettingsPath, settings.SaveToString());
	}
}
