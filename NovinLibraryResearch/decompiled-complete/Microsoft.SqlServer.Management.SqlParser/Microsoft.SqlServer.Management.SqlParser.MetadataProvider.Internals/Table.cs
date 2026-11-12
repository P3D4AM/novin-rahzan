using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class Table : TableViewBase, IMutableTable, ITable, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IMutableTableViewBase, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	private readonly TabularType m_tabularType;

	public override TabularType TabularType => m_tabularType;

	public Table(ISchema schema, string name)
		: base(schema, name)
	{
		bool flag = name.StartsWith("#", StringComparison.OrdinalIgnoreCase);
		m_tabularType = (flag ? TabularType.TemporaryTable : TabularType.Table);
	}

	public Table(ITable table)
		: base(table)
	{
		m_tabularType = table.TabularType;
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
