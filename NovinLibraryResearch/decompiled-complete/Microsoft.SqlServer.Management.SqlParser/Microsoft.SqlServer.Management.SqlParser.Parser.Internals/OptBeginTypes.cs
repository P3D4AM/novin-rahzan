using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptBeginTypes : OptLookup
{
	public OptBeginTypes()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[2]
		{
			StringToken.Dialog,
			StringToken.Conversation
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[2] { 370, 371 };
	}
}
