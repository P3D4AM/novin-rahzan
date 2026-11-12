using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_AUDIT_TARGET_TYPE_SUBTOKENS : OptLookup
{
	public OPT_AUDIT_TARGET_TYPE_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[5]
		{
			StringToken.ExternalMonitor,
			StringToken.SecurityLog,
			StringToken.MdsLog,
			StringToken.ApplicationLog,
			StringToken.Url
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[5] { 931, 929, 928, 930, 927 };
	}
}
