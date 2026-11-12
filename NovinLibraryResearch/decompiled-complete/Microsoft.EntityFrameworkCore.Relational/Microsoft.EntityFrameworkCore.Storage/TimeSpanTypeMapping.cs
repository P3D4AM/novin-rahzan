using System;
using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class TimeSpanTypeMapping : RelationalTypeMapping
{
	protected override string SqlLiteralFormatString => "'{0}'";

	public TimeSpanTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(TimeSpan), dbType)
	{
	}

	protected TimeSpanTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new TimeSpanTypeMapping(parameters);
	}
}
