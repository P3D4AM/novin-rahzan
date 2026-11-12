namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableTableViewBase : ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	new IMutableMetadataCollection<IDmlTrigger> Triggers { get; }

	new bool IsQuotedIdentifierOn { get; set; }
}
