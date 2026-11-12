using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class ModelCacheKeyFactory : IModelCacheKeyFactory
{
	public ModelCacheKeyFactory([JetBrains.Annotations.NotNull] ModelCacheKeyFactoryDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
	}

	public virtual object Create(DbContext context)
	{
		return new ModelCacheKey(context);
	}
}
