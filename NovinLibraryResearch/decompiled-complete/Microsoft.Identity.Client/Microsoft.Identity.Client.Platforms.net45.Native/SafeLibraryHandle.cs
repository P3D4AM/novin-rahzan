using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeLibraryHandle()
		: base(ownsHandle: true)
	{
	}

	[DllImport("kernel32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SuppressUnmanagedCodeSecurity]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool FreeLibrary(IntPtr hModule);

	protected override bool ReleaseHandle()
	{
		return FreeLibrary(handle);
	}
}
