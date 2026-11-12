using System;
using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class GuidTypeMapping : RelationalTypeMapping
{
	protected override string SqlLiteralFormatString => "'{0}'";

	public GuidTypeMapping([JetBrains.Annotations.NotNull] string storeType, DbType? dbType = null)
		: base(storeType, typeof(Guid), dbType)
	{
	}

	protected GuidTypeMapping(RelationalTypeMappingParameters parameters)
		: base(parameters)
	{
	}

	protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
	{
		return new GuidTypeMapping(parameters);
	}
}
