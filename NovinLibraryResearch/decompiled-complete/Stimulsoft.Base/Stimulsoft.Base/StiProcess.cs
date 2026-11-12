using System.Diagnostics;

namespace Stimulsoft.Base;

public static class StiProcess
{
	public static void Start(string fileName)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo
		{
			FileName = fileName,
			UseShellExecute = true
		};
		Process.Start(startInfo);
	}
}
