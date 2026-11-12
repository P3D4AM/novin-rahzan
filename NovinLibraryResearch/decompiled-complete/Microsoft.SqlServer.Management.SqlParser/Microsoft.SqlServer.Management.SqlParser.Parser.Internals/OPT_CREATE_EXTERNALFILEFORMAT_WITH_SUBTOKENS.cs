using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_EXTERNALFILEFORMAT_WITH_SUBTOKENS : OptLookup
{
	public OPT_CREATE_EXTERNALFILEFORMAT_WITH_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[18]
		{
			StringToken.False,
			StringToken.Encoding,
			StringToken.FieldTerminator,
			StringToken.RowTerminator,
			StringToken.Orc,
			StringToken.FirstRow,
			StringToken.DelimitedText,
			StringToken.DataCompression,
			StringToken.True,
			StringToken.Json,
			StringToken.SerdeMethod,
			StringToken.StringDelimiter,
			StringToken.FormatOptions,
			StringToken.Parquet,
			StringToken.UseTypeDefault,
			StringToken.FormatType,
			StringToken.Rcfile,
			StringToken.DateFormat
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[18]
		{
			782, 779, 774, 780, 785, 787, 783, 773, 781, 790,
			778, 775, 771, 786, 777, 772, 784, 776
		};
	}
}
