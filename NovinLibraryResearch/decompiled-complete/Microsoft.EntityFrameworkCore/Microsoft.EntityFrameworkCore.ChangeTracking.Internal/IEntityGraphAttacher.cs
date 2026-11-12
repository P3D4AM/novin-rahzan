using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IEntityGraphAttacher
{
	void AttachGraph([JetBrains.Annotations.NotNull] InternalEntityEntry rootEntry, EntityState targetState, EntityState storeGeneratedWithKeySetTargetState, bool forceStateWhenUnknownKey);

	Task AttachGraphAsync([JetBrains.Annotations.NotNull] InternalEntityEntry rootEntry, EntityState targetState, EntityState storeGeneratedWithKeySetTargetState, bool forceStateWhenUnknownKey, CancellationToken cancellationToken = default(CancellationToken));
}
