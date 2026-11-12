using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class UpdateAdapter : IUpdateAdapter
{
	private readonly IStateManager _stateManager;

	private readonly IChangeDetector _changeDetector;

	public virtual CascadeTiming DeleteOrphansTiming
	{
		get
		{
			return _stateManager.DeleteOrphansTiming;
		}
		set
		{
			_stateManager.DeleteOrphansTiming = value;
		}
	}

	public virtual CascadeTiming CascadeDeleteTiming
	{
		get
		{
			return _stateManager.CascadeDeleteTiming;
		}
		set
		{
			_stateManager.CascadeDeleteTiming = value;
		}
	}

	public virtual IEnumerable<IUpdateEntry> Entries => _stateManager.Entries;

	public virtual IModel Model => _stateManager.Model;

	public UpdateAdapter([JetBrains.Annotations.NotNull] IStateManager stateManager)
	{
		_stateManager = stateManager;
		_changeDetector = _stateManager.Context.GetDependencies().ChangeDetector;
	}

	public virtual IUpdateEntry FindPrincipal(IUpdateEntry dependentEntry, IForeignKey foreignKey)
	{
		return _stateManager.FindPrincipal((InternalEntityEntry)dependentEntry, foreignKey);
	}

	public virtual IEnumerable<IUpdateEntry> GetDependents(IUpdateEntry principalEntry, IForeignKey foreignKey)
	{
		return _stateManager.GetDependents((InternalEntityEntry)principalEntry, foreignKey);
	}

	public virtual IUpdateEntry TryGetEntry(IKey key, object[] keyValues)
	{
		return _stateManager.TryGetEntry(key, keyValues);
	}

	public virtual void DetectChanges()
	{
		if ((string)_stateManager.Model["ChangeDetector.SkipDetectChanges"] != "true")
		{
			_changeDetector.DetectChanges(_stateManager);
		}
	}

	public virtual void CascadeChanges()
	{
		_stateManager.CascadeChanges(force: true);
	}

	public virtual void CascadeDelete(IUpdateEntry entry, IEnumerable<IForeignKey> foreignKeys = null)
	{
		_stateManager.CascadeDelete((InternalEntityEntry)entry, force: true, foreignKeys);
	}

	public virtual IList<IUpdateEntry> GetEntriesToSave()
	{
		return _stateManager.GetEntriesToSave(cascadeChanges: false);
	}

	public virtual IUpdateEntry CreateEntry(IDictionary<string, object> values, IEntityType entityType)
	{
		return _stateManager.CreateEntry(values, entityType);
	}
}
