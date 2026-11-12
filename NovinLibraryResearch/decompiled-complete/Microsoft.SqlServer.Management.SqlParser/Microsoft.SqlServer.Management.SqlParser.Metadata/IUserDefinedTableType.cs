namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("IsUnknown", false)]
[Duplicate("CollationInfo", "Schema.Database.CollationInfo")]
public interface IUserDefinedTableType : IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, ITableDataType, IDatabaseTable, ITabular
{
}
