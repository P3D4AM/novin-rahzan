using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IChangeDetector
{
	void PropertyChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IPropertyBase propertyBase, bool setModified);

	void PropertyChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IPropertyBase propertyBase);

	void DetectChanges([JetBrains.Annotations.NotNull] IStateManager stateManager);

	void DetectChanges([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Suspend();

	void Resume();
}
