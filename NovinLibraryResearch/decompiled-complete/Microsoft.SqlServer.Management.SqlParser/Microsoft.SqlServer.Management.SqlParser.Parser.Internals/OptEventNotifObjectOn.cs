using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OptEventNotifObjectOn : OptLookup
{
	public OptEventNotifObjectOn()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[4]
		{
			StringToken.Server,
			StringToken.Type,
			StringToken.Assembly,
			StringToken.Queue
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[4] { 361, 369, 367, 358 };
	}
}
