namespace Microsoft.SqlServer.Management.HadrData;

public static class Constants
{
	internal static readonly UserSecurity.WellKnownSidType[] WellKnownSidTypes = new UserSecurity.WellKnownSidType[6]
	{
		UserSecurity.WellKnownSidType.WinLocalSystemSid,
		UserSecurity.WellKnownSidType.WinLocalServiceSid,
		UserSecurity.WellKnownSidType.WinNetworkServiceSid,
		UserSecurity.WellKnownSidType.WinAnonymousSid,
		UserSecurity.WellKnownSidType.WinWorldSid,
		UserSecurity.WellKnownSidType.WinNtAuthoritySid
	};

	internal static string[] DefaultDatabaseInitFields => new string[12]
	{
		"AvailabilityGroupName", "HasFullBackup", "IsAccessible", "IsDatabaseSnapshot", "IsMirroringEnabled", "IsSystemObject", "Name", "RecoveryModel", "UserAccess", "ReadOnly",
		"AutoClose", "HasDatabaseEncryptionKey"
	};
}
