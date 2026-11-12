using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class ByteTypeMapping : RelationalTypeMapping
{
	public ByteTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(byte), dbType)
	{
	}

	protected ByteTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new ByteTypeMapping(parameters);
	}
}
