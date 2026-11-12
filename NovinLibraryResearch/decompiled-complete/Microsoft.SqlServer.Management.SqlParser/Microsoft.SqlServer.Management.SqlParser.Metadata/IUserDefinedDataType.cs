namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[ReadWrite("BaseSystemDataType")]
[Reference("BaseSystemDataType")]
[Constant("IsUnknown", false)]
public interface IUserDefinedDataType : IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IScalarDataType
{
	[ReadWrite]
	bool Nullable { get; }
}
