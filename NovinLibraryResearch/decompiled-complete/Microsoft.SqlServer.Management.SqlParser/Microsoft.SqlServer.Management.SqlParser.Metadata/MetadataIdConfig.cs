namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

internal static class MetadataIdConfig
{
	private class Root
	{
	}

	private static MetadataTypeInfo rootTypeInfo = new MetadataTypeInfo(typeof(Root), -1, MetadataType.Abstract);

	public static MetadataTypeInfo RootTypeInfo => rootTypeInfo;
}
