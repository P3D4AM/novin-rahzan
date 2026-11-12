using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public static class StateManagerExtensions
{
	public static IReadOnlyList<InternalEntityEntry> ToListForState([JetBrains.Annotations.NotNull] this IStateManager stateManager, bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
	{
		List<InternalEntityEntry> list = new List<InternalEntityEntry>(stateManager.GetCountForState(added, modified, deleted, unchanged));
		foreach (InternalEntityEntry item in stateManager.GetEntriesForState(added, modified, deleted, unchanged))
		{
			list.Add(item);
		}
		return list;
	}

	public static IReadOnlyList<InternalEntityEntry> ToList([JetBrains.Annotations.NotNull] this IStateManager stateManager)
	{
		return stateManager.ToListForState(added: true, modified: true, deleted: true, unchanged: true);
	}
}
