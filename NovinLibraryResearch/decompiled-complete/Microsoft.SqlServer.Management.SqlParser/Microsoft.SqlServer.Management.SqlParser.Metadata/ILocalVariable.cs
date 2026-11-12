namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
[ReadOnly("Name")]
public interface ILocalVariable : IMetadataObject
{
	[IsType(typeof(IScalarVariable))]
	bool IsScalarVariable { get; }

	[IsType(typeof(ITableVariable))]
	bool IsTableVariable { get; }

	[IsType(typeof(ICursorVariable))]
	bool IsCursorVariable { get; }

	[IsType(typeof(IParameter))]
	bool IsParameter { get; }

	[Abstract]
	IDataType DataType { get; }
}
