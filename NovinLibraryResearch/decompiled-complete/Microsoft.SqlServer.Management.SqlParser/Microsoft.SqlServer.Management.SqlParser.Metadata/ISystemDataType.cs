namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[Duplicate("Name", "TypeSpec.Name")]
[Constant(InheritanceType.NotInherited, "BaseSystemDataType", SpecialValue.This)]
[Constant(InheritanceType.NotInherited, "IsUnknown", false)]
public interface ISystemDataType : IScalarDataType, IDataType, IMetadataObject
{
	[ReadOnly]
	DataTypeSpec TypeSpec { get; }

	[ReadOnly]
	int Length { get; }

	[ReadOnly]
	int NumericPrecision { get; }

	[ReadOnly]
	int NumericScale { get; }
}
