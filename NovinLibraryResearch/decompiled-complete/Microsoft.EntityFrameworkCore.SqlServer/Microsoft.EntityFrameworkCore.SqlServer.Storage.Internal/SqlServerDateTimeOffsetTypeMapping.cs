using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerDateTimeOffsetTypeMapping : DateTimeOffsetTypeMapping
{
	private const string DateTimeOffsetFormatConst = "{0:yyyy-MM-ddTHH:mm:ss.fffffffzzz}";

	protected override string SqlLiteralFormatString => "'{0:yyyy-MM-ddTHH:mm:ss.fffffffzzz}'";

	public SqlServerDateTimeOffsetTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = System.Data.DbType.DateTimeOffset)
		: base(storeType, dbType)
	{
	}

	protected SqlServerDateTimeOffsetTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerDateTimeOffsetTypeMapping(parameters);
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
