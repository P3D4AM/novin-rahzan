using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class ShapedQueryCompilingExpressionVisitorDependencies
{
	public IEntityMaterializerSource EntityMaterializerSource { get; }

	public ITypeMappingSource TypeMappingSource { get; }

	public IMemoryCache MemoryCache { get; }

	[EntityFrameworkInternal]
	public ShapedQueryCompilingExpressionVisitorDependencies([JetBrains.Annotations.NotNull] IEntityMaterializerSource entityMaterializerSource, [JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource, [JetBrains.Annotations.NotNull] IMemoryCache memoryCache)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityMaterializerSource, "entityMaterializerSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(typeMappingSource, "typeMappingSource");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memoryCache, "memoryCache");
		EntityMaterializerSource = entityMaterializerSource;
		TypeMappingSource = typeMappingSource;
		MemoryCache = memoryCache;
	}

	public ShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IEntityMaterializerSource entityMaterializerSource)
	{
		return new ShapedQueryCompilingExpressionVisitorDependencies(entityMaterializerSource, TypeMappingSource, MemoryCache);
	}

	public ShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] ITypeMappingSource typeMappingSource)
	{
		return new ShapedQueryCompilingExpressionVisitorDependencies(EntityMaterializerSource, typeMappingSource, MemoryCache);
	}

	public ShapedQueryCompilingExpressionVisitorDependencies With([JetBrains.Annotations.NotNull] IMemoryCache memoryCache)
	{
		return new ShapedQueryCompilingExpressionVisitorDependencies(EntityMaterializerSource, TypeMappingSource, memoryCache);
	}
}
