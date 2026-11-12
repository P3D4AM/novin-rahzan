using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class SerializationIdGenerator : MetadataIdGenerator
{
	private readonly Dictionary<IMetadataObject, IMetadataObject> parentMap;

	private readonly Func<IMetadataObject, MetadataObjectSelector> objectSelector;

	public SerializationIdGenerator(Dictionary<IMetadataObject, IMetadataObject> parentMap, Func<IMetadataObject, MetadataObjectSelector> objectSelector)
	{
		this.parentMap = parentMap;
		this.objectSelector = objectSelector;
	}

	public override MetadataObjectSelector GetObjectSelector(IMetadataObject obj)
	{
		if (objectSelector == null)
		{
			return base.GetObjectSelector(obj);
		}
		return objectSelector(obj);
	}

	protected override IMetadataObject GetParent(IMetadataObject obj)
	{
		if (!parentMap.TryGetValue(obj, out var value))
		{
			return base.GetParent(obj);
		}
		return value;
	}
}
