using System;
using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class DateTimeTypeMapping : RelationalTypeMapping
{
	private const string DateTimeFormatConst = "{0:yyyy-MM-dd HH\\:mm\\:ss.fffffff}";

	protected override string SqlLiteralFormatString => "TIMESTAMP '{0:yyyy-MM-dd HH\\:mm\\:ss.fffffff}'";

	public DateTimeTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(DateTime), dbType)
	{
	}

	protected DateTimeTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new DateTimeTypeMapping(parameters);
	}
}
