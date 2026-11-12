using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IMetadataDisplayInfoProvider
{
	CasingStyle BuiltInCasing { get; set; }

	string GetDisplayName(IMetadataObject metadataObject);

	string GetDescription(IMetadataObject metadataObject);

	string GetDatabaseQualifiedName(IMetadataObject metadataObject);

	string ObjectToString(IMetadataObject metadataObject);

	string CollectionToString<T>(IMetadataCollection<T> metadataCollection, bool singleLine) where T : class, IMetadataObject;

	string CollectionToString<T>(IMetadataOrderedCollection<T> metadataCollection, bool singleLine) where T : class, IMetadataObject;
}
