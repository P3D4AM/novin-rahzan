using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Identity.Client.Platforms.Features.Windows;

internal static class WindowsNativeMethods
{
	public enum NetJoinStatus
	{
		NetSetupUnknownStatus,
		NetSetupUnjoined,
		NetSetupWorkgroupName,
		NetSetupDomainName
	}

	private struct SYSTEM_INFO
	{
		public readonly short wProcessorArchitecture;

		public readonly short wReserved;

		public readonly int dwPageSize;

		public readonly IntPtr lpMinimumApplicationAddress;

		public readonly IntPtr lpMaximumApplicationAddress;

		public readonly IntPtr dwActiveProcessorMask;

		public readonly int dwNumberOfProcessors;

		public readonly int dwProcessorType;

		public readonly int dwAllocationGranularity;

		public readonly short wProcessorLevel;

		public readonly short wProcessorRevision;
	}

	private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;

	private const int PROCESSOR_ARCHITECTURE_ARM = 5;

	private const int PROCESSOR_ARCHITECTURE_IA64 = 6;

	private const int PROCESSOR_ARCHITECTURE_INTEL = 0;

	public const int ErrorSuccess = 0;

	[DllImport("kernel32.dll")]
	private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

	public static string GetProcessorArchitecture()
	{
		try
		{
			SYSTEM_INFO lpSystemInfo = default(SYSTEM_INFO);
			GetNativeSystemInfo(ref lpSystemInfo);
			switch (lpSystemInfo.wProcessorArchitecture)
			{
			case 6:
			case 9:
				return "x64";
			case 5:
				return "ARM";
			case 0:
				return "x86";
			default:
				return "Unknown";
			}
		}
		catch (Exception)
		{
			return "Unknown";
		}
	}

	[DllImport("secur32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.U1)]
	public static extern bool GetUserNameEx(int nameFormat, StringBuilder userName, ref uint userNameSize);

	[DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	public static extern int NetGetJoinInformation(string server, out IntPtr domain, out NetJoinStatus status);

	[DllImport("Netapi32.dll")]
	public static extern int NetApiBufferFree(IntPtr Buffer);

	[DllImport("user32.dll")]
	public static extern IntPtr GetForegroundWindow();
}
