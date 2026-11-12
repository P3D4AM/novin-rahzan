using System;
using Microsoft.Data.Common;

namespace Microsoft.Data.Sql;

internal sealed class SqlGenericUtil
{
	private SqlGenericUtil()
	{
	}

	internal static Exception NullCommandText()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.Sql_NullCommandText));
	}

	internal static Exception MismatchedMetaDataDirectionArrayLengths()
	{
		return ADP.Argument(StringsHelper.GetString(Strings.Sql_MismatchedMetaDataDirectionArrayLengths));
	}
}
