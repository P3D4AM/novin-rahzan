namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableView : IView, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IMutableTableViewBase, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	new bool IsEncrypted { get; set; }

	new bool IsSchemaBound { get; set; }

	new string QueryText { get; set; }

	new bool ReturnsViewMetadata { get; set; }

	new bool HasCheckOption { get; set; }

	new bool HasColumnSpecification { get; set; }
}
