using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SidecarValuesFactoryFactory : SnapshotFactoryFactory<InternalEntityEntry>
{
	protected override int GetPropertyIndex(IPropertyBase propertyBase)
	{
		return propertyBase.GetStoreGeneratedIndex();
	}

	protected override int GetPropertyCount(IEntityType entityType)
	{
		return entityType.StoreGeneratedCount();
	}

	protected override ValueComparer GetValueComparer(IProperty property)
	{
		ValueComparer valueComparer = property.GetValueComparer();
		if (valueComparer == null)
		{
			CoreTypeMapping coreTypeMapping = property.FindTypeMapping();
			if (coreTypeMapping == null)
			{
				return null;
			}
			valueComparer = coreTypeMapping.Comparer;
		}
		return valueComparer;
	}
}
