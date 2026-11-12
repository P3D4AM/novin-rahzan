using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class MetadataChildTypeComparer : PropertyComparer
{
	public MetadataChildTypeComparer(PropertyComparerFactory creator, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
		: base(creator, changeResultBuilder, differenceFilter)
	{
	}

	public override bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult)
	{
		MetadataTypeInfo typeInfo = sourceParent.TypeInfo();
		object propertyValue = MetadataUtils.GetPropertyValue(sourceParent, typeInfo, propertyInfo);
		object propertyValue2 = MetadataUtils.GetPropertyValue(targetParent, typeInfo, propertyInfo);
		IMetadataObject metadataObject = (IMetadataObject)propertyValue;
		IMetadataObject metadataObject2 = (IMetadataObject)propertyValue2;
		GetMetadataObjectId(metadataObject);
		GetMetadataObjectId(metadataObject2);
		if (CompareRegularMetadataObjects(metadataObject, metadataObject2))
		{
			if (metadataObject != null && !MetadataUtils.IsValueTypeObject(metadataObject))
			{
				ProcessNodes(metadataObject, metadataObject2, addToChangeResult);
			}
		}
		else
		{
			if (metadataObject != null)
			{
				base.ChangeResultBuilder.AddDeletedEntry(metadataObject);
			}
			if (metadataObject2 != null)
			{
				base.ChangeResultBuilder.AddCreatedEntry(metadataObject2);
			}
		}
		return true;
	}
}
