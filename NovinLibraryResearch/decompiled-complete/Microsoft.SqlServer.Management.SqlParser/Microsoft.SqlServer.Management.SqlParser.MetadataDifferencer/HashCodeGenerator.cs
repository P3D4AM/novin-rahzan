using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class HashCodeGenerator
{
	private const int MaxRecursionDepth = 3;

	private readonly MetadataIdGenerator metadataIdGenerator;

	private readonly IDifferencerFilter differencerFilter;

	public HashCodeGenerator(MetadataIdGenerator metadataIdGenerator, IDifferencerFilter differencerFilter)
	{
		this.metadataIdGenerator = metadataIdGenerator;
		this.differencerFilter = differencerFilter;
	}

	public int GetHashCode(IMetadataObject obj)
	{
		return GetObjectHashCode(obj, 3);
	}

	private int GetObjectHashCode(IMetadataObject obj, int depthLeft)
	{
		if (obj != null)
		{
			if (!MetadataUtils.IsValueTypeObject(obj))
			{
				return metadataIdGenerator.GenerateId(obj).GetHashCode();
			}
			if (depthLeft > 0)
			{
				return GetUnnamedObjectHashCode(obj, depthLeft);
			}
			return 13;
		}
		return 0;
	}

	private int GetUnnamedObjectHashCode(IMetadataObject obj, int depthLeft)
	{
		int num = 0;
		MetadataTypeInfo metadataTypeInfo = obj.TypeInfo();
		foreach (MetadataPropertyInfo item in metadataTypeInfo.InstanceProperties.Where(DifferencerUtils.IsPropertyTypeComparable))
		{
			if (!DifferencerUtils.IsPropertySkipped(obj, metadataTypeInfo, item))
			{
				object propertyValue = MetadataUtils.GetPropertyValue(obj, metadataTypeInfo, item);
				int propertyHashCode = GetPropertyHashCode(propertyValue, metadataTypeInfo, item, depthLeft - 1);
				num = MergeHashCode(num, propertyHashCode, symetrical: false);
			}
		}
		return num;
	}

	private int GetPropertyHashCode(object value, MetadataTypeInfo typeInfo, MetadataPropertyInfo instancePropertyInfo, int depthLeft)
	{
		IEqualityComparer equalityComparer;
		int num;
		if ((equalityComparer = differencerFilter.GetEqualityComparer(typeInfo, instancePropertyInfo)) != null)
		{
			num = equalityComparer.GetHashCode(value);
		}
		else if (value != null)
		{
			TypeInfo returnType = instancePropertyInfo.ReturnType;
			if (returnType.IsPrimitive)
			{
				num = GetPrimitiveHashCode(value);
			}
			else if (!returnType.IsCollection)
			{
				num = (returnType.IsMetadata ? GetObjectHashCode((IMetadataObject)value, depthLeft) : 0);
			}
			else
			{
				IEnumerable<IMetadataObject> enumerable = ((IEnumerable)value).Cast<IMetadataObject>();
				bool symetrical = !((CollectionTypeInfo)returnType).IsOrdered;
				num = 0;
				foreach (IMetadataObject item in enumerable)
				{
					num = MergeHashCode(num, GetObjectHashCode(item, depthLeft), symetrical);
				}
			}
		}
		else
		{
			num = 0;
		}
		return num;
	}

	private static int GetPrimitiveHashCode(object value)
	{
		return value?.GetHashCode() ?? 0;
	}

	private static int MergeHashCode(int a, int b, bool symetrical)
	{
		if (!symetrical)
		{
			return (a * 131) ^ b;
		}
		return a ^ b;
	}
}
