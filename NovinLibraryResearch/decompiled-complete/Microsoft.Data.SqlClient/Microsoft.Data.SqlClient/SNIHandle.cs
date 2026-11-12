using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class SNIHandle : SafeHandle
{
	private readonly uint _status = uint.MaxValue;

	private readonly bool _fSync;

	public override bool IsInvalid => IntPtr.Zero == handle;

	internal uint Status => _status;

	internal SNIHandle(SNINativeMethodWrapper.ConsumerInfo myInfo, string serverName, byte[] spnBuffer, bool ignoreSniOpenTimeout, int timeout, out byte[] instanceName, bool flushCache, bool fSync, bool fParallel, TransparentNetworkResolutionState transparentNetworkResolutionState, int totalTimeout, SqlConnectionIPAddressPreference ipPreference, SQLDNSInfo cachedDNSInfo)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			_fSync = fSync;
			instanceName = new byte[256];
			if (ignoreSniOpenTimeout)
			{
				timeout = -1;
			}
			_status = SNINativeMethodWrapper.SNIOpenSyncEx(myInfo, serverName, ref handle, spnBuffer, instanceName, flushCache, fSync, timeout, fParallel, (int)transparentNetworkResolutionState, totalTimeout, ADP.IsAzureSqlServerEndpoint(serverName), ipPreference, cachedDNSInfo);
		}
	}

	internal SNIHandle(SNINativeMethodWrapper.ConsumerInfo myInfo, SNIHandle parent, SqlConnectionIPAddressPreference ipPreference, SQLDNSInfo cachedDNSInfo)
		: base(IntPtr.Zero, ownsHandle: true)
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			_status = SNINativeMethodWrapper.SNIOpenMarsSession(myInfo, parent, ref handle, parent._fSync, ipPreference, cachedDNSInfo);
		}
	}

	protected override bool ReleaseHandle()
	{
		IntPtr intPtr = handle;
		handle = IntPtr.Zero;
		if (IntPtr.Zero != intPtr && SNINativeMethodWrapper.SNIClose(intPtr) != 0)
		{
			return false;
		}
		return true;
	}
}
