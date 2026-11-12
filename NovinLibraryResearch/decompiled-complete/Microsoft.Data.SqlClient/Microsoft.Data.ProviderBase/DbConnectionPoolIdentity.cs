using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Data.Common;

namespace Microsoft.Data.ProviderBase;

[Serializable]
internal sealed class DbConnectionPoolIdentity
{
	private const int E_NotImpersonationToken = -2147023587;

	private const int Win32_CheckTokenMembership = 1;

	private const int Win32_GetTokenInformation_1 = 2;

	private const int Win32_GetTokenInformation_2 = 3;

	private const int Win32_ConvertSidToStringSidW = 4;

	private const int Win32_CreateWellKnownSid = 5;

	public static readonly DbConnectionPoolIdentity NoIdentity = new DbConnectionPoolIdentity(string.Empty, isRestricted: false, isNetwork: true);

	private static readonly byte[] NetworkSid = (ADP.IsWindowsNT ? CreateWellKnownSid((WellKnownSidType)9) : null);

	private static DbConnectionPoolIdentity _lastIdentity = null;

	private readonly string _sidString;

	private readonly bool _isRestricted;

	private readonly bool _isNetwork;

	private readonly int _hashCode;

	internal bool IsRestricted => _isRestricted;

	internal bool IsNetwork => _isNetwork;

	private DbConnectionPoolIdentity(string sidString, bool isRestricted, bool isNetwork)
	{
		_sidString = sidString;
		_isRestricted = isRestricted;
		_isNetwork = isNetwork;
		_hashCode = sidString?.GetHashCode() ?? 0;
	}

	private static byte[] CreateWellKnownSid(WellKnownSidType sidType)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected I4, but got Unknown
		uint resultSidLength = (uint)SecurityIdentifier.MaxBinaryLength;
		byte[] array = new byte[resultSidLength];
		if (UnsafeNativeMethods.CreateWellKnownSid((int)sidType, null, array, ref resultSidLength) == 0)
		{
			IntegratedSecurityError(5);
		}
		return array;
	}

	public override bool Equals(object value)
	{
		bool flag = this == NoIdentity || this == value;
		if (!flag && value != null)
		{
			DbConnectionPoolIdentity dbConnectionPoolIdentity = (DbConnectionPoolIdentity)value;
			flag = _sidString == dbConnectionPoolIdentity._sidString && _isRestricted == dbConnectionPoolIdentity._isRestricted && _isNetwork == dbConnectionPoolIdentity._isNetwork;
		}
		return flag;
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
	internal static WindowsIdentity GetCurrentWindowsIdentity()
	{
		return WindowsIdentity.GetCurrent();
	}

	[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
	private static IntPtr GetWindowsIdentityToken(WindowsIdentity identity)
	{
		return identity.Token;
	}

	internal static DbConnectionPoolIdentity GetCurrent()
	{
		if (!ADP.IsWindowsNT)
		{
			return NoIdentity;
		}
		WindowsIdentity currentWindowsIdentity = GetCurrentWindowsIdentity();
		IntPtr windowsIdentityToken = GetWindowsIdentityToken(currentWindowsIdentity);
		uint num = 2048u;
		uint tokenString = 0u;
		IntPtr intPtr = IntPtr.Zero;
		IntPtr stringSid = IntPtr.Zero;
		bool flag = Win32NativeMethods.IsTokenRestrictedWrapper(windowsIdentityToken);
		DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (!UnsafeNativeMethods.CheckTokenMembership(windowsIdentityToken, NetworkSid, out var isMember))
			{
				IntegratedSecurityError(1);
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				intPtr = Microsoft.Data.Common.SafeNativeMethods.LocalAlloc(0, (IntPtr)num);
			}
			if (IntPtr.Zero == intPtr)
			{
				throw new OutOfMemoryException();
			}
			if (!UnsafeNativeMethods.GetTokenInformation(windowsIdentityToken, 1u, intPtr, num, ref tokenString))
			{
				if (tokenString > num)
				{
					num = tokenString;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						Microsoft.Data.Common.SafeNativeMethods.LocalFree(intPtr);
						intPtr = IntPtr.Zero;
						intPtr = Microsoft.Data.Common.SafeNativeMethods.LocalAlloc(0, (IntPtr)num);
					}
					if (IntPtr.Zero == intPtr)
					{
						throw new OutOfMemoryException();
					}
					if (!UnsafeNativeMethods.GetTokenInformation(windowsIdentityToken, 1u, intPtr, num, ref tokenString))
					{
						IntegratedSecurityError(2);
					}
				}
				else
				{
					IntegratedSecurityError(3);
				}
			}
			currentWindowsIdentity.Dispose();
			IntPtr sid = Marshal.ReadIntPtr(intPtr, 0);
			if (!UnsafeNativeMethods.ConvertSidToStringSidW(sid, out stringSid))
			{
				IntegratedSecurityError(4);
			}
			if (IntPtr.Zero == stringSid)
			{
				throw ADP.InternalError(ADP.InternalErrorCode.ConvertSidToStringSidWReturnedNull);
			}
			string text = Marshal.PtrToStringUni(stringSid);
			DbConnectionPoolIdentity lastIdentity = _lastIdentity;
			dbConnectionPoolIdentity = ((lastIdentity == null || !(lastIdentity._sidString == text) || lastIdentity._isRestricted != flag || lastIdentity._isNetwork != isMember) ? new DbConnectionPoolIdentity(text, flag, isMember) : lastIdentity);
		}
		finally
		{
			if (IntPtr.Zero != intPtr)
			{
				Microsoft.Data.Common.SafeNativeMethods.LocalFree(intPtr);
				intPtr = IntPtr.Zero;
			}
			if (IntPtr.Zero != stringSid)
			{
				Microsoft.Data.Common.SafeNativeMethods.LocalFree(stringSid);
				stringSid = IntPtr.Zero;
			}
		}
		_lastIdentity = dbConnectionPoolIdentity;
		return dbConnectionPoolIdentity;
	}

	public override int GetHashCode()
	{
		return _hashCode;
	}

	private static void IntegratedSecurityError(int caller)
	{
		int hRForLastWin32Error = Marshal.GetHRForLastWin32Error();
		if (1 != caller || -2147023587 != hRForLastWin32Error)
		{
			Marshal.ThrowExceptionForHR(hRForLastWin32Error);
		}
	}
}
