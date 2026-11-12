using System;
using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerDoubleTypeMapping : DoubleTypeMapping
{
	public SqlServerDoubleTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, dbType)
	{
	}

	protected SqlServerDoubleTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerDoubleTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		string text = base.GenerateNonNullSqlLiteral(value);
		double d = Convert.ToDouble(value);
		if (text.Contains("E") || text.Contains("e") || double.IsNaN(d) || double.IsInfinity(d))
		{
			return text;
		}
		return text + "E0";
	}

	protected override void ConfigureParameter(DbParameter parameter)
	{
		base.ConfigureParameter(parameter);
		if (Size.HasValue && Size.Value != -1)
		{
			parameter.Size = Size.Value;
		}
	}
}
