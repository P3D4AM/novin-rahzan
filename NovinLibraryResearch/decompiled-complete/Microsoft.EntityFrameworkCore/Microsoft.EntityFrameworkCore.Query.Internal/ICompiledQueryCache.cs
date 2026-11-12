using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public interface ICompiledQueryCache
{
	Func<QueryContext, TResult> GetOrAddQuery<TResult>([JetBrains.Annotations.NotNull] object cacheKey, [JetBrains.Annotations.NotNull] Func<Func<QueryContext, TResult>> compiler);

	Func<QueryContext, TResult> GetOrAddAsyncQuery<TResult>([JetBrains.Annotations.NotNull] object cacheKey, [JetBrains.Annotations.NotNull] Func<Func<QueryContext, TResult>> compiler);
}
