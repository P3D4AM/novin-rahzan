using System.IO;

namespace Stimulsoft.Base;

public static class StiFileUtils
{
	public static bool ProcessReadOnly(string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		FileAttributes attributes = File.GetAttributes(path);
		if ((attributes & FileAttributes.ReadOnly) <= (FileAttributes)0)
		{
			return false;
		}
		File.SetAttributes(path, attributes - 1);
		return true;
	}

	public static bool IsReadOnly(string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		FileAttributes attributes = File.GetAttributes(path);
		return (attributes & FileAttributes.ReadOnly) > (FileAttributes)0;
	}
}
