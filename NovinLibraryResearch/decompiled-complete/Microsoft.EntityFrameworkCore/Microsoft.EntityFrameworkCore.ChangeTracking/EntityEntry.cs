using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

[DebuggerDisplay("{InternalEntry,nq}")]
public class EntityEntry : IInfrastructure<InternalEntityEntry>
{
	private static readonly int _maxEntityState = Enum.GetValues(typeof(EntityState)).Cast<int>().Max();

	[EntityFrameworkInternal]
	protected virtual InternalEntityEntry InternalEntry
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual object Entity => InternalEntry.Entity;

	public virtual EntityState State
	{
		get
		{
			return InternalEntry.EntityState;
		}
		set
		{
			if (value < EntityState.Detached || (int)value > _maxEntityState)
			{
				throw new ArgumentException(CoreStrings.InvalidEnumValue("value", typeof(EntityState)));
			}
			InternalEntry.SetEntityState(value);
		}
	}

	[EntityFrameworkInternal]
	InternalEntityEntry IInfrastructure<InternalEntityEntry>.Instance => InternalEntry;

	public virtual DbContext Context => InternalEntry.StateManager.Context;

	public virtual IEntityType Metadata => InternalEntry.EntityType;

	public virtual IEnumerable<MemberEntry> Members => Properties.Cast<MemberEntry>().Concat(Navigations);

	public virtual IEnumerable<NavigationEntry> Navigations => from navigation in InternalEntry.EntityType.GetNavigations()
		select (!navigation.IsCollection()) ? ((NavigationEntry)new ReferenceEntry(InternalEntry, navigation)) : ((NavigationEntry)new CollectionEntry(InternalEntry, navigation));

	public virtual IEnumerable<PropertyEntry> Properties => from property in InternalEntry.EntityType.GetProperties()
		select new PropertyEntry(InternalEntry, property);

	public virtual IEnumerable<ReferenceEntry> References => from n in InternalEntry.EntityType.GetNavigations()
		where !n.IsCollection()
		select n into navigation
		select new ReferenceEntry(InternalEntry, navigation);

	public virtual IEnumerable<CollectionEntry> Collections => from n in InternalEntry.EntityType.GetNavigations()
		where n.IsCollection()
		select n into navigation
		select new CollectionEntry(InternalEntry, navigation);

	public virtual bool IsKeySet => InternalEntry.IsKeySet.IsSet;

	public virtual PropertyValues CurrentValues
	{
		[DebuggerStepThrough]
		get
		{
			return new CurrentPropertyValues(InternalEntry);
		}
	}

	public virtual PropertyValues OriginalValues
	{
		[DebuggerStepThrough]
		get
		{
			return new OriginalPropertyValues(InternalEntry);
		}
	}

	private IEntityFinder Finder => InternalEntry.StateManager.CreateEntityFinder(InternalEntry.EntityType);

	[EntityFrameworkInternal]
	public EntityEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(internalEntry, "internalEntry");
		InternalEntry = internalEntry;
	}

	public virtual void DetectChanges()
	{
		if ((string)Context.Model["ChangeDetector.SkipDetectChanges"] != "true")
		{
			Context.GetDependencies().ChangeDetector.DetectChanges(InternalEntry);
		}
	}

	public virtual MemberEntry Member([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		if (InternalEntry.EntityType.FindProperty(propertyName) != null)
		{
			return new PropertyEntry(InternalEntry, propertyName);
		}
		INavigation navigation = InternalEntry.EntityType.FindNavigation(propertyName);
		if (navigation != null)
		{
			if (!navigation.IsCollection())
			{
				return new ReferenceEntry(InternalEntry, propertyName);
			}
			return new CollectionEntry(InternalEntry, propertyName);
		}
		throw new InvalidOperationException(CoreStrings.PropertyNotFound(propertyName, InternalEntry.EntityType.DisplayName()));
	}

	public virtual NavigationEntry Navigation([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		INavigation navigation = InternalEntry.EntityType.FindNavigation(propertyName);
		if (navigation != null)
		{
			if (!navigation.IsCollection())
			{
				return new ReferenceEntry(InternalEntry, propertyName);
			}
			return new CollectionEntry(InternalEntry, propertyName);
		}
		if (InternalEntry.EntityType.FindProperty(propertyName) != null)
		{
			throw new InvalidOperationException(CoreStrings.NavigationIsProperty(propertyName, InternalEntry.EntityType.DisplayName(), "Reference", "Collection", "Property"));
		}
		throw new InvalidOperationException(CoreStrings.PropertyNotFound(propertyName, InternalEntry.EntityType.DisplayName()));
	}

	public virtual PropertyEntry Property([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		return new PropertyEntry(InternalEntry, propertyName);
	}

	public virtual ReferenceEntry Reference([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		return new ReferenceEntry(InternalEntry, propertyName);
	}

	public virtual CollectionEntry Collection([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		return new CollectionEntry(InternalEntry, propertyName);
	}

	public virtual PropertyValues GetDatabaseValues()
	{
		object[] databaseValues = Finder.GetDatabaseValues(InternalEntry);
		if (databaseValues != null)
		{
			return new ArrayPropertyValues(InternalEntry, databaseValues);
		}
		return null;
	}

	public virtual async Task<PropertyValues> GetDatabaseValuesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		object[] array = await Finder.GetDatabaseValuesAsync(InternalEntry, cancellationToken);
		return (array == null) ? null : new ArrayPropertyValues(InternalEntry, array);
	}

	public virtual void Reload()
	{
		Reload(GetDatabaseValues());
	}

	public virtual async Task ReloadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		Reload(await GetDatabaseValuesAsync(cancellationToken));
	}

	private void Reload(PropertyValues storeValues)
	{
		if (storeValues == null)
		{
			if (State != EntityState.Added)
			{
				State = EntityState.Deleted;
				State = EntityState.Detached;
			}
		}
		else
		{
			CurrentValues.SetValues(storeValues);
			OriginalValues.SetValues(storeValues);
			State = EntityState.Unchanged;
		}
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
public class EntityEntry<TEntity> : EntityEntry where TEntity : class
{
	public new virtual TEntity Entity => (TEntity)base.Entity;

	[EntityFrameworkInternal]
	public EntityEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry)
		: base(internalEntry)
	{
	}

	public virtual PropertyEntry<TEntity, TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TProperty>> propertyExpression)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression");
		return new PropertyEntry<TEntity, TProperty>(InternalEntry, propertyExpression.GetPropertyAccess().GetSimpleMemberName());
	}

	public virtual ReferenceEntry<TEntity, TProperty> Reference<TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, TProperty>> propertyExpression) where TProperty : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression");
		return new ReferenceEntry<TEntity, TProperty>(InternalEntry, propertyExpression.GetPropertyAccess().GetSimpleMemberName());
	}

	public virtual CollectionEntry<TEntity, TProperty> Collection<TProperty>([JetBrains.Annotations.NotNull] Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(propertyExpression, "propertyExpression");
		return new CollectionEntry<TEntity, TProperty>(InternalEntry, propertyExpression.GetPropertyAccess().GetSimpleMemberName());
	}

	public virtual ReferenceEntry<TEntity, TProperty> Reference<TProperty>([JetBrains.Annotations.NotNull] string propertyName) where TProperty : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		return new ReferenceEntry<TEntity, TProperty>(InternalEntry, propertyName);
	}

	public virtual CollectionEntry<TEntity, TProperty> Collection<TProperty>([JetBrains.Annotations.NotNull] string propertyName) where TProperty : class
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		return new CollectionEntry<TEntity, TProperty>(InternalEntry, propertyName);
	}

	public virtual PropertyEntry<TEntity, TProperty> Property<TProperty>([JetBrains.Annotations.NotNull] string propertyName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(propertyName, "propertyName");
		ValidateType<TProperty>(InternalEntry.EntityType.FindProperty(propertyName));
		return new PropertyEntry<TEntity, TProperty>(InternalEntry, propertyName);
	}

	private static void ValidateType<TProperty>(IProperty property)
	{
		if (property != null && property.ClrType != typeof(TProperty))
		{
			throw new ArgumentException(CoreStrings.WrongGenericPropertyType(property.Name, property.DeclaringEntityType.DisplayName(), property.ClrType.ShortDisplayName(), typeof(TProperty).ShortDisplayName()));
		}
	}
}
