using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeNCryptBuffer : SafeHandleZeroOrMinusOneIsInvalid
{
	internal SafeNCryptBuffer()
		: base(ownsHandle: true)
	{
	}

	[SecurityCritical]
	internal unsafe T ReadArray<T>(uint index) where T : struct
	{
		checked
		{
			uint num = (uint)Marshal.SizeOf(typeof(T));
			if (unchecked(num % UIntPtr.Size) != 0L)
			{
				num += (uint)(UIntPtr.Size - unchecked(num % UIntPtr.Size));
			}
			return (T)Marshal.PtrToStructure(new IntPtr(new UIntPtr(new UIntPtr(handle.ToPointer()).ToUInt64() + num * index).ToPointer()), typeof(T));
		}
	}

	protected override bool ReleaseHandle()
	{
		return NCryptNative.UnsafeNativeMethods.NCryptFreeBuffer(handle) == NCryptNative.ErrorCode.Success;
	}
}
