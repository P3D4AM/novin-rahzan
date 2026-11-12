using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ALTER_EXTERNAL_LIBRARY_OPT_SUBTOKENS : OptLookup
{
	public OPT_ALTER_EXTERNAL_LIBRARY_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Platform,
			StringToken.Remove
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 953, 952 };
	}
}
