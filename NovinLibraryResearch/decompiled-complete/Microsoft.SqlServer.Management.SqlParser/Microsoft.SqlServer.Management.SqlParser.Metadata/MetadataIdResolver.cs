using Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal class MetadataIdResolver
{
	private readonly IServer server;

	public MetadataIdResolver(IServer server)
	{
		this.server = server;
	}

	public IMetadataObject ResolveObject(MetadataObjectId id)
	{
		MetadataTypeInfo typeInfo = (MetadataTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(100);
		IMetadataObject childElement = server;
		foreach (MetadataObjectSelector selector in id.Selectors)
		{
			childElement = XmlConfig.GetChildElement(typeInfo, selector.TypeInfo, childElement, selector.Name);
			typeInfo = selector.TypeInfo;
			if (childElement == null)
			{
				break;
			}
		}
		return childElement;
	}
}
