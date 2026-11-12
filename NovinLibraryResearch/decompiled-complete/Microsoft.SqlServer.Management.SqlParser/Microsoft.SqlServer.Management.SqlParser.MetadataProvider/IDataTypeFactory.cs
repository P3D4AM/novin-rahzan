using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IDataTypeFactory
{
	ICursorDataType Cursor { get; }

	IClrDataType UnknownClr { get; }

	IScalarDataType UnknownScalar { get; }

	ITableDataType UnknownTable { get; }

	IScalarDataType Void { get; }

	IXmlDataType XmlNode { get; }

	IMutableTableDataType CreateTableDataType(string name, CollationInfo collationInfo);
}
