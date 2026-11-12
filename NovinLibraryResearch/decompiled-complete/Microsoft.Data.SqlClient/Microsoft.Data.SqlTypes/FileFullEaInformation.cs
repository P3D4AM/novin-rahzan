using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Data.Common;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Data.SqlTypes;

internal class FileFullEaInformation : SafeHandleZeroOrMinusOneIsInvalid
{
	private string EA_NAME_STRING = "Filestream_Transaction_Tag";

	private int m_cbBuffer;

	public int Length => m_cbBuffer;

	public FileFullEaInformation(byte[] transactionContext)
		: base(ownsHandle: true)
	{
		m_cbBuffer = 0;
		InitializeEaBuffer(transactionContext);
	}

	protected override bool ReleaseHandle()
	{
		m_cbBuffer = 0;
		if (handle == IntPtr.Zero)
		{
			return true;
		}
		Marshal.FreeHGlobal(handle);
		handle = IntPtr.Zero;
		return true;
	}

	private void InitializeEaBuffer(byte[] transactionContext)
	{
		if (transactionContext.Length >= 65535)
		{
			throw ADP.ArgumentOutOfRange("transactionContext");
		}
		UnsafeNativeMethods.FILE_FULL_EA_INFORMATION structure = default(UnsafeNativeMethods.FILE_FULL_EA_INFORMATION);
		structure.nextEntryOffset = 0u;
		structure.flags = 0;
		structure.EaName = 0;
		structure.EaNameLength = (byte)EA_NAME_STRING.Length;
		structure.EaValueLength = (ushort)transactionContext.Length;
		m_cbBuffer = Marshal.SizeOf(structure) + structure.EaNameLength + structure.EaValueLength;
		IntPtr zero = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			zero = Marshal.AllocHGlobal(m_cbBuffer);
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
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			byte[] bytes = aSCIIEncoding.GetBytes(EA_NAME_STRING);
			int i = Marshal.OffsetOf(typeof(UnsafeNativeMethods.FILE_FULL_EA_INFORMATION), "EaName").ToInt32();
			int num = 0;
			for (; i < m_cbBuffer; i++)
			{
				if (num >= structure.EaNameLength)
				{
					break;
				}
				Marshal.WriteByte(ptr, i, bytes[num]);
				num++;
			}
			Marshal.WriteByte(ptr, i, 0);
			i++;
			int num2 = 0;
			for (; i < m_cbBuffer; i++)
			{
				if (num2 >= structure.EaValueLength)
				{
					break;
				}
				Marshal.WriteByte(ptr, i, transactionContext[num2]);
				num2++;
			}
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
