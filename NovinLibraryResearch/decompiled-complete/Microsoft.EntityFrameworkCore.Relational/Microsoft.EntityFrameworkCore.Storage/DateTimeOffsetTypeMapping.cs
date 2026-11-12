using System;
using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class DateTimeOffsetTypeMapping : RelationalTypeMapping
{
	private const string DateTimeOffsetFormatConst = "{0:yyyy-MM-dd HH\\:mm\\:ss.fffffffzzz}";

	protected override string SqlLiteralFormatString => "TIMESTAMP '{0:yyyy-MM-dd HH\\:mm\\:ss.fffffffzzz}'";

	public DateTimeOffsetTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(DateTimeOffset), dbType)
	{
	}

	protected DateTimeOffsetTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new DateTimeOffsetTypeMapping(parameters);
	}
}
