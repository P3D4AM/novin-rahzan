using System.Data;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerDecimalTypeMapping : DecimalTypeMapping
{
	public SqlServerDecimalTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null, int? precision = null, int? scale = null, StoreTypePostfix storeTypePostfix = StoreTypePostfix.None)
		: base(new RelationalTypeMappingParameters(new CoreTypeMappingParameters(typeof(decimal)), storeType, storeTypePostfix, dbType).WithPrecisionAndScale(precision, scale))
	{
	}

	protected SqlServerDecimalTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new SqlServerDecimalTypeMapping(parameters);
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
