using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public class CollectionEntry : NavigationEntry
{
	public new virtual IEnumerable CurrentValue
	{
		get
		{
			return (IEnumerable)base.CurrentValue;
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			base.CurrentValue = value;
		}
	}

	[EntityFrameworkInternal]
	public CollectionEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: base(internalEntry, name, collection: true)
	{
		LocalDetectChanges();
	}

	[EntityFrameworkInternal]
	public CollectionEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(internalEntry, navigation)
	{
		LocalDetectChanges();
	}

	private void LocalDetectChanges()
	{
		IEnumerable currentValue = CurrentValue;
		if (currentValue == null)
		{
			return;
		}
		IEntityType targetType = Metadata.GetTargetType();
		DbContext context = InternalEntry.StateManager.Context;
		IChangeDetector changeDetector = ((context.ChangeTracker.AutoDetectChangesEnabled && (string)context.Model["ChangeDetector.SkipDetectChanges"] != "true") ? context.GetDependencies().ChangeDetector : null);
		foreach (object item in currentValue.OfType<object>().ToList())
		{
			InternalEntityEntry orCreateEntry = InternalEntry.StateManager.GetOrCreateEntry(item, targetType);
			changeDetector?.DetectChanges(orCreateEntry);
		}
	}

	public override void Load()
	{
		EnsureInitialized();
		base.Load();
	}

	public override Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		EnsureInitialized();
		return base.LoadAsync(cancellationToken);
	}

	public override IQueryable Query()
	{
		EnsureInitialized();
		return base.Query();
	}

	private void EnsureInitialized()
	{
		Metadata.AsNavigation("EnsureInitialized").CollectionAccessor.GetOrCreate(InternalEntry.Entity, forMaterialization: true);
	}

	public virtual EntityEntry FindEntry([JetBrains.Annotations.NotNull] object entity)
	{
		InternalEntityEntry internalTargetEntry = GetInternalTargetEntry(entity);
		if (internalTargetEntry != null)
		{
			return new EntityEntry(internalTargetEntry);
		}
		return null;
	}

	protected virtual InternalEntityEntry GetInternalTargetEntry([JetBrains.Annotations.NotNull] object entity)
	{
		if (CurrentValue != null && ((Navigation)Metadata).CollectionAccessor.Contains(InternalEntry.Entity, entity))
		{
			return InternalEntry.StateManager.GetOrCreateEntry(entity, Metadata.GetTargetType());
		}
		return null;
	}
}
public class CollectionEntry<TEntity, TRelatedEntity> : CollectionEntry where TEntity : class where TRelatedEntity : class
{
	public new virtual EntityEntry<TEntity> EntityEntry => new EntityEntry<TEntity>(InternalEntry);

	public new virtual IEnumerable<TRelatedEntity> CurrentValue
	{
		get
		{
			return this.GetInfrastructure().GetCurrentValue<IEnumerable<TRelatedEntity>>(Metadata);
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			base.CurrentValue = value;
		}
	}

	[EntityFrameworkInternal]
	public CollectionEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name)
		: base(internalEntry, name)
	{
	}

	[EntityFrameworkInternal]
	public CollectionEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(internalEntry, navigation)
	{
	}

	public new virtual IQueryable<TRelatedEntity> Query()
	{
		InternalEntry.GetOrCreateCollection(Metadata, forMaterialization: true);
		return (IQueryable<TRelatedEntity>)base.Query();
	}

	public new virtual EntityEntry<TRelatedEntity> FindEntry([JetBrains.Annotations.NotNull] object entity)
	{
		InternalEntityEntry internalTargetEntry = GetInternalTargetEntry(entity);
		if (internalTargetEntry != null)
		{
			return new EntityEntry<TRelatedEntity>(internalTargetEntry);
		}
		return null;
	}
}
