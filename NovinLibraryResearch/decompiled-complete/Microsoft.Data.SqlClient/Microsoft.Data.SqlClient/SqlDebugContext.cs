using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlDebugContext : IDisposable
{
	internal uint pid;

	internal uint tid;

	internal bool active;

	internal IntPtr pMemMap = ADP.PtrZero;

	internal IntPtr hMemMap = ADP.PtrZero;

	internal uint dbgpid;

	internal bool fOption;

	internal string machineName;

	internal string sdiDllName;

	internal byte[] data;

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (pMemMap != IntPtr.Zero)
		{
			NativeMethods.UnmapViewOfFile(pMemMap);
			pMemMap = IntPtr.Zero;
		}
		if (hMemMap != IntPtr.Zero)
		{
			NativeMethods.CloseHandle(hMemMap);
			hMemMap = IntPtr.Zero;
		}
		active = false;
	}

	~SqlDebugContext()
	{
		Dispose(disposing: false);
	}
}
