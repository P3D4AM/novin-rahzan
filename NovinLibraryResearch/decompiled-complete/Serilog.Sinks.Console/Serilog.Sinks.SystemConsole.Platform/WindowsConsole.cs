using System;
using System.Runtime.InteropServices;

namespace Serilog.Sinks.SystemConsole.Platform;

internal static class WindowsConsole
{
	private const int StandardOutputHandleId = -11;

	private const uint EnableVirtualTerminalProcessingMode = 4u;

	private const long InvalidHandleValue = -1L;

	public static void EnableVirtualTerminalProcessing()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			IntPtr stdHandle = GetStdHandle(-11);
			if (stdHandle != (IntPtr)(-1L) && GetConsoleMode(stdHandle, out var mode))
			{
				SetConsoleMode(stdHandle, mode | 4u);
			}
		}
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr GetStdHandle(int handleId);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool GetConsoleMode(IntPtr handle, out uint mode);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetConsoleMode(IntPtr handle, uint mode);
}
