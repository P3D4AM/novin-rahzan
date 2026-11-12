using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal static class Win32Native
{
	[Flags]
	internal enum FormatMessageFlags
	{
		None = 0,
		AllocateBuffer = 0x100,
		FromModule = 0x800,
		FromSystem = 0x1000
	}

	internal struct SYSTEMTIME
	{
		internal ushort wYear;

		internal ushort wMonth;

		internal ushort wDayOfWeek;

		internal ushort wDay;

		internal ushort wHour;

		internal ushort wMinute;

		internal ushort wSecond;

		internal ushort wMilliseconds;

		internal SYSTEMTIME(DateTime time)
		{
			wYear = (ushort)time.Year;
			wMonth = (ushort)time.Month;
			wDayOfWeek = (ushort)time.DayOfWeek;
			wDay = (ushort)time.Day;
			wHour = (ushort)time.Hour;
			wMinute = (ushort)time.Minute;
			wSecond = (ushort)time.Second;
			wMilliseconds = (ushort)time.Millisecond;
		}
	}

	[SuppressUnmanagedCodeSecurity]
	private static class UnsafeNativeMethods
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern SafeLibraryHandle LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static extern IntPtr LocalFree(IntPtr hMem);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int FormatMessage(FormatMessageFlags dwFlags, SafeLibraryHandle lpSource, int dwMessageId, int dwLanguageId, [In][Out] ref IntPtr lpBuffer, int nSize, IntPtr pArguments);
	}

	[SecurityCritical]
	internal static string FormatMessageFromLibrary(int message, string library)
	{
		using SafeLibraryHandle lpSource = UnsafeNativeMethods.LoadLibrary(library);
		IntPtr lpBuffer = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (UnsafeNativeMethods.FormatMessage(FormatMessageFlags.AllocateBuffer | FormatMessageFlags.FromModule | FormatMessageFlags.FromSystem, lpSource, message, 0, ref lpBuffer, 0, IntPtr.Zero) == 0)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
			}
			return Marshal.PtrToStringUni(lpBuffer);
		}
		finally
		{
			if (lpBuffer != IntPtr.Zero)
			{
				UnsafeNativeMethods.LocalFree(lpBuffer);
			}
		}
	}

	internal static string GetNTStatusMessage(int ntstatus)
	{
		return FormatMessageFromLibrary(ntstatus, "ntdll.dll");
	}
}
