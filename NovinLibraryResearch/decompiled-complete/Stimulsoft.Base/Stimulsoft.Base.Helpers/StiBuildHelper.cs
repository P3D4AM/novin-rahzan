namespace Stimulsoft.Base.Helpers;

internal static class StiBuildHelper
{
	internal static bool DecodeVersion(string version, out int major, out int minor, out int build)
	{
		major = 0;
		minor = 0;
		build = 0;
		if (string.IsNullOrWhiteSpace(version))
		{
			return false;
		}
		string[] array = version.Split(new char[1] { '.' });
		if (array.Length < 3)
		{
			return false;
		}
		if (!int.TryParse(array[0], out major))
		{
			return false;
		}
		if (!int.TryParse(array[1], out minor))
		{
			return false;
		}
		if (!int.TryParse(array[2], out build))
		{
			return false;
		}
		return true;
	}
}
