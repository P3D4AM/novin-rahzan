namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[ReadOnly("Name")]
[Value(InheritanceType.NotInherited, "Name")]
[Constant(InheritanceType.NotInherited, "IsUnknown", true)]
[Constant(InheritanceType.NotInherited, "BaseSystemDataType", null)]
public interface IClrDataType : IScalarDataType, IDataType, IMetadataObject
{
	[Constant(InheritanceType.NotInherited, null)]
	string AssemblyName { get; }

	[Constant(InheritanceType.NotInherited, null)]
	string ClassName { get; }

	[Constant(InheritanceType.NotInherited, false)]
	bool IsBinaryOrdered { get; }

	[Constant(InheritanceType.NotInherited, false)]
	bool IsComVisible { get; }

	[Constant(InheritanceType.NotInherited, false)]
	bool IsNullable { get; }

	[Constant(InheritanceType.NotInherited, null)]
	IMetadataCollection<IUdtMethod> Methods { get; }

	[Constant(InheritanceType.NotInherited, null)]
	IMetadataCollection<IUdtDataMember> DataMembers { get; }
}
