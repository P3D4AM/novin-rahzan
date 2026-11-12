using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public static class ExtensionMethods
{
	private static readonly MetadataDisplayInfoProvider displayInfoProvider;

	public static IMetadataDisplayInfoProvider DisplayInfoProvider => displayInfoProvider;

	static ExtensionMethods()
	{
		displayInfoProvider = new MetadataDisplayInfoProvider();
	}

	public static string GetDisplayName(IMetadataObject metadataObject)
	{
		return displayInfoProvider.GetDisplayName(metadataObject);
	}

	public static string GetDatabaseQualifiedName(IMetadataObject metadataObject)
	{
		return displayInfoProvider.GetDatabaseQualifiedName(metadataObject);
	}

	public static string GetDescription(IMetadataObject metadataObject)
	{
		return displayInfoProvider.GetDescription(metadataObject);
	}

	public static string ToString(IMetadataObject metadataObject)
	{
		return displayInfoProvider.ObjectToString(metadataObject);
	}
}
