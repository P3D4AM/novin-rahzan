using System.Collections;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal abstract class PropertyComparer
{
	private readonly ChangeResultBuilder changeResultBuilder;

	private readonly PropertyComparerFactory propertyComparerFactory;

	private readonly IDifferencerFilter differenceFilter;

	public ChangeResultBuilder ChangeResultBuilder => changeResultBuilder;

	protected PropertyComparerFactory PropertyComparerFactory => propertyComparerFactory;

	public IDifferencerFilter DifferenceFilter => differenceFilter;

	public UnnamedObjectsIdentityAssumptions EqualityAssumptions => PropertyComparerFactory.EqualityAssumptions;

	protected PropertyComparer(PropertyComparerFactory propertyComparerFactory, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
	{
		this.propertyComparerFactory = propertyComparerFactory;
		this.changeResultBuilder = changeResultBuilder;
		this.differenceFilter = differenceFilter;
	}

	public abstract bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult);

	public bool ProcessNodes(IMetadataObject source, IMetadataObject target, bool addToChangeResult)
	{
		bool flag = true;
		MetadataTypeInfo metadataTypeInfo = source.TypeInfo();
		foreach (MetadataPropertyInfo item in metadataTypeInfo.InstanceProperties.Where(DifferencerUtils.IsPropertyTypeComparable))
		{
			if (DifferencerUtils.IsPropertySkipped(source, metadataTypeInfo, item) && DifferencerUtils.IsPropertySkipped(target, metadataTypeInfo, item))
			{
				continue;
			}
			IEqualityComparer equalityComparer;
			bool flag2;
			if ((equalityComparer = DifferenceFilter.GetEqualityComparer(metadataTypeInfo, item)) != null)
			{
				object propertyValue = MetadataUtils.GetPropertyValue(source, metadataTypeInfo, item);
				object propertyValue2 = MetadataUtils.GetPropertyValue(target, metadataTypeInfo, item);
				flag2 = equalityComparer.Equals(propertyValue, propertyValue2);
				if (!flag2 && addToChangeResult)
				{
					changeResultBuilder.AddModifiedEntry(source, target, new ScalarDifference(item.Name, propertyValue, propertyValue2));
				}
			}
			else
			{
				flag2 = PropertyComparerFactory.Create(item).Process(source, target, item, addToChangeResult);
			}
			flag = flag && flag2;
		}
		return flag;
	}

	protected MetadataObjectId GetMetadataObjectId(IMetadataObject metadataObject)
	{
		MetadataIdGenerator metadataIdGenerator = propertyComparerFactory.MetadataIdGenerator;
		MetadataTypeInfo metadataTypeInfo = metadataObject.TypeInfo();
		if (metadataTypeInfo == null || metadataTypeInfo.MetadataType != 0)
		{
			return default(MetadataObjectId);
		}
		return metadataIdGenerator.GenerateId(metadataObject);
	}

	public static bool IsOrderedCollection(MetadataPropertyInfo propertyInfo)
	{
		return ((CollectionTypeInfo)propertyInfo.ReturnType).IsOrdered;
	}

	protected bool CompareRegularMetadataObjects(IMetadataObject sourceObject, IMetadataObject targetObject)
	{
		MetadataTypeInfo metadataTypeInfo = sourceObject.TypeInfo();
		MetadataTypeInfo metadataTypeInfo2 = targetObject.TypeInfo();
		bool result;
		if (sourceObject == null && targetObject == null)
		{
			result = true;
		}
		else if (metadataTypeInfo == metadataTypeInfo2)
		{
			bool flag = MetadataUtils.IsValueTypeObject(sourceObject);
			bool flag2 = MetadataUtils.IsValueTypeObject(targetObject);
			MetadataObjectId metadataObjectId = GetMetadataObjectId(sourceObject);
			MetadataObjectId metadataObjectId2 = GetMetadataObjectId(targetObject);
			if (!(flag && flag2))
			{
				result = !flag && !flag2 && metadataObjectId == metadataObjectId2;
			}
			else
			{
				bool flag3 = EqualityAssumptions.AreAssumedEqual(sourceObject, targetObject);
				bool flag4 = metadataObjectId == metadataObjectId2 && EqualityAssumptions.CanBeAssumedEqual(sourceObject, targetObject);
				if (!flag3 && flag4)
				{
					EqualityAssumptions.AddEqualityAssumption(sourceObject, targetObject);
					result = ProcessNodes(sourceObject, targetObject, addToChangeResult: false);
					EqualityAssumptions.RemoveEqualityAssumption(sourceObject, targetObject);
				}
				else
				{
					result = flag3;
				}
			}
		}
		else
		{
			result = false;
		}
		return result;
	}
}
