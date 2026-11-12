using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class DifferencerIdResolver
{
	private readonly IServer server;

	private readonly IDatabaseObject root;

	public DifferencerIdResolver(IDatabaseObject root)
	{
		server = GetServer(root);
		this.root = root;
	}

	public IMetadataObject ResolveObject(MetadataObjectId id)
	{
		IMetadataObject childElement;
		int i;
		if (id.Selectors[0].TypeInfo == MetadataIdConfig.RootTypeInfo)
		{
			childElement = root;
			i = 1;
		}
		else
		{
			childElement = server;
			i = 0;
		}
		MetadataTypeInfo typeInfo = childElement.TypeInfo();
		for (; i < id.Selectors.Count; i++)
		{
			MetadataObjectSelector metadataObjectSelector = id.Selectors[i];
			childElement = XmlConfig.GetChildElement(typeInfo, metadataObjectSelector.TypeInfo, childElement, metadataObjectSelector.Name);
			typeInfo = metadataObjectSelector.TypeInfo;
			if (childElement == null)
			{
				break;
			}
		}
		return childElement;
	}

	public bool ObjectExists(MetadataObjectId id)
	{
		return ResolveObject(id) != null;
	}

	private static IServer GetServer(IDatabaseObject databaseObject)
	{
		while (databaseObject != null)
		{
			if (databaseObject is IServer result)
			{
				return result;
			}
			databaseObject = databaseObject.Parent;
		}
		return null;
	}
}
