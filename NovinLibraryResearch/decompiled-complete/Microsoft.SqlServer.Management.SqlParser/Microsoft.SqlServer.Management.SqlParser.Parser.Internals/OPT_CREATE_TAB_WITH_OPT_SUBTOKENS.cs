using Microsoft.SqlServer.Management.SqlParser.Common;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

internal sealed class OPT_CREATE_TAB_WITH_OPT_SUBTOKENS : OptLookup
{
	public OPT_CREATE_TAB_WITH_OPT_SUBTOKENS()
		: base(GetStringTokens(), GetTokenIds(), -1)
	{
	}

	private static StringToken[] GetStringTokens()
	{
		return new StringToken[14]
		{
			StringToken.FileTableCollateFilename,
			StringToken.Ledger,
			StringToken.DataDeletion,
			StringToken.DataCompression,
			StringToken.Durability,
			StringToken.FileTableStreamIdUniqueConstraintName,
			StringToken.SystemVersioning,
			StringToken.FileTableFullpathUniqueConstraintName,
			StringToken.Location,
			StringToken.RemoteDataArchive,
			StringToken.Heap,
			StringToken.FileTableDirectory,
			StringToken.FileTablePrimaryKeyConstraintName,
			StringToken.MemoryOptimized
		};
	}

	private static int[] GetTokenIds()
	{
		return new int[14]
		{
			702, 715, 716, 700, 707, 704, 708, 705, 714, 709,
			713, 701, 703, 706
		};
	}
}
