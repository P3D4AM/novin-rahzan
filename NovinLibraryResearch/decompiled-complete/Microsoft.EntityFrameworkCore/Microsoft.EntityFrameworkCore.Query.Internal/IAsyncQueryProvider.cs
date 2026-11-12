using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public interface IAsyncQueryProvider : IQueryProvider
{
	TResult ExecuteAsync<TResult>([JetBrains.Annotations.NotNull] Expression expression, CancellationToken cancellationToken = default(CancellationToken));
}
