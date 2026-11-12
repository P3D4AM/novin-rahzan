namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public enum SqlDataCompressionType
{
	None,
	Row,
	Page,
	ColumnStore,
	ColumnStoreArchive,
	Invalid
}
