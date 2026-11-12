using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface ITabularFactory
{
	IMutableTabular CreateCommonTableExpression(string name, CollationInfo collationInfo);

	IMutableTabular CreateDerivedTable(string name, CollationInfo collationInfo);

	ITabular CreateDmlInsertedTable(ITabular targetTable);

	ITabular CreateDmlDeletedTable(ITabular targetTable);

	IMutableTabular CreateDmlTableSource(string name, CollationInfo collationInfo);

	IMutableTabular CreatePivotTable(string name, CollationInfo collationInfo);

	IMutableTable CreateTable(ISchema schema, string name);

	IMutableTable CreateTable(ITable table);

	ITabular CreateTableAlias(ITabular aliasedTable, string alias);

	IMutableTabular CreateUnpivotTable(string name, CollationInfo collationInfo);

	IMutableView CreateView(ISchema schema, string name);

	IMutableView CreateView(IView view);
}
