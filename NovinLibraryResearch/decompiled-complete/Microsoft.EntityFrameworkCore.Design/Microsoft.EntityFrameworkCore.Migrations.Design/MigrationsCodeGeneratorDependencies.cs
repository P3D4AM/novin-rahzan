using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public sealed class MigrationsCodeGeneratorDependencies
{
	public IRelationalTypeMappingSource RelationalTypeMappingSource { get; }

	[EntityFrameworkInternal]
	public MigrationsCodeGeneratorDependencies([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource relationalTypeMappingSource)
	{
		RelationalTypeMappingSource = relationalTypeMappingSource;
	}

	public MigrationsCodeGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource relationalTypeMappingSource)
	{
		return new MigrationsCodeGeneratorDependencies(relationalTypeMappingSource);
	}
}
