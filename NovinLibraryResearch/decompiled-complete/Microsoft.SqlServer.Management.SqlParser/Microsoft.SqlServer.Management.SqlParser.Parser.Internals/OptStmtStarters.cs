using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptStmtStarters : OptLookup
{
	public OptStmtStarters()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[9]
		{
			StringToken.Throw,
			StringToken.Disable,
			StringToken.Get,
			StringToken.Receive,
			StringToken.Revert,
			StringToken.Enable,
			StringToken.Merge,
			StringToken.Move,
			StringToken.Send
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[9] { 372, 355, 352, 351, 383, 354, 384, 350, 353 };
	}
}
