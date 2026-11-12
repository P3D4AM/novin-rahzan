namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("BaseSystemDataType", SpecialValue.This)]
[Constant("IsUnknown", false)]
[Constant("AssemblyName", null)]
[Constant("ClassName", null)]
[Constant("IsBinaryOrdered", false)]
[Constant("IsComVisible", false)]
[Constant("IsNullable", false)]
[Constant("Methods", null)]
[Constant("DataMembers", null)]
public interface ISystemClrDataType : ISystemDataType, IScalarDataType, IDataType, IMetadataObject, IClrDataType
{
}
