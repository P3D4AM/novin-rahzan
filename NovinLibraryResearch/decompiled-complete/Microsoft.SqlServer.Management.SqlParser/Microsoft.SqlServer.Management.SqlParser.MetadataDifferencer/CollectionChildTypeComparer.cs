using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class CollectionChildTypeComparer : PropertyComparer
{
	private class CollectionElement
	{
		private readonly int hashCode;

		private readonly MetadataObjectId key;

		private readonly IMetadataObject metadataObject;

		private readonly int indexId;

		public int HashCode => hashCode;

		public int IndexId => indexId;

		public MetadataObjectId Key => key;

		public IMetadataObject MetadataObject => metadataObject;

		public bool IsMarked { get; set; }

		public CollectionElement(int hashCode, MetadataObjectId key, IMetadataObject metadataObject, int indexId)
		{
			this.hashCode = hashCode;
			this.key = key;
			this.metadataObject = metadataObject;
			this.indexId = indexId;
		}
	}

	public CollectionChildTypeComparer(PropertyComparerFactory creator, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
		: base(creator, changeResultBuilder, differenceFilter)
	{
	}

	public override bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult)
	{
		MetadataTypeInfo typeInfo = sourceParent.TypeInfo();
		object propertyValue = MetadataUtils.GetPropertyValue(sourceParent, typeInfo, propertyInfo);
		object propertyValue2 = MetadataUtils.GetPropertyValue(targetParent, typeInfo, propertyInfo);
		IEnumerable source = (IEnumerable)propertyValue;
		IEnumerable source2 = (IEnumerable)propertyValue2;
		List<CollectionElement> elements = GetElements(source.Cast<IMetadataObject>(), sourceParent, propertyInfo);
		List<CollectionElement> elements2 = GetElements(source2.Cast<IMetadataObject>(), targetParent, propertyInfo);
		elements.Sort(CompareCollectionElements);
		elements2.Sort(CompareCollectionElements);
		bool flag = true;
		bool flag2 = PropertyComparer.IsOrderedCollection(propertyInfo);
		bool flag3 = propertyInfo.PropertyType == PropertyType.Child;
		int num = 0;
		int num2 = 0;
		while (num < elements.Count && num2 < elements2.Count)
		{
			CollectionElement right = ((CompareCollectionElements(elements[num], elements2[num2]) <= 0) ? elements[num] : elements2[num2]);
			int i;
			for (i = num; i < elements.Count && CompareCollectionElements(elements[i], right) == 0; i++)
			{
			}
			int j;
			for (j = num2; j < elements2.Count && CompareCollectionElements(elements2[j], right) == 0; j++)
			{
			}
			for (int k = num; k < i; k++)
			{
				CollectionElement collectionElement = elements[k];
				for (int l = num2; l < j; l++)
				{
					if (elements2[l].IsMarked)
					{
						continue;
					}
					CollectionElement collectionElement2 = elements2[l];
					if (!CompareRegularMetadataObjects(collectionElement.MetadataObject, collectionElement2.MetadataObject))
					{
						continue;
					}
					if (flag2 && collectionElement.IndexId != collectionElement2.IndexId)
					{
						flag = false;
						if (addToChangeResult && flag3)
						{
							base.ChangeResultBuilder.AddOrderedScalarDifferenceEntry(sourceParent, targetParent, new OrderedScalarDifference(propertyInfo.Name, collectionElement.MetadataObject, collectionElement.IndexId, collectionElement2.MetadataObject, collectionElement2.IndexId));
						}
					}
					if (flag3 && !MetadataUtils.IsValueTypeObject(collectionElement.MetadataObject))
					{
						ProcessNodes(collectionElement.MetadataObject, collectionElement2.MetadataObject, addToChangeResult);
					}
					collectionElement.IsMarked = true;
					collectionElement2.IsMarked = true;
					break;
				}
			}
			num = i;
			num2 = j;
		}
		for (int m = 0; m < elements.Count; m++)
		{
			if (!elements[m].IsMarked && !IsDeletedObjectIgnored(elements[m].MetadataObject))
			{
				flag = false;
				if (addToChangeResult && flag3)
				{
					base.ChangeResultBuilder.AddDeletedEntry(elements[m].MetadataObject);
				}
			}
		}
		for (int n = 0; n < elements2.Count; n++)
		{
			if (!elements2[n].IsMarked && !IsCreatedObjectIgnored(elements2[n].MetadataObject))
			{
				flag = false;
				if (addToChangeResult && flag3)
				{
					base.ChangeResultBuilder.AddCreatedEntry(elements2[n].MetadataObject);
				}
			}
		}
		if (addToChangeResult && !flag3 && !flag)
		{
			base.ChangeResultBuilder.AddModifiedEntry(sourceParent, targetParent, new ScalarDifference(propertyInfo.Name, propertyValue, propertyValue2));
		}
		return flag;
	}

	private bool IsDeletedObjectIgnored(IMetadataObject metadataObj)
	{
		bool result = false;
		if (metadataObj is IDatabasePermission databasePermission && MetadataUtils.IsSystemObject(databasePermission.TargetObject))
		{
			MetadataObjectId id = base.PropertyComparerFactory.MetadataIdGenerator.GenerateId(databasePermission.TargetObject);
			result = !base.PropertyComparerFactory.TargetIdResolver.ObjectExists(id);
		}
		return result;
	}

	private bool IsCreatedObjectIgnored(IMetadataObject metadataObj)
	{
		bool result = false;
		if (metadataObj is IDatabasePermission databasePermission && MetadataUtils.IsSystemObject(databasePermission.TargetObject))
		{
			MetadataObjectId id = base.PropertyComparerFactory.MetadataIdGenerator.GenerateId(databasePermission.TargetObject);
			result = !base.PropertyComparerFactory.SourceIdResolver.ObjectExists(id);
		}
		return result;
	}

	private List<CollectionElement> GetElements(IEnumerable<IMetadataObject> collection, IMetadataObject parent, MetadataPropertyInfo propertyInfo)
	{
		List<CollectionElement> list = new List<CollectionElement>();
		int num = 0;
		foreach (IMetadataObject item in collection)
		{
			if (!MetadataUtils.IsSystemObject(item))
			{
				MetadataObjectId metadataObjectId = GetMetadataObjectId(item);
				int hashCode = base.PropertyComparerFactory.HashCodeGenerator.GetHashCode(item);
				list.Add(new CollectionElement(hashCode, metadataObjectId, item, num++));
			}
		}
		return list;
	}

	private static int CompareCollectionElements(CollectionElement left, CollectionElement right)
	{
		int num = left.HashCode.CompareTo(right.HashCode);
		if (num != 0)
		{
			return num;
		}
		return CompareObjectIds(left.Key, right.Key);
	}

	private static int CompareObjectIds(MetadataObjectId left, MetadataObjectId right)
	{
		int num = 0;
		for (int i = 0; i < left.Selectors.Count && i < right.Selectors.Count; i++)
		{
			if (i >= left.Selectors.Count)
			{
				num = -1;
				break;
			}
			if (i >= right.Selectors.Count)
			{
				num = 1;
				break;
			}
			num = left.Selectors[i].TypeInfo.Id.CompareTo(right.Selectors[i].TypeInfo.Id);
			if (num != 0)
			{
				break;
			}
			num = string.Compare(left.Selectors[i].Name, right.Selectors[i].Name, StringComparison.Ordinal);
			if (num != 0)
			{
				break;
			}
		}
		return num;
	}
}
