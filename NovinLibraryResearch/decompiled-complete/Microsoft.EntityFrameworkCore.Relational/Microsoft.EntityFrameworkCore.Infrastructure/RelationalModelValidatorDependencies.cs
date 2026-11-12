using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public sealed class RelationalModelValidatorDependencies
{
	public IRelationalTypeMappingSource TypeMappingSource { get; }

	[EntityFrameworkInternal]
	public RelationalModelValidatorDependencies([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		TypeMappingSource = typeMappingSource;
	}

	public RelationalModelValidatorDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new RelationalModelValidatorDependencies(typeMappingSource);
	}
}
