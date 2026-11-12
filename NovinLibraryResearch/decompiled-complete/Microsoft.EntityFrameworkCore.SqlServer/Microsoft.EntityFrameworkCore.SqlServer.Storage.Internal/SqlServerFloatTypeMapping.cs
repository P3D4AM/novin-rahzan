using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerFloatTypeMapping : FloatTypeMapping
{
	public SqlServerFloatTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, dbType)
	{
	}

	protected SqlServerFloatTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerFloatTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		return "CAST(" + base.GenerateNonNullSqlLiteral(value) + " AS " + StoreType + ")";
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
