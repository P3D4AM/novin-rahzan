using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IModelCacheKeyFactory
{
	object Create([JetBrains.Annotations.NotNull] DbContext context);
}
