using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_SEQUENCE_OPTIONS_SUBTOKENS : OptLookup
{
	public OPT_SEQUENCE_OPTIONS_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[8]
		{
			StringToken.Increment,
			StringToken.Start,
			StringToken.MaxValue,
			StringToken.No,
			StringToken.Cache,
			StringToken.Restart,
			StringToken.MinValue,
			StringToken.Cycle
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[8] { 599, 598, 601, 604, 603, 597, 600, 602 };
	}
}
