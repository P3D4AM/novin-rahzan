namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
public interface ICursorVariable : ILocalVariable, IMetadataObject
{
	[Constant(SpecialValue.CursorDataType)]
	new ICursorDataType DataType { get; }
}
