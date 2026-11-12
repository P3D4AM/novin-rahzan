using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalCommandBuilderDependencies
{
	public IRelationalTypeMappingSource TypeMappingSource { get; }

	[EntityFrameworkInternal]
	public RelationalCommandBuilderDependencies([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		TypeMappingSource = typeMappingSource;
	}

	public RelationalCommandBuilderDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new RelationalCommandBuilderDependencies(typeMappingSource);
	}
}
