using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal class DatabaseRelativeIdGenerator : MetadataIdGenerator
{
	private IDatabaseObject sourceRoot;

	private IDatabaseObject targetRoot;

	public DatabaseRelativeIdGenerator(IDatabaseObject sourceRoot, IDatabaseObject targetRoot)
	{
		this.sourceRoot = sourceRoot;
		this.targetRoot = targetRoot;
	}

	protected override List<MetadataObjectSelector> GetObjectSelectors(IMetadataObject obj)
	{
		if (IsRootObject(obj))
		{
			return new List<MetadataObjectSelector>(0);
		}
		if (AreEqual(sourceRoot, obj) || AreEqual(targetRoot, obj))
		{
			return new List<MetadataObjectSelector>
			{
				new MetadataObjectSelector(MetadataIdConfig.RootTypeInfo, string.Empty)
			};
		}
		IMetadataObject parent = GetParent(obj);
		MetadataObjectId metadataObjectId = GenerateId(parent);
		List<MetadataObjectSelector> list = new List<MetadataObjectSelector>(metadataObjectId.Selectors.Count + 1);
		list.AddRange(metadataObjectId.Selectors);
		list.Add(GetObjectSelector(obj));
		return list;
	}

	private bool AreEqual(IMetadataObject obj1, IMetadataObject obj2)
	{
		IMetadataObject metadataObject = obj1;
		IMetadataObject metadataObject2 = obj2;
		if (metadataObject2 == null)
		{
			return false;
		}
		while (true)
		{
			if (metadataObject == metadataObject2)
			{
				return true;
			}
			TypeInfo typeInfo = metadataObject.TypeInfo();
			TypeInfo typeInfo2 = metadataObject2.TypeInfo();
			if (typeInfo != typeInfo2)
			{
				return false;
			}
			if (typeInfo.Id == 100)
			{
				return true;
			}
			if (metadataObject.Name != metadataObject2.Name)
			{
				break;
			}
			metadataObject = GetParent(metadataObject);
			metadataObject2 = GetParent(metadataObject2);
		}
		return false;
	}
}
