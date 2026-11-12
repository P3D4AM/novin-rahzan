using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IInternalEntityEntrySubscriber
{
	bool SnapshotAndSubscribe([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Unsubscribe([JetBrains.Annotations.NotNull] InternalEntityEntry entry);
}
