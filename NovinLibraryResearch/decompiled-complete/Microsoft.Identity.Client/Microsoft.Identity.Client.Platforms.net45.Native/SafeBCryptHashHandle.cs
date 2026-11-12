using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeBCryptHashHandle : SafeHandleWithBuffer
{
	[DllImport("bcrypt.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SuppressUnmanagedCodeSecurity]
	private static extern BCryptNative.ErrorCode BCryptDestroyHash(IntPtr hHash);

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	protected override bool ReleaseNativeHandle()
	{
		return BCryptDestroyHash(handle) == BCryptNative.ErrorCode.Success;
	}
}
