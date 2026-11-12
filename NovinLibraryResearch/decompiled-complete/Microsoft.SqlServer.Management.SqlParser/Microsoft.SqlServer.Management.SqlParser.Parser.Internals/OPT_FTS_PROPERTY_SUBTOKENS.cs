using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_FTS_PROPERTY_SUBTOKENS : OptLookup
{
	public OPT_FTS_PROPERTY_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Property,
			StringToken.List
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 636, 637 };
	}
}
