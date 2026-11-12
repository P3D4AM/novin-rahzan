using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class InOutOfProcHelper
{
	private static readonly InOutOfProcHelper SingletonInstance = new InOutOfProcHelper();

	private bool _inProc;

	internal static bool InProc => SingletonInstance._inProc;

	private InOutOfProcHelper()
	{
		IntPtr moduleHandle = Microsoft.Data.Common.SafeNativeMethods.GetModuleHandle(null);
		if (IntPtr.Zero != moduleHandle)
		{
			if (IntPtr.Zero != Microsoft.Data.Common.SafeNativeMethods.GetProcAddress(moduleHandle, "_______SQL______Process______Available@0"))
			{
				_inProc = true;
			}
			else if (IntPtr.Zero != Microsoft.Data.Common.SafeNativeMethods.GetProcAddress(moduleHandle, "______SQL______Process______Available"))
			{
				_inProc = true;
			}
		}
	}
}
