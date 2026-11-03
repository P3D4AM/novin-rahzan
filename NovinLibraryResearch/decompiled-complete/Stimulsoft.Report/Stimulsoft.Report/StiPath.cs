using System;
using System.IO;

namespace Stimulsoft.Report;

public static class StiPath
{
	public static string GetPath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return string.Empty;
		}
		string empty = string.Empty;
		if (Path.GetPathRoot(path).Trim() == "")
		{
			empty = StiOptions.Configuration.ApplicationDirectory;
			string text = empty;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			if (!text.EndsWith(directorySeparatorChar.ToString() ?? "", StringComparison.InvariantCulture))
			{
				string text2 = empty;
				directorySeparatorChar = Path.AltDirectorySeparatorChar;
				if (!text2.EndsWith(directorySeparatorChar.ToString() ?? "", StringComparison.InvariantCulture))
				{
					string text3 = empty;
					directorySeparatorChar = Path.DirectorySeparatorChar;
					empty = text3 + directorySeparatorChar;
				}
			}
			empty += path;
		}
		else
		{
			empty = path;
		}
		if (empty.IndexOf(Path.DirectorySeparatorChar) >= 0 && path.IndexOf(Path.DirectorySeparatorChar) >= 0)
		{
			empty = empty.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}
		return empty;
	}
}
