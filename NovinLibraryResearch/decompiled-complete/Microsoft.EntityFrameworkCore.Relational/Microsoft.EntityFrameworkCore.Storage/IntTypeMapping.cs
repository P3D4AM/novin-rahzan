using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class IntTypeMapping : RelationalTypeMapping
{
	public IntTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(int), dbType)
	{
	}

	protected IntTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new IntTypeMapping(parameters);
	}
}
