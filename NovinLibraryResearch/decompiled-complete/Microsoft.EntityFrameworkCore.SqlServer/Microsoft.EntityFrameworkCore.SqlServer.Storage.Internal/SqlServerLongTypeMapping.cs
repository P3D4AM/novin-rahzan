using System.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerLongTypeMapping : LongTypeMapping
{
	public SqlServerLongTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, dbType)
	{
	}

	protected SqlServerLongTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerLongTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		return "CAST(" + base.GenerateNonNullSqlLiteral(value) + " AS " + StoreType + ")";
	}
}
