using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking;

public abstract class NavigationEntry : MemberEntry
{
	public virtual bool IsLoaded
	{
		get
		{
			return InternalEntry.IsLoaded(Metadata);
		}
		set
		{
			InternalEntry.SetIsLoaded(Metadata, value);
		}
	}

	private IEntityFinder TargetFinder => InternalEntry.StateManager.CreateEntityFinder(Metadata.GetTargetType());

	public override bool IsModified
	{
		get
		{
			if (Metadata.IsDependentToPrincipal())
			{
				return AnyFkPropertiesModified(InternalEntry);
			}
			object currentValue = CurrentValue;
			if (currentValue != null)
			{
				if (!Metadata.IsCollection())
				{
					return AnyFkPropertiesModified(currentValue);
				}
				return ((IEnumerable)currentValue).OfType<object>().Any(CollectionContainsNewOrChangedRelationships);
			}
			return false;
		}
		set
		{
			if (Metadata.IsDependentToPrincipal())
			{
				SetFkPropertiesModified(InternalEntry, value);
				return;
			}
			object currentValue = CurrentValue;
			if (currentValue == null)
			{
				return;
			}
			if (Metadata.IsCollection())
			{
				foreach (object item in (IEnumerable)currentValue)
				{
					SetFkPropertiesModified(item, value);
				}
				return;
			}
			SetFkPropertiesModified(currentValue, value);
		}
	}

	public new virtual INavigation Metadata => (INavigation)base.Metadata;

	[EntityFrameworkInternal]
	protected NavigationEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] string name, bool collection)
		: this(internalEntry, GetNavigation(internalEntry, name, collection))
	{
	}

	[EntityFrameworkInternal]
	protected NavigationEntry([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntry, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(internalEntry, navigation)
	{
	}

	private static INavigation GetNavigation(InternalEntityEntry internalEntry, string name, bool collection)
	{
		INavigation navigation = internalEntry.EntityType.FindNavigation(name);
		if (navigation == null)
		{
			if (internalEntry.EntityType.FindProperty(name) != null)
			{
				throw new InvalidOperationException(CoreStrings.NavigationIsProperty(name, internalEntry.EntityType.DisplayName(), "Reference", "Collection", "Property"));
			}
			throw new InvalidOperationException(CoreStrings.PropertyNotFound(name, internalEntry.EntityType.DisplayName()));
		}
		if (collection && !navigation.IsCollection())
		{
			throw new InvalidOperationException(CoreStrings.CollectionIsReference(name, internalEntry.EntityType.DisplayName(), "Collection", "Reference"));
		}
		if (!collection && navigation.IsCollection())
		{
			throw new InvalidOperationException(CoreStrings.ReferenceIsCollection(name, internalEntry.EntityType.DisplayName(), "Reference", "Collection"));
		}
		return navigation;
	}

	public virtual void Load()
	{
		if (!IsLoaded)
		{
			TargetFinder.Load(Metadata, InternalEntry);
		}
	}

	public virtual Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!IsLoaded)
		{
			return TargetFinder.LoadAsync(Metadata, InternalEntry, cancellationToken);
		}
		return Task.FromResult(0);
	}

	public virtual IQueryable Query()
	{
		return TargetFinder.Query(Metadata, InternalEntry);
	}

	private bool CollectionContainsNewOrChangedRelationships(object relatedEntity)
	{
		InternalEntityEntry internalEntityEntry = InternalEntry.StateManager.TryGetEntry(relatedEntity, Metadata.GetTargetType());
		if (internalEntityEntry != null)
		{
			if (internalEntityEntry.EntityState != EntityState.Added && internalEntityEntry.EntityState != EntityState.Deleted)
			{
				return Metadata.ForeignKey.Properties.Any(internalEntityEntry.IsModified);
			}
			return true;
		}
		return false;
	}

	private bool AnyFkPropertiesModified(object relatedEntity)
	{
		InternalEntityEntry internalEntityEntry = InternalEntry.StateManager.TryGetEntry(relatedEntity, Metadata.GetTargetType());
		if (internalEntityEntry != null)
		{
			return Metadata.ForeignKey.Properties.Any(internalEntityEntry.IsModified);
		}
		return false;
	}

	private void SetFkPropertiesModified(object relatedEntity, bool modified)
	{
		InternalEntityEntry internalEntityEntry = InternalEntry.StateManager.TryGetEntry(relatedEntity, Metadata.GetTargetType());
		if (internalEntityEntry != null)
		{
			SetFkPropertiesModified(internalEntityEntry, modified);
		}
	}

	private void SetFkPropertiesModified(InternalEntityEntry internalEntityEntry, bool modified)
	{
		bool flag = Metadata.ForeignKey.Properties.Any((IProperty p) => !p.IsPrimaryKey());
		foreach (IProperty property in Metadata.ForeignKey.Properties)
		{
			if (flag && !property.IsPrimaryKey())
			{
				internalEntityEntry.SetPropertyModified(property, changeState: true, modified, isConceptualNull: false, acceptChanges: true);
			}
		}
	}

	private bool AnyFkPropertiesModified(InternalEntityEntry internalEntityEntry)
	{
		return Metadata.ForeignKey.Properties.Any(internalEntityEntry.IsModified);
	}
}
