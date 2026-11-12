using System;
using System.IO;

namespace Stimulsoft.Base.Licenses;

public static class StiLicensePath
{
	public static string GetStimulsoftLicenseKeyPath()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return Path.Combine(text, "license.key");
	}

	public static string GetStimulsoftLicenseKeyAppPath()
	{
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		if (!Directory.Exists(baseDirectory))
		{
			Directory.CreateDirectory(baseDirectory);
		}
		return Path.Combine(baseDirectory, "license.key");
	}
}
