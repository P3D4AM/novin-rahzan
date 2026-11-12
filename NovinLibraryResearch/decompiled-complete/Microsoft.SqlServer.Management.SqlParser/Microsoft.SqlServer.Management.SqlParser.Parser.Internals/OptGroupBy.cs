using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptGroupBy : OptLookup
{
	public OptGroupBy()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Rollup,
			StringToken.Cube
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 380, 379 };
	}
}
