using System;
using System.Data;
using System.Globalization;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class FloatTypeMapping : RelationalTypeMapping
{
	public FloatTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(float), dbType)
	{
	}

	protected FloatTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new FloatTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		return Convert.ToSingle(value).ToString("R", CultureInfo.InvariantCulture);
	}
}
