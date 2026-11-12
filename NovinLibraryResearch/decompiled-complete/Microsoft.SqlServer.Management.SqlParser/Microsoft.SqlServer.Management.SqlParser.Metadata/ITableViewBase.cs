namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[Duplicate("CollationInfo", "Schema.Database.CollationInfo")]
public interface ITableViewBase : IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject
{
	[ReadWrite]
	[Child]
	IMetadataCollection<IDmlTrigger> Triggers { get; }

	[ReadWrite]
	bool IsQuotedIdentifierOn { get; }
}
