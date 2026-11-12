namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Abstract)]
public interface IDataType : IMetadataObject
{
	[IsType(typeof(IScalarDataType))]
	bool IsScalar { get; }

	[IsType(typeof(ITableDataType))]
	bool IsTable { get; }

	[IsType(typeof(ICursorDataType))]
	bool IsCursor { get; }

	[Abstract]
	bool IsUnknown { get; }
}
