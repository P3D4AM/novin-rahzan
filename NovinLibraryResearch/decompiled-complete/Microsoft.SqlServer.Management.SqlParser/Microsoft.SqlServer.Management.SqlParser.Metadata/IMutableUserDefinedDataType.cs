namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public interface IMutableUserDefinedDataType : IUserDefinedDataType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IScalarDataType, IMutableUserDefinedType, IMutableDataType, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableScalarDataType
{
	new bool Nullable { get; set; }
}
