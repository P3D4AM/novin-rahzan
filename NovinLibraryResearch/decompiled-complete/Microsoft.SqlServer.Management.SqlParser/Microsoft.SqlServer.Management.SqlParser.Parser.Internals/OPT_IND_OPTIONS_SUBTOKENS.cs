using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_IND_OPTIONS_SUBTOKENS : OptLookup
{
	public OPT_IND_OPTIONS_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), 571)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[10]
		{
			StringToken.DataCompression,
			StringToken.BucketCount,
			StringToken.MaxDuration,
			StringToken.MaxDop,
			StringToken.Distribution,
			StringToken.Resumable,
			StringToken.CompressionDelay,
			StringToken.StatisticsOnly,
			StringToken.Online,
			StringToken.WaitAtLowPriority
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[10] { 568, 573, 580, 569, 572, 579, 575, 570, 574, 581 };
	}
}
