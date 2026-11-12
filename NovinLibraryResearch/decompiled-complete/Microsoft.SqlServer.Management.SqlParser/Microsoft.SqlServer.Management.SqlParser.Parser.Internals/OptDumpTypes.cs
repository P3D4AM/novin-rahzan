using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptDumpTypes : OptLookup
{
	public OptDumpTypes()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.Master,
			StringToken.Certificate,
			StringToken.Log,
			StringToken.Asymmetric,
			StringToken.Service
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 363, 359, 377, 366, 356 };
	}
}
