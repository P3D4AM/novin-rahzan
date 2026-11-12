using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Data.SqlTypes;

internal class UnicodeString : SafeHandleZeroOrMinusOneIsInvalid
{
	public UnicodeString(string path)
		: base(ownsHandle: true)
	{
		Initialize(path);
	}

	protected override bool ReleaseHandle()
	{
		if (handle == IntPtr.Zero)
		{
			return true;
		}
		Marshal.FreeHGlobal(handle);
		handle = IntPtr.Zero;
		return true;
	}

	private void Initialize(string path)
	{
		UnsafeNativeMethods.UNICODE_STRING structure = default(UnsafeNativeMethods.UNICODE_STRING);
		structure.length = (ushort)(path.Length * 2);
		structure.maximumLength = (ushort)(path.Length * 2);
		structure.buffer = path;
		IntPtr zero = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			zero = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			if (zero != IntPtr.Zero)
			{
				SetHandle(zero);
			}
		}
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			DangerousAddRef(ref success);
			IntPtr ptr = DangerousGetHandle();
			Marshal.StructureToPtr(structure, ptr, fDeleteOld: false);
		}
		finally
		{
			if (success)
			{
				DangerousRelease();
			}
		}
	}
}
