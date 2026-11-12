using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeLocalAllocHandle()
		: base(ownsHandle: true)
	{
	}

	[DllImport("kernel32.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr LocalFree(IntPtr hMem);

	internal unsafe T Read<T>(int offset) where T : struct
	{
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			return (T)Marshal.PtrToStructure(new IntPtr((byte*)handle.ToPointer() + offset), typeof(T));
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}

	protected override bool ReleaseHandle()
	{
		return LocalFree(handle) == IntPtr.Zero;
	}
}
