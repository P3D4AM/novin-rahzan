using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IEntityEntryGraphIterator
{
	void TraverseGraph<TState>([JetBrains.Annotations.NotNull] EntityEntryGraphNode<TState> node, [JetBrains.Annotations.NotNull] Func<EntityEntryGraphNode<TState>, bool> handleNode);

	Task TraverseGraphAsync<TState>([JetBrains.Annotations.NotNull] EntityEntryGraphNode<TState> node, [JetBrains.Annotations.NotNull] Func<EntityEntryGraphNode<TState>, CancellationToken, Task<bool>> handleNode, CancellationToken cancellationToken = default(CancellationToken));
}
