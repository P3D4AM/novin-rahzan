using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AFTER_END_SUBTOKENS : OptLookup
{
	public OPT_AFTER_END_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[3]
		{
			StringToken.Try,
			StringToken.Conversation,
			StringToken.Catch
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[3] { 462, 461, 460 };
	}
}
