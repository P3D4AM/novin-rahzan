using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Update;

public sealed class UpdateSqlGeneratorDependencies
{
	public ISqlGenerationHelper SqlGenerationHelper { get; }

	public IRelationalTypeMappingSource TypeMappingSource { get; }

	[EntityFrameworkInternal]
	public UpdateSqlGeneratorDependencies([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper, [JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(sqlGenerationHelper, "sqlGenerationHelper");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		SqlGenerationHelper = sqlGenerationHelper;
		TypeMappingSource = typeMappingSource;
	}

	public UpdateSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] ISqlGenerationHelper sqlGenerationHelper)
	{
		return new UpdateSqlGeneratorDependencies(sqlGenerationHelper, TypeMappingSource);
	}

	public UpdateSqlGeneratorDependencies With([JetBrains.Annotations.NotNull] IRelationalTypeMappingSource typeMappingSource)
	{
		return new UpdateSqlGeneratorDependencies(SqlGenerationHelper, typeMappingSource);
	}
}
