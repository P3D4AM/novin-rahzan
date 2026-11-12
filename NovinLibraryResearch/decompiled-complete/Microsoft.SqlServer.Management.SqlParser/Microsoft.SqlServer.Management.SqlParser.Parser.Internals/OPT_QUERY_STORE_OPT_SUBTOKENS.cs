using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_QUERY_STORE_OPT_SUBTOKENS : OptLookup
{
	public OPT_QUERY_STORE_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[10]
		{
			StringToken.QueryCaptureMode,
			StringToken.SizeBasedCleanupMode,
			StringToken.IntervalLengthMinutes,
			StringToken.CleanupPolicy,
			StringToken.DesiredState,
			StringToken.MaxPlansPerQuery,
			StringToken.OperationMode,
			StringToken.MaxStorageSizeMb,
			StringToken.FlushIntervalSeconds,
			StringToken.DataFlushIntervalSeconds
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[10] { 865, 866, 871, 864, 862, 872, 863, 868, 869, 870 };
	}
}
