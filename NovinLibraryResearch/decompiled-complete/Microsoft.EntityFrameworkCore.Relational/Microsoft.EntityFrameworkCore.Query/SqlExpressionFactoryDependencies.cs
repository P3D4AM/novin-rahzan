using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class SqlExpressionFactoryDependencies
{
	public IRelationalTypeMappingSource TypeMappingSource { get; }

	[EntityFrameworkInternal]
	public SqlExpressionFactoryDependencies(IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		TypeMappingSource = typeMappingSource;
	}

	public SqlExpressionFactoryDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new SqlExpressionFactoryDependencies(typeMappingSource);
	}
}
