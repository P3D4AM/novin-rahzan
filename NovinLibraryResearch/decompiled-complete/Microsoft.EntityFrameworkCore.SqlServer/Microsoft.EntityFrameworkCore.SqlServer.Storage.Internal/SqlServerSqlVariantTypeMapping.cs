using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerSqlVariantTypeMapping : RelationalTypeMapping
{
	public SqlServerSqlVariantTypeMapping([JetBrains.Annotations.NotNull] string storeType)
		: base(storeType, typeof(object))
	{
	}

	protected SqlServerSqlVariantTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerSqlVariantTypeMapping(parameters);
	}
}
