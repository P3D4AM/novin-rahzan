using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class ChangeTracker : IResettableService
{
	private readonly IModel _model;

	private QueryTrackingBehavior _queryTrackingBehavior;

	private readonly QueryTrackingBehavior _defaultQueryTrackingBehavior;

	public virtual bool AutoDetectChangesEnabled { get; set; } = true;


	public virtual bool LazyLoadingEnabled { get; set; } = true;


	public virtual QueryTrackingBehavior QueryTrackingBehavior
	{
		get
		{
			return _queryTrackingBehavior;
		}
		set
		{
			_queryTrackingBehavior = value;
		}
	}

	public virtual CascadeTiming DeleteOrphansTiming
	{
		get
		{
			return StateManager.DeleteOrphansTiming;
		}
		set
		{
			StateManager.DeleteOrphansTiming = value;
		}
	}

	public virtual CascadeTiming CascadeDeleteTiming
	{
		get
		{
			return StateManager.CascadeDeleteTiming;
		}
		set
		{
			StateManager.CascadeDeleteTiming = value;
		}
	}

	public virtual DbContext Context { get; }

	private IStateManager StateManager { get; }

	private IChangeDetector ChangeDetector { get; }

	private IEntityEntryGraphIterator GraphIterator { get; }

	public event EventHandler<EntityTrackedEventArgs> Tracked
	{
		add
		{
			StateManager.Tracked += value;
		}
		remove
		{
			StateManager.Tracked -= value;
		}
	}

	public event EventHandler<EntityStateChangedEventArgs> StateChanged
	{
		add
		{
			StateManager.StateChanged += value;
		}
		remove
		{
			StateManager.StateChanged -= value;
		}
	}

	[EntityFrameworkInternal]
	public ChangeTracker([JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IChangeDetector changeDetector, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IEntityEntryGraphIterator graphIterator)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(context, "context");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(stateManager, "stateManager");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(changeDetector, "changeDetector");
		Context = context;
		_defaultQueryTrackingBehavior = context.GetService<IDbContextOptions>().Extensions.OfType<CoreOptionsExtension>().FirstOrDefault()?.QueryTrackingBehavior ?? QueryTrackingBehavior.TrackAll;
		_queryTrackingBehavior = _defaultQueryTrackingBehavior;
		StateManager = stateManager;
		ChangeDetector = changeDetector;
		_model = model;
		GraphIterator = graphIterator;
	}

	public virtual IEnumerable<EntityEntry> Entries()
	{
		TryDetectChanges();
		return StateManager.Entries.Select((InternalEntityEntry e) => new EntityEntry(e));
	}

	public virtual IEnumerable<EntityEntry<TEntity>> Entries<TEntity>() where TEntity : class
	{
		TryDetectChanges();
		return from e in StateManager.Entries
			where e.Entity is TEntity
			select new EntityEntry<TEntity>(e);
	}

	private void TryDetectChanges()
	{
		if (AutoDetectChangesEnabled)
		{
			DetectChanges();
		}
	}

	public virtual bool HasChanges()
	{
		TryDetectChanges();
		return StateManager.ChangedCount > 0;
	}

	public virtual void DetectChanges()
	{
		if ((string)_model["ChangeDetector.SkipDetectChanges"] != "true")
		{
			ChangeDetector.DetectChanges(StateManager);
		}
	}

	public virtual void AcceptAllChanges()
	{
		StateManager.AcceptAllChanges();
	}

	public virtual void TrackGraph([JetBrains.Annotations.NotNull] object rootEntity, [JetBrains.Annotations.NotNull] Action<EntityEntryGraphNode> callback)
	{
		TrackGraph(rootEntity, callback, delegate(EntityEntryGraphNode<Action<EntityEntryGraphNode>> n)
		{
			if (n.Entry.State != 0)
			{
				return false;
			}
			n.NodeState(n);
			return n.Entry.State != EntityState.Detached;
		});
	}

	public virtual void TrackGraph<TState>([JetBrains.Annotations.NotNull] object rootEntity, [JetBrains.Annotations.CanBeNull] TState state, [JetBrains.Annotations.NotNull] Func<EntityEntryGraphNode<TState>, bool> callback)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(rootEntity, "rootEntity");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(callback, "callback");
		InternalEntityEntry orCreateEntry = StateManager.GetOrCreateEntry(rootEntity);
		GraphIterator.TraverseGraph(new EntityEntryGraphNode<TState>(orCreateEntry, state, null, null), callback);
	}

	public virtual void CascadeChanges()
	{
		if (AutoDetectChangesEnabled)
		{
			DetectChanges();
		}
		StateManager.CascadeChanges(force: true);
	}

	void IResettableService.ResetState()
	{
		_queryTrackingBehavior = _defaultQueryTrackingBehavior;
		AutoDetectChangesEnabled = true;
		LazyLoadingEnabled = true;
		CascadeDeleteTiming = CascadeTiming.Immediate;
		DeleteOrphansTiming = CascadeTiming.Immediate;
	}

	Task IResettableService.ResetStateAsync(CancellationToken cancellationToken)
	{
		((IResettableService)this).ResetState();
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return base.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
