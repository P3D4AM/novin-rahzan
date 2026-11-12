using System.IO;
using System.Runtime.InteropServices;

namespace Serilog.Sinks.File;

internal static class IOErrors
{
	public static bool IsLockedFile(IOException ex)
	{
		int num = Marshal.GetHRForException(ex) & 0xFFFF;
		if (num != 32)
		{
			return num == 33;
		}
		return true;
	}
}
