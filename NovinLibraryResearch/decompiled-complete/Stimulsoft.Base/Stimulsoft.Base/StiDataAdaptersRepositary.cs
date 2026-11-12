using System;
using System.IO;

namespace Stimulsoft.Base;

public sealed class StiDataAdaptersRepositary
{
	public static string GetRootDataAdaptersFolder()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stimulsoft", "DataAdapters");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static string GetDataAdapterFolder(string assemblyName)
	{
		string rootDataAdaptersFolder = GetRootDataAdaptersFolder();
		string path = ((assemblyName.EndsWithInvariantIgnoreCase("exe") || assemblyName.EndsWithInvariantIgnoreCase("dll")) ? Path.GetFileNameWithoutExtension(assemblyName) : assemblyName);
		return Path.Combine(rootDataAdaptersFolder, path);
	}

	public static string GetDataAdapterVersionFolder(string assemblyName, string version)
	{
		string rootDataAdaptersFolder = GetRootDataAdaptersFolder();
		string text = ((assemblyName.EndsWithInvariantIgnoreCase("exe") || assemblyName.EndsWithInvariantIgnoreCase("dll")) ? Path.GetFileNameWithoutExtension(assemblyName) : assemblyName);
		return Path.Combine(rootDataAdaptersFolder, text + "-" + version);
	}

	public static string GetDataAdapterPathFromRoot(string assembly)
	{
		string text = Path.Combine(GetRootDataAdaptersFolder(), assembly);
		if (File.Exists(text))
		{
			return text;
		}
		return null;
	}

	public static string GetDataAdapterPath(string assembly)
	{
		foreach (string item in Directory.EnumerateDirectories(GetRootDataAdaptersFolder()))
		{
			string text = Path.Combine(item, assembly);
			if (File.Exists(text))
			{
				return text;
			}
		}
		return null;
	}
}
