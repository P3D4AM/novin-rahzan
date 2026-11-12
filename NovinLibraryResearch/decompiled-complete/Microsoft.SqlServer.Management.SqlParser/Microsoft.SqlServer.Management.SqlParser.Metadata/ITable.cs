namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("TabularType", TabularType.Table)]
public interface ITable : ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject
{
}
