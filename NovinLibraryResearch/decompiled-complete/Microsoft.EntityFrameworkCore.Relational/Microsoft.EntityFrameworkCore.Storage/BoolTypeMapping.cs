using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class BoolTypeMapping : RelationalTypeMapping
{
	public BoolTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(bool), dbType)
	{
	}

	protected BoolTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new BoolTypeMapping(parameters);
	}

	protected override string GenerateNonNullSqlLiteral(object value)
	{
		if (!(bool)value)
		{
			return "0";
		}
		return "1";
	}
}
