using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal abstract class SafeHandleWithBuffer : SafeHandleZeroOrMinusOneIsInvalid
{
	private IntPtr m_dataBuffer;

	public override bool IsInvalid
	{
		get
		{
			if (handle == IntPtr.Zero)
			{
				return m_dataBuffer == IntPtr.Zero;
			}
			return false;
		}
	}

	internal IntPtr DataBuffer
	{
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		get
		{
			return m_dataBuffer;
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		set
		{
			m_dataBuffer = value;
		}
	}

	protected SafeHandleWithBuffer()
		: base(ownsHandle: true)
	{
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected virtual bool ReleaseBuffer()
	{
		Marshal.FreeCoTaskMem(m_dataBuffer);
		return true;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected sealed override bool ReleaseHandle()
	{
		bool flag = false;
		if (handle != IntPtr.Zero)
		{
			flag = ReleaseNativeHandle();
		}
		if (m_dataBuffer != IntPtr.Zero)
		{
			flag &= ReleaseBuffer();
		}
		return flag;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected abstract bool ReleaseNativeHandle();
}
