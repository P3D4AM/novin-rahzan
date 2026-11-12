using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class EntityGraphAttacher : IEntityGraphAttacher
{
	private readonly IEntityEntryGraphIterator _graphIterator;

	public EntityGraphAttacher([JetBrains.Annotations.NotNull] IEntityEntryGraphIterator graphIterator)
	{
		_graphIterator = graphIterator;
	}

	public virtual void AttachGraph(InternalEntityEntry rootEntry, EntityState targetState, EntityState storeGeneratedWithKeySetTargetState, bool forceStateWhenUnknownKey)
	{
		_graphIterator.TraverseGraph(new EntityEntryGraphNode<(EntityState, EntityState, bool)>(rootEntry, (targetState, storeGeneratedWithKeySetTargetState, forceStateWhenUnknownKey), null, null), PaintAction);
	}

	public virtual Task AttachGraphAsync(InternalEntityEntry rootEntry, EntityState targetState, EntityState storeGeneratedWithKeySetTargetState, bool forceStateWhenUnknownKey, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _graphIterator.TraverseGraphAsync(new EntityEntryGraphNode<(EntityState, EntityState, bool)>(rootEntry, (targetState, storeGeneratedWithKeySetTargetState, forceStateWhenUnknownKey), null, null), PaintActionAsync, cancellationToken);
	}

	private static bool PaintAction(EntityEntryGraphNode<(EntityState TargetState, EntityState StoreGenTargetState, bool Force)> node)
	{
		SetReferenceLoaded(node);
		InternalEntityEntry infrastructure = node.GetInfrastructure();
		if (infrastructure.EntityState != 0)
		{
			return false;
		}
		var (entityState, entityState2, flag) = node.NodeState;
		var (flag2, flag3) = infrastructure.IsKeySet;
		infrastructure.SetEntityState((!flag3) ? EntityState.Added : (flag2 ? entityState2 : entityState), acceptChanges: true, modifyProperties: true, flag ? new EntityState?(entityState) : null);
		return true;
	}

	private static async Task<bool> PaintActionAsync(EntityEntryGraphNode<(EntityState TargetState, EntityState StoreGenTargetState, bool Force)> node, CancellationToken cancellationToken)
	{
		SetReferenceLoaded(node);
		InternalEntityEntry infrastructure = node.GetInfrastructure();
		if (infrastructure.EntityState != 0)
		{
			return false;
		}
		var (entityState, entityState2, flag) = node.NodeState;
		var (flag2, flag3) = infrastructure.IsKeySet;
		await infrastructure.SetEntityStateAsync((!flag3) ? EntityState.Added : (flag2 ? entityState2 : entityState), acceptChanges: true, modifyProperties: true, flag ? new EntityState?(entityState) : null, cancellationToken);
		return true;
	}

	private static void SetReferenceLoaded(EntityEntryGraphNode<(EntityState TargetState, EntityState StoreGenTargetState, bool Force)> node)
	{
		INavigation inboundNavigation = node.InboundNavigation;
		if (inboundNavigation != null && !inboundNavigation.IsCollection())
		{
			node.SourceEntry.GetInfrastructure().SetIsLoaded(inboundNavigation);
		}
	}
}
