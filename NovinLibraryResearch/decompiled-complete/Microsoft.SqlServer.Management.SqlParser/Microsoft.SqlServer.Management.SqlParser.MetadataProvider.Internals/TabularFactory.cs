using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class TabularFactory : ObjectFactoryBase<TabularFactory>, ITabularFactory
{
	public IMutableTabular CreateCommonTableExpression(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new CommonTableExpression(name, collationInfo);
	}

	public IMutableTabular CreateDerivedTable(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DerivedTable(name, collationInfo);
	}

	public ITabular CreateDmlInsertedTable(ITabular targetTable)
	{
		if (targetTable == null)
		{
			throw new ArgumentNullException("targetTable");
		}
		return new DmlInsertedTable(targetTable);
	}

	public ITabular CreateDmlDeletedTable(ITabular targetTable)
	{
		if (targetTable == null)
		{
			throw new ArgumentNullException("targetTable");
		}
		return new DmlDeletedTable(targetTable);
	}

	public IMutableTabular CreateDmlTableSource(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new DmlTableSource(name, collationInfo);
	}

	public IMutableTabular CreatePivotTable(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new PivotTable(name, collationInfo);
	}

	public ITabular CreateTableAlias(ITabular aliasedTable, string alias)
	{
		if (aliasedTable == null)
		{
			throw new ArgumentNullException("aliasedTable");
		}
		if (alias == null)
		{
			throw new ArgumentNullException("alias");
		}
		return new TableAlias(aliasedTable, alias);
	}

	public IMutableTable CreateTable(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Table(schema, name);
	}

	public IMutableTable CreateTable(ITable table)
	{
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		return new Table(table);
	}

	public IMutableTabular CreateUnpivotTable(string name, CollationInfo collationInfo)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new UnpivotTable(name, collationInfo);
	}

	public IMutableView CreateView(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new View(schema, name);
	}

	public IMutableView CreateView(IView view)
	{
		if (view == null)
		{
			throw new ArgumentNullException("view");
		}
		return new View(view);
	}
}
