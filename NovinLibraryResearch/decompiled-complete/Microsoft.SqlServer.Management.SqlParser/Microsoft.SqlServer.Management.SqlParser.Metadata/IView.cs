namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("TabularType", TabularType.View)]
public interface IView : ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject
{
	[ReadWrite]
	bool IsEncrypted { get; }

	[ReadWrite]
	bool IsSchemaBound { get; }

	[ReadWrite]
	string QueryText { get; }

	[ReadWrite]
	bool ReturnsViewMetadata { get; }

	[ReadWrite]
	bool HasCheckOption { get; }

	[ReadWrite]
	bool HasColumnSpecification { get; }
}
