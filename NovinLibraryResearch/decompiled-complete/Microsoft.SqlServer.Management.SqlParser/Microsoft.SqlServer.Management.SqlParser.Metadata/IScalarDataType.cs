namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[ReadOnly("Name")]
[Value(InheritanceType.NotInherited, "Name")]
[Constant(InheritanceType.NotInherited, "IsUnknown", true)]
public interface IScalarDataType : IDataType, IMetadataObject
{
	[IsType(typeof(ISystemDataType))]
	bool IsSystem { get; }

	[IsType(typeof(IClrDataType))]
	bool IsClr { get; }

	[IsType(typeof(IXmlDataType))]
	bool IsXml { get; }

	[IsType(typeof(IVoidDataType))]
	bool IsVoid { get; }

	[Constant(InheritanceType.NotInherited, null)]
	ISystemDataType BaseSystemDataType { get; }
}
