using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class CollectionRegularTypeComparer : PropertyComparer
{
	public CollectionRegularTypeComparer(PropertyComparerFactory creator, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
		: base(creator, changeResultBuilder, differenceFilter)
	{
	}

	public override bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult)
	{
		MetadataTypeInfo typeInfo = sourceParent.TypeInfo();
		object propertyValue = MetadataUtils.GetPropertyValue(sourceParent, typeInfo, propertyInfo);
		object propertyValue2 = MetadataUtils.GetPropertyValue(targetParent, typeInfo, propertyInfo);
		IEnumerable<IMetadataObject> enumerable = ((IEnumerable)propertyValue).Cast<IMetadataObject>();
		IEnumerable<IMetadataObject> enumerable2 = ((IEnumerable)propertyValue2).Cast<IMetadataObject>();
		bool flag = true;
		using (IEnumerator<IMetadataObject> enumerator = enumerable.GetEnumerator())
		{
			using IEnumerator<IMetadataObject> enumerator2 = enumerable2.GetEnumerator();
			while (true)
			{
				bool flag2 = enumerator.MoveNext();
				bool flag3 = enumerator2.MoveNext();
				if (!flag2 && !flag3)
				{
					break;
				}
				if (flag2 != flag3)
				{
					flag = false;
					break;
				}
				if (!CompareRegularMetadataObjects(enumerator.Current, enumerator2.Current))
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag && addToChangeResult)
		{
			base.ChangeResultBuilder.AddModifiedEntry(sourceParent, targetParent, new ScalarDifference(propertyInfo.Name, propertyValue, propertyValue2));
		}
		return flag;
	}
}
