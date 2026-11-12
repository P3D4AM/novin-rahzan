using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeBCryptKeyHandle : SafeHandleWithBuffer
{
	[DllImport("bcrypt.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SuppressUnmanagedCodeSecurity]
	private static extern BCryptNative.ErrorCode BCryptDestroyKey(IntPtr hKey);

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected override bool ReleaseNativeHandle()
	{
		return BCryptDestroyKey(handle) == BCryptNative.ErrorCode.Success;
	}
}
