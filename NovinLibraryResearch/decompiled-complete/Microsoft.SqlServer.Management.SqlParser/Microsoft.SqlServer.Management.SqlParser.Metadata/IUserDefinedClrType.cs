namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata]
[Constant("BaseSystemDataType", null)]
[Constant("IsUnknown", false)]
[ReadWrite("AssemblyName")]
[ReadWrite("ClassName")]
[ReadWrite("IsBinaryOrdered")]
[ReadWrite("IsComVisible")]
[ReadWrite("IsNullable")]
[ReadWrite("Methods")]
[ReadWrite("DataMembers")]
public interface IUserDefinedClrType : IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IClrDataType, IScalarDataType
{
}
