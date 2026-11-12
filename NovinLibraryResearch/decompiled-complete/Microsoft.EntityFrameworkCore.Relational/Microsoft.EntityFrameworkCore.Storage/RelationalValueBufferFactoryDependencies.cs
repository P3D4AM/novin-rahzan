using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalValueBufferFactoryDependencies
{
	public IRelationalTypeMappingSource TypeMappingSource { get; }

	public ICoreSingletonOptions CoreOptions { get; }

	[EntityFrameworkInternal]
	public RelationalValueBufferFactoryDependencies([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] ICoreSingletonOptions coreOptions)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(coreOptions, "coreOptions");
		TypeMappingSource = typeMappingSource;
		CoreOptions = coreOptions;
	}

	public RelationalValueBufferFactoryDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new RelationalValueBufferFactoryDependencies(typeMappingSource, CoreOptions);
	}

	public RelationalValueBufferFactoryDependencies With([JetBrains.Annotations.NotNull] ICoreSingletonOptions coreOptions)
	{
		return new RelationalValueBufferFactoryDependencies(TypeMappingSource, coreOptions);
	}
}
