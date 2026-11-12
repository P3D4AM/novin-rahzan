using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IKeyPropagator
{
	InternalEntityEntry PropagateValue([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IProperty property);

	Task<InternalEntityEntry> PropagateValueAsync([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IProperty property, CancellationToken cancellationToken = default(CancellationToken));
}
