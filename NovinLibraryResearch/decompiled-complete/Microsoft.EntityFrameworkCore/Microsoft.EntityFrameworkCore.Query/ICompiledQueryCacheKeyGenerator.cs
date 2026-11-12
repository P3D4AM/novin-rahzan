using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query;

public interface ICompiledQueryCacheKeyGenerator
{
	object GenerateCacheKey([JetBrains.Annotations.NotNull] Expression query, bool async);
}
