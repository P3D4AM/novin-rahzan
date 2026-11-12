using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class EntityEntryGraphNode : IInfrastructure<InternalEntityEntry>
{
	private readonly InternalEntityEntry _entry;

	private readonly InternalEntityEntry _sourceEntry;

	public virtual EntityEntry SourceEntry
	{
		get
		{
			if (_sourceEntry != null)
			{
				return new EntityEntry(_sourceEntry);
			}
			return null;
		}
	}

	public virtual INavigation InboundNavigation { get; }

	public virtual EntityEntry Entry => new EntityEntry(_entry);

	InternalEntityEntry IInfrastructure<InternalEntityEntry>.Instance => _entry;

	[DebuggerStepThrough]
	[EntityFrameworkInternal]
	public EntityEntryGraphNode([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.CanBeNull] InternalEntityEntry sourceEntry, [JetBrains.Annotations.CanBeNull] INavigation inboundNavigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entry, "entry");
		_entry = entry;
		_sourceEntry = sourceEntry;
		InboundNavigation = inboundNavigation;
	}

	public virtual EntityEntryGraphNode CreateNode([JetBrains.Annotations.NotNull] EntityEntryGraphNode currentNode, [JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, [JetBrains.Annotations.NotNull] INavigation reachedVia)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentNode, "currentNode");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(internalEntityEntry, "internalEntityEntry");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reachedVia, "reachedVia");
		return new EntityEntryGraphNode(internalEntityEntry, currentNode.Entry.GetInfrastructure(), reachedVia);
	}
}
public class EntityEntryGraphNode<TState> : EntityEntryGraphNode
{
	public virtual TState NodeState { get; }

	[DebuggerStepThrough]
	[EntityFrameworkInternal]
	public EntityEntryGraphNode([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.CanBeNull] InternalEntityEntry sourceEntry, [JetBrains.Annotations.CanBeNull] INavigation inboundNavigation)
		: base(entry, sourceEntry, inboundNavigation)
	{
		NodeState = state;
	}

	public override EntityEntryGraphNode CreateNode(EntityEntryGraphNode currentNode, InternalEntityEntry internalEntityEntry, INavigation reachedVia)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentNode, "currentNode");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(internalEntityEntry, "internalEntityEntry");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(reachedVia, "reachedVia");
		return new EntityEntryGraphNode<TState>(internalEntityEntry, ((EntityEntryGraphNode<TState>)currentNode).NodeState, currentNode.Entry.GetInfrastructure(), reachedVia);
	}
}
