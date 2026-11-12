using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class EntityEntryGraphIterator : IEntityEntryGraphIterator
{
	public virtual void TraverseGraph<TState>(EntityEntryGraphNode<TState> node, Func<EntityEntryGraphNode<TState>, bool> handleNode)
	{
		if (!handleNode(node))
		{
			return;
		}
		InternalEntityEntry infrastructure = node.GetInfrastructure();
		IEnumerable<Navigation> navigations = ((EntityType)infrastructure.EntityType).GetNavigations();
		IStateManager stateManager = infrastructure.StateManager;
		foreach (Navigation item in navigations)
		{
			object obj = infrastructure[item];
			if (obj == null)
			{
				continue;
			}
			EntityType targetType = item.GetTargetType();
			if (item.IsCollection())
			{
				foreach (object item2 in ((IEnumerable)obj).Cast<object>().ToList())
				{
					InternalEntityEntry orCreateEntry = stateManager.GetOrCreateEntry(item2, targetType);
					TraverseGraph((EntityEntryGraphNode<TState>)node.CreateNode(node, orCreateEntry, item), handleNode);
				}
			}
			else
			{
				InternalEntityEntry orCreateEntry2 = stateManager.GetOrCreateEntry(obj, targetType);
				TraverseGraph((EntityEntryGraphNode<TState>)node.CreateNode(node, orCreateEntry2, item), handleNode);
			}
		}
	}

	public virtual async Task TraverseGraphAsync<TState>(EntityEntryGraphNode<TState> node, Func<EntityEntryGraphNode<TState>, CancellationToken, Task<bool>> handleNode, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!(await handleNode(node, cancellationToken)))
		{
			return;
		}
		InternalEntityEntry internalEntityEntry = node.GetInfrastructure();
		IEnumerable<INavigation> navigations = internalEntityEntry.EntityType.GetNavigations();
		IStateManager stateManager = internalEntityEntry.StateManager;
		foreach (INavigation navigation in navigations)
		{
			object obj = internalEntityEntry[navigation];
			if (obj == null)
			{
				continue;
			}
			IEntityType targetType = navigation.GetTargetType();
			if (navigation.IsCollection())
			{
				foreach (object item in ((IEnumerable)obj).Cast<object>().ToList())
				{
					InternalEntityEntry orCreateEntry = stateManager.GetOrCreateEntry(item, targetType);
					await TraverseGraphAsync((EntityEntryGraphNode<TState>)node.CreateNode(node, orCreateEntry, navigation), handleNode, cancellationToken);
				}
			}
			else
			{
				InternalEntityEntry orCreateEntry2 = stateManager.GetOrCreateEntry(obj, targetType);
				await TraverseGraphAsync((EntityEntryGraphNode<TState>)node.CreateNode(node, orCreateEntry2, navigation), handleNode, cancellationToken);
			}
		}
	}
}
