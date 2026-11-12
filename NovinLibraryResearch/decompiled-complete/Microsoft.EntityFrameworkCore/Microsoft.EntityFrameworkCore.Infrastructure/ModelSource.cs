using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ModelSource : IModelSource
{
	private readonly object _syncObject = new object();

	protected virtual ModelSourceDependencies Dependencies { get; }

	public ModelSource([JetBrains.Annotations.NotNull] ModelSourceDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IModel GetModel(DbContext context, IConventionSetBuilder conventionSetBuilder)
	{
		IMemoryCache memoryCache = Dependencies.MemoryCache;
		object key = Dependencies.ModelCacheKeyFactory.Create(context);
		if (!memoryCache.TryGetValue<IModel>(key, out var value))
		{
			lock (_syncObject)
			{
				if (!memoryCache.TryGetValue<IModel>(key, out value))
				{
					value = CreateModel(context, conventionSetBuilder);
					value = memoryCache.Set(key, value, new MemoryCacheEntryOptions
					{
						Size = 100L,
						Priority = CacheItemPriority.High
					});
				}
			}
		}
		return value;
	}

	protected virtual IModel CreateModel([JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] IConventionSetBuilder conventionSetBuilder)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		ModelBuilder modelBuilder = new ModelBuilder(conventionSetBuilder.CreateConventionSet());
		Dependencies.ModelCustomizer.Customize(modelBuilder, context);
		return modelBuilder.FinalizeModel();
	}
}
