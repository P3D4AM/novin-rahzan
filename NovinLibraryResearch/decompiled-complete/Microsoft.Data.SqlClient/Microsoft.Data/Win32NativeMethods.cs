using System;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data;

internal static class Win32NativeMethods
{
	internal static bool IsTokenRestrictedWrapper(IntPtr token)
	{
		bool isRestricted;
		uint num = SNINativeMethodWrapper.UnmanagedIsTokenRestricted(token, out isRestricted);
		if (num != 0)
		{
			Marshal.ThrowExceptionForHR((int)num);
		}
		return isRestricted;
	}
}
