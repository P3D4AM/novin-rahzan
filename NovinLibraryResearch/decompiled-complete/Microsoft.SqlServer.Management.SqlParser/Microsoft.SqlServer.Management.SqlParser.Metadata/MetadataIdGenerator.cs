using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class MetadataIdGenerator : IMetadataIdGenerator<MetadataObjectId>
{
	private readonly Dictionary<IMetadataObject, MetadataObjectId> metadataIdMap;

	public MetadataIdGenerator()
	{
		metadataIdMap = new Dictionary<IMetadataObject, MetadataObjectId>(4096);
	}

	public virtual MetadataObjectSelector GetObjectSelector(IMetadataObject obj)
	{
		return new MetadataObjectSelector(obj.TypeInfo(), obj.Name);
	}

	protected bool IsRootObject(IMetadataObject obj)
	{
		return obj.TypeInfo().Id == 100;
	}

	protected virtual IMetadataObject GetParent(IMetadataObject obj)
	{
		return MetadataUtils.GetParentObject(obj);
	}

	protected virtual List<MetadataObjectSelector> GetObjectSelectors(IMetadataObject obj)
	{
		IMetadataObject obj2 = obj;
		List<MetadataObjectSelector> list = new List<MetadataObjectSelector>();
		while (!IsRootObject(obj2))
		{
			list.Add(GetObjectSelector(obj2));
			obj2 = GetParent(obj2);
		}
		list.Reverse();
		return list;
	}

	public MetadataObjectId GenerateId(IMetadataObject obj)
	{
		if (!metadataIdMap.TryGetValue(obj, out var value))
		{
			List<MetadataObjectSelector> objectSelectors = GetObjectSelectors(obj);
			value = new MetadataObjectId(objectSelectors);
			metadataIdMap.Add(obj, value);
		}
		return value;
	}
}
