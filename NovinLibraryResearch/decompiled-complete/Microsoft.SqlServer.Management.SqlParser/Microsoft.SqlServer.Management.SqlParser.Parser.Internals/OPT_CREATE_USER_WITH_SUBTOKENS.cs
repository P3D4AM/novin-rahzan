using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_USER_WITH_SUBTOKENS : OptLookup
{
	public OPT_CREATE_USER_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[7]
		{
			StringToken.Sid,
			StringToken.DefaultLanguage,
			StringToken.Password,
			StringToken.DefaultSchema,
			StringToken.ObjectId,
			StringToken.Type,
			StringToken.AllowEncryptedValueModifications
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[7] { 647, 647, 645, 647, 647, 646, 647 };
	}
}
