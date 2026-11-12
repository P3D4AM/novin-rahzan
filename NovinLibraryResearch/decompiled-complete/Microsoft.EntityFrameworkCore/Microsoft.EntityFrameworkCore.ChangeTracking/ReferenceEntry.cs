using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class ReferenceEntry : NavigationEntry
{
	public virtual EntityEntry TargetEntry
	{
		get
		{
			InternalEntityEntry targetEntry = GetTargetEntry();
			if (targetEntry != null)
			{
				return new EntityEntry(targetEntry);
			}
			return null;
		}
	}

	[EntityFrameworkInternal]
	public ReferenceEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: base(internalEntry, name, collection: false)
	{
		LocalDetectChanges();
	}

	[EntityFrameworkInternal]
	public ReferenceEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(internalEntry, navigation)
	{
		LocalDetectChanges();
	}

	private void LocalDetectChanges()
	{
		if (Metadata.IsDependentToPrincipal())
		{
			return;
		}
		InternalEntityEntry targetEntry = GetTargetEntry();
		if (targetEntry != null)
		{
			DbContext context = InternalEntry.StateManager.Context;
			if (context.ChangeTracker.AutoDetectChangesEnabled && (string)context.Model["ChangeDetector.SkipDetectChanges"] != "true")
			{
				context.GetDependencies().ChangeDetector.DetectChanges(targetEntry);
			}
		}
	}

	[EntityFrameworkInternal]
	protected virtual InternalEntityEntry GetTargetEntry()
	{
		if (CurrentValue != null)
		{
			return InternalEntry.StateManager.GetOrCreateEntry(CurrentValue, Metadata.GetTargetType());
		}
		return null;
	}
}
public class ReferenceEntry<TEntity, TProperty> : ReferenceEntry where TEntity : class where TProperty : class
{
	public new virtual EntityEntry<TEntity> EntityEntry => new EntityEntry<TEntity>(InternalEntry);

	public new virtual EntityEntry<TProperty> TargetEntry
	{
		get
		{
			InternalEntityEntry targetEntry = GetTargetEntry();
			if (targetEntry != null)
			{
				return new EntityEntry<TProperty>(targetEntry);
			}
			return null;
		}
	}

	public new virtual TProperty CurrentValue
	{
		get
		{
			return this.GetInfrastructure().GetCurrentValue<TProperty>(Metadata);
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			base.CurrentValue = value;
		}
	}

	[EntityFrameworkInternal]
	public ReferenceEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: base(internalEntry, name)
	{
	}

	[EntityFrameworkInternal]
	public ReferenceEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(internalEntry, navigation)
	{
	}

	public new virtual IQueryable<TProperty> Query()
	{
		return (IQueryable<TProperty>)base.Query();
	}
}
