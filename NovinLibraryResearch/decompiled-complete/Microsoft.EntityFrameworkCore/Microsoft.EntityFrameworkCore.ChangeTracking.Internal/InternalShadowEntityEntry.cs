using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalShadowEntityEntry : InternalEntityEntry
{
	private readonly ISnapshot _propertyValues;

	public override object Entity => null;

	public InternalShadowEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType)
		: base(stateManager, entityType)
	{
		_propertyValues = entityType.GetEmptyShadowValuesFactory()();
		MarkShadowPropertiesNotSet(entityType);
	}

	public InternalShadowEntityEntry([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IEntityType entityType, in ValueBuffer valueBuffer)
		: base(stateManager, entityType)
	{
		_propertyValues = ((EntityType)entityType).ShadowValuesFactory(valueBuffer);
	}

	protected override T ReadShadowValue<T>(int shadowIndex)
	{
		return _propertyValues.GetValue<T>(shadowIndex);
	}

	protected override object ReadPropertyValue(IPropertyBase propertyBase)
	{
		return _propertyValues[propertyBase.GetShadowIndex()];
	}

	protected override bool PropertyHasDefaultValue(IPropertyBase propertyBase)
	{
		return propertyBase.ClrType.IsDefaultValue(_propertyValues[propertyBase.GetShadowIndex()]);
	}

	protected override void WritePropertyValue(IPropertyBase propertyBase, object value, bool forMaterialization)
	{
		_propertyValues[propertyBase.GetShadowIndex()] = value;
	}

	public override object GetOrCreateCollection(INavigation navigation, bool forMaterialization)
	{
		return GetOrCreateCollectionTyped(navigation);
	}

	private ICollection<object> GetOrCreateCollectionTyped(INavigation navigation)
	{
		ICollection<object> collection = _propertyValues[navigation.GetShadowIndex()] as ICollection<object>;
		if (collection == null)
		{
			collection = new HashSet<object>();
			_propertyValues[navigation.GetShadowIndex()] = collection;
		}
		return collection;
	}

	public override bool CollectionContains(INavigation navigation, InternalEntityEntry value)
	{
		return GetOrCreateCollectionTyped(navigation).Contains(value.Entity);
	}

	public override bool AddToCollection(INavigation navigation, InternalEntityEntry value, bool forMaterialization)
	{
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
		return GetOrCreateCollectionTyped(navigation).Remove(value.Entity);
	}
}
