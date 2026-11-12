using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class RelationshipSnapshotFactoryFactory : SnapshotFactoryFactory<InternalEntityEntry>
{
	protected override int GetPropertyIndex(IPropertyBase propertyBase)
	{
		return propertyBase.GetRelationshipIndex();
	}

	protected override int GetPropertyCount(IEntityType entityType)
	{
		return entityType.RelationshipPropertyCount();
	}

	protected override ValueComparer GetValueComparer(IProperty property)
	{
		ValueComparer valueComparer = property.GetKeyValueComparer();
		if (valueComparer == null)
		{
			CoreTypeMapping coreTypeMapping = property.FindTypeMapping();
			if (coreTypeMapping == null)
			{
				return null;
			}
			valueComparer = coreTypeMapping.KeyComparer;
		}
		return valueComparer;
	}
}
