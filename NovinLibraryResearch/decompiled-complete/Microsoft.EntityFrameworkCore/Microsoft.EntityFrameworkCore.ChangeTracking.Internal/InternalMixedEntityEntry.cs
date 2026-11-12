using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalMixedEntityEntry : InternalEntityEntry
{
	private readonly ISnapshot _shadowValues;

	public override object Entity { get; }

	public InternalMixedEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] object entity)
		: base(stateManager, entityType)
	{
		Entity = entity;
		_shadowValues = entityType.GetEmptyShadowValuesFactory()();
		MarkShadowPropertiesNotSet(entityType);
	}

	public InternalMixedEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] object entity, in ValueBuffer valueBuffer)
		: base(stateManager, entityType)
	{
		Entity = entity;
		_shadowValues = ((EntityType)entityType).ShadowValuesFactory(valueBuffer);
	}

	protected override T ReadShadowValue<T>(int shadowIndex)
	{
		return _shadowValues.GetValue<T>(shadowIndex);
	}

	protected override object ReadPropertyValue(IPropertyBase propertyBase)
	{
		if (propertyBase.IsShadowProperty())
		{
			return _shadowValues[propertyBase.GetShadowIndex()];
		}
		return base.ReadPropertyValue(propertyBase);
	}

	protected override bool PropertyHasDefaultValue(IPropertyBase propertyBase)
	{
		if (propertyBase.IsShadowProperty())
		{
			return propertyBase.ClrType.IsDefaultValue(_shadowValues[propertyBase.GetShadowIndex()]);
		}
		return base.PropertyHasDefaultValue(propertyBase);
	}

	protected override void WritePropertyValue(IPropertyBase propertyBase, object value, bool forMaterialization)
	{
		if (!propertyBase.IsShadowProperty())
		{
			base.WritePropertyValue(propertyBase, value, forMaterialization);
		}
		else
		{
			_shadowValues[propertyBase.GetShadowIndex()] = value;
		}
	}

	public override object GetOrCreateCollection(INavigation navigation, bool forMaterialization)
	{
		if (!navigation.IsShadowProperty())
		{
			return base.GetOrCreateCollection(navigation, forMaterialization);
		}
		return GetOrCreateCollectionTyped(navigation);
	}

	private ICollection<object> GetOrCreateCollectionTyped(INavigation navigation)
	{
		ICollection<object> collection = _shadowValues[navigation.GetShadowIndex()] as ICollection<object>;
		if (collection == null)
		{
			collection = new HashSet<object>();
			_shadowValues[navigation.GetShadowIndex()] = collection;
		}
		return collection;
	}

	public override bool CollectionContains(INavigation navigation, InternalEntityEntry value)
	{
		if (!navigation.IsShadowProperty())
		{
			return base.CollectionContains(navigation, value);
		}
		return GetOrCreateCollectionTyped(navigation).Contains(value.Entity);
	}

	public override bool AddToCollection(INavigation navigation, InternalEntityEntry value, bool forMaterialization)
	{
		if (!navigation.IsShadowProperty())
		{
			return base.AddToCollection(navigation, value, forMaterialization);
		}
		if (navigation.GetTargetType().ClrType == null)
		{
			return false;
		}
		ICollection<object> orCreateCollectionTyped = GetOrCreateCollectionTyped(navigation);
		if (!orCreateCollectionTyped.Contains(value.Entity))
		{
			orCreateCollectionTyped.Add(value.Entity);
			return true;
		}
		return false;
	}

	public override bool RemoveFromCollection(INavigation navigation, InternalEntityEntry value)
	{
		if (!navigation.IsShadowProperty())
		{
			return base.RemoveFromCollection(navigation, value);
		}
		return GetOrCreateCollectionTyped(navigation).Remove(value.Entity);
	}
}
