using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class DecimalTypeMapping : RelationalTypeMapping
{
	private const string DecimalFormatConst = "{0:0.0###########################}";

	protected override string SqlLiteralFormatString => "{0:0.0###########################}";

	public DecimalTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(decimal), dbType)
	{
	}

	protected DecimalTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new DecimalTypeMapping(parameters);
	}
}
