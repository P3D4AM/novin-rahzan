using System.IO;

namespace Stimulsoft.Base;

public static class StiFileUrlHelper
{
	public static byte[] Get(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		byte[] array = null;
		try
		{
			if (File.Exists(path))
			{
				array = File.ReadAllBytes(path);
			}
		}
		catch
		{
		}
		try
		{
			if (array == null)
			{
				array = StiDownloadCache.Get(path);
			}
		}
		catch
		{
		}
		return array;
	}
}
