using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class MetadataRegularTypeComparer : PropertyComparer
{
	public MetadataRegularTypeComparer(PropertyComparerFactory creator, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
		: base(creator, changeResultBuilder, differenceFilter)
	{
	}

	public override bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult)
	{
		MetadataTypeInfo typeInfo = sourceParent.TypeInfo();
		object propertyValue = MetadataUtils.GetPropertyValue(sourceParent, typeInfo, propertyInfo);
		object propertyValue2 = MetadataUtils.GetPropertyValue(targetParent, typeInfo, propertyInfo);
		IMetadataObject sourceObject = (IMetadataObject)propertyValue;
		IMetadataObject targetObject = (IMetadataObject)propertyValue2;
		bool num = CompareRegularMetadataObjects(sourceObject, targetObject);
		if (!num && addToChangeResult)
		{
			base.ChangeResultBuilder.AddModifiedEntry(sourceParent, targetParent, new ScalarDifference(propertyInfo.Name, propertyValue, propertyValue2));
		}
		return num;
	}
}
