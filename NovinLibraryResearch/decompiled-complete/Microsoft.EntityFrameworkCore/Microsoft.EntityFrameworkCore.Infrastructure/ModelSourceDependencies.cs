using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public sealed class ModelSourceDependencies
{
	public IModelCustomizer ModelCustomizer { get; }

	public IModelCacheKeyFactory ModelCacheKeyFactory { get; }

	public IMemoryCache MemoryCache { get; }

	[EntityFrameworkInternal]
	public ModelSourceDependencies([JetBrains.Annotations.NotNull] IModelCustomizer modelCustomizer, [JetBrains.Annotations.NotNull] IModelCacheKeyFactory modelCacheKeyFactory, [JetBrains.Annotations.NotNull] IMemoryCache memoryCache)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelCustomizer, "modelCustomizer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(modelCacheKeyFactory, "modelCacheKeyFactory");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(memoryCache, "memoryCache");
		ModelCustomizer = modelCustomizer;
		ModelCacheKeyFactory = modelCacheKeyFactory;
		MemoryCache = memoryCache;
	}

	public ModelSourceDependencies With([JetBrains.Annotations.NotNull] IModelCustomizer modelCustomizer)
	{
		return new ModelSourceDependencies(modelCustomizer, ModelCacheKeyFactory, MemoryCache);
	}

	public ModelSourceDependencies With([JetBrains.Annotations.NotNull] IModelCacheKeyFactory modelCacheKeyFactory)
	{
		return new ModelSourceDependencies(ModelCustomizer, modelCacheKeyFactory, MemoryCache);
	}

	public ModelSourceDependencies With([JetBrains.Annotations.NotNull] IMemoryCache memoryCache)
	{
		return new ModelSourceDependencies(ModelCustomizer, ModelCacheKeyFactory, memoryCache);
	}
}
