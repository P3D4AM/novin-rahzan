using System;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public interface IQueryCompiler
{
	TResult Execute<TResult>([JetBrains.Annotations.NotNull] Expression query);

	TResult ExecuteAsync<TResult>([JetBrains.Annotations.NotNull] Expression query, CancellationToken cancellationToken);

	Func<QueryContext, TResult> CreateCompiledQuery<TResult>([JetBrains.Annotations.NotNull] Expression query);

	Func<QueryContext, TResult> CreateCompiledAsyncQuery<TResult>([JetBrains.Annotations.NotNull] Expression query);
}
