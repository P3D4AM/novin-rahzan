using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Identity.Client.Platforms.net45.Native;

internal sealed class SafeBCryptAlgorithmHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	public SafeBCryptAlgorithmHandle()
		: base(ownsHandle: true)
	{
	}

	[DllImport("bcrypt.dll")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SuppressUnmanagedCodeSecurity]
	private static extern BCryptNative.ErrorCode BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, int flags);

	protected override bool ReleaseHandle()
	{
		return BCryptCloseAlgorithmProvider(handle, 0) == BCryptNative.ErrorCode.Success;
	}
}
