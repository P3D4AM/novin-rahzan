namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Constant("BaseSystemDataType", SpecialValue.This)]
[Constant("IsUnknown", false)]
public interface IXmlDataType : ISystemDataType, IScalarDataType, IDataType, IMetadataObject
{
	[ReadOnly]
	bool IsXmlNode { get; }

	[Constant(SpecialValue.XmlMethods)]
	IMetadataCollection<IXmlDataTypeMethod> Methods { get; }

	[Constant(SpecialValue.XmlTableMethods)]
	IMetadataCollection<IXmlDataTypeTableMethod> TableMethods { get; }
}
