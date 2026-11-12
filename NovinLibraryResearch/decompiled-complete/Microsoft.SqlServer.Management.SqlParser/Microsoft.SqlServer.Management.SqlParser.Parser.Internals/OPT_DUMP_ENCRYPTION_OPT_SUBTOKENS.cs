using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_DUMP_ENCRYPTION_OPT_SUBTOKENS : OptLookup
{
	public OPT_DUMP_ENCRYPTION_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Algorithm,
			StringToken.Certificate,
			StringToken.Server,
			StringToken.Asymmetric
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 523, 525, 524, 526 };
	}
}
