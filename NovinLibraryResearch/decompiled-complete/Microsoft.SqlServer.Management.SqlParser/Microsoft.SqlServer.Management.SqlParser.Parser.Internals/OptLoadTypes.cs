using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptLoadTypes : OptLookup
{
	public OptLoadTypes()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Log,
			StringToken.Master,
			StringToken.Asymmetric,
			StringToken.Service
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 377, 363, 366, 356 };
	}
}
