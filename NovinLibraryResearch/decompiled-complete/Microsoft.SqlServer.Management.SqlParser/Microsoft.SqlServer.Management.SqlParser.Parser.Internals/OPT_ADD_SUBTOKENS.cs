using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_ADD_SUBTOKENS : OptLookup
{
	public OPT_ADD_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Counter,
			StringToken.Signature,
			StringToken.Sensitivity,
			StringToken.Classification
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 455, 454, 456, 457 };
	}
}
