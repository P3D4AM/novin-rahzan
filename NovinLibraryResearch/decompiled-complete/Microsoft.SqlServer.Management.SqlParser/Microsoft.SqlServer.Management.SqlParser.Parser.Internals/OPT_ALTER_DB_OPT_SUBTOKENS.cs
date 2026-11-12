using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_DB_OPT_SUBTOKENS : OptLookup
{
	public OPT_ALTER_DB_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 488)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[31]
		{
			StringToken.AcceleratedDatabaseRecovery,
			StringToken.Rebuild,
			StringToken.Containment,
			StringToken.FailOver,
			StringToken.Secondary,
			StringToken.TargetRecoveryTime,
			StringToken.Distribution,
			StringToken.Encryption,
			StringToken.ForceFailoverAllowDataLoss,
			StringToken.Log,
			StringToken.Hardening,
			StringToken.Modify,
			StringToken.AutomaticTuning,
			StringToken.QueryStore,
			StringToken.FileStream,
			StringToken.PersistentLogBuffer,
			StringToken.FileGroup,
			StringToken.Remove,
			StringToken.DelayedDurability,
			StringToken.StalePageDetection,
			StringToken.TemporalHistoryRetention,
			StringToken.Partition,
			StringToken.DataRetention,
			StringToken.AutoCreateStatistics,
			StringToken.MemoryOptimized,
			StringToken.PersistentVersionStoreFilegroup,
			StringToken.Hadr,
			StringToken.Name,
			StringToken.BackupStorageRedundancy,
			StringToken.ChangeTracking,
			StringToken.RemoteDataArchive
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[31]
		{
			511, 497, 491, 494, 499, 492, 710, 504, 498, 500,
			509, 496, 505, 503, 487, 510, 501, 495, 493, 513,
			508, 711, 515, 506, 514, 512, 490, 502, 516, 486,
			507
		};
	}
}
