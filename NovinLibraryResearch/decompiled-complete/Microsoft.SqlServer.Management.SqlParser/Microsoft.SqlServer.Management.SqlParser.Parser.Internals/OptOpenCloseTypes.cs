using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptOpenCloseTypes : OptLookup
{
	public OptOpenCloseTypes()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Asymmetric,
			StringToken.Symmetric,
			StringToken.Master
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 366, 364, 363 };
	}
}
