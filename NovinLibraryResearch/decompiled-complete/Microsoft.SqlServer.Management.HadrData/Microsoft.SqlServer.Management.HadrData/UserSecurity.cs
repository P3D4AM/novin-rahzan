using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.SqlServer.Management.HadrData;

internal class UserSecurity
{
	public enum WellKnownSidType
	{
		WinNullSid,
		WinWorldSid,
		WinLocalSid,
		WinCreatorOwnerSid,
		WinCreatorGroupSid,
		WinCreatorOwnerServerSid,
		WinCreatorGroupServerSid,
		WinNtAuthoritySid,
		WinDialupSid,
		WinNetworkSid,
		WinBatchSid,
		WinInteractiveSid,
		WinServiceSid,
		WinAnonymousSid,
		WinProxySid,
		WinEnterpriseControllersSid,
		WinSelfSid,
		WinAuthenticatedUserSid,
		WinRestrictedCodeSid,
		WinTerminalServerSid,
		WinRemoteLogonIdSid,
		WinLogonIdsSid,
		WinLocalSystemSid,
		WinLocalServiceSid,
		WinNetworkServiceSid,
		WinBuiltinDomainSid,
		WinBuiltinAdministratorsSid,
		WinBuiltinUsersSid,
		WinBuiltinGuestsSid,
		WinBuiltinPowerUsersSid,
		WinBuiltinAccountOperatorsSid,
		WinBuiltinSystemOperatorsSid,
		WinBuiltinPrintOperatorsSid,
		WinBuiltinBackupOperatorsSid,
		WinBuiltinReplicatorSid,
		WinBuiltinPreWindows2000CompatibleAccessSid,
		WinBuiltinRemoteDesktopUsersSid,
		WinBuiltinNetworkConfigurationOperatorsSid,
		WinAccountAdministratorSid,
		WinAccountGuestSid,
		WinAccountKrbtgtSid,
		WinAccountDomainAdminsSid,
		WinAccountDomainUsersSid,
		WinAccountDomainGuestsSid,
		WinAccountComputersSid,
		WinAccountControllersSid,
		WinAccountCertAdminsSid,
		WinAccountSchemaAdminsSid,
		WinAccountEnterpriseAdminsSid,
		WinAccountPolicyAdminsSid,
		WinAccountRasAndIasServersSid,
		WinNTLMAuthenticationSid,
		WinDigestAuthenticationSid,
		WinSChannelAuthenticationSid,
		WinThisOrganizationSid,
		WinOtherOrganizationSid,
		WinBuiltinIncomingForestTrustBuildersSid,
		WinBuiltinPerfMonitoringUsersSid,
		WinBuiltinPerfLoggingUsersSid,
		WinBuiltinAuthorizationAccessSid,
		WinBuiltinTerminalServerLicenseServersSid,
		WinBuiltinDCOMUsersSid,
		WinBuiltinIUsersSid,
		WinIUserSid,
		WinBuiltinCryptoOperatorsSid,
		WinUntrustedLabelSid,
		WinLowLabelSid,
		WinMediumLabelSid,
		WinHighLabelSid,
		WinSystemLabelSid,
		WinWriteRestrictedCodeSid,
		WinCreatorOwnerRightsSid,
		WinCacheablePrincipalsGroupSid,
		WinNonCacheablePrincipalsGroupSid,
		WinEnterpriseReadonlyControllersSid,
		WinAccountReadonlyControllersSid,
		WinBuiltinEventLogReadersGroup,
		WinNewEnterpriseReadonlyControllersSid,
		WinBuiltinCertSvcDComAccessGroup
	}

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool LookupAccountName([In][MarshalAs(UnmanagedType.LPWStr)] string systemName, [In][MarshalAs(UnmanagedType.LPWStr)] string accountName, IntPtr sid, ref int cbSid, StringBuilder referencedDomainName, ref int cbReferencedDomainName, out int use);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool IsWellKnownSid(IntPtr pSid, int sidType);

	public static bool IsDomainUserAccount(string accountName, IEnumerable<WellKnownSidType> sidTypeList)
	{
		if (string.IsNullOrEmpty(accountName))
		{
			return false;
		}
		if (sidTypeList == null)
		{
			throw new ArgumentNullException("sidTypeList");
		}
		IntPtr zero = IntPtr.Zero;
		int cbSid = 0;
		int cbReferencedDomainName = 0;
		StringBuilder referencedDomainName = new StringBuilder();
		int num = 0;
		LookupAccountName(null, accountName, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out var use);
		switch (Marshal.GetLastWin32Error())
		{
		case 1332:
			return false;
		case 122:
			referencedDomainName = new StringBuilder(cbReferencedDomainName);
			try
			{
				zero = Marshal.AllocHGlobal(cbSid);
			}
			catch (OutOfMemoryException)
			{
				return false;
			}
			if (zero == IntPtr.Zero)
			{
				return false;
			}
			try
			{
				if (!LookupAccountName(null, accountName, zero, ref cbSid, referencedDomainName, ref cbReferencedDomainName, out use))
				{
					num = Marshal.GetLastWin32Error();
					return false;
				}
				foreach (WellKnownSidType sidType in sidTypeList)
				{
					if (IsWellKnownSid(zero, (int)sidType))
					{
						return false;
					}
				}
				return true;
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(zero);
				}
			}
		default:
			return false;
		}
	}
}
