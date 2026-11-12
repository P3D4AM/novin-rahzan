using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface ILocalViewListener
{
	void RegisterView([JetBrains.Annotations.NotNull] Action<InternalEntityEntry, EntityState> viewAction);

	void StateChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState newState);

	void StateChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState oldState, bool fromQuery);
}
