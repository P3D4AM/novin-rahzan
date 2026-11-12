using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class TableAlias : MetadataObjectBase, IVirtualTable, ITabular, IMetadataObject
{
	private readonly ITabular m_aliasedTable;

	public TabularType TabularType => TabularType.TableAlias;

	public IMetadataOrderedCollection<IColumn> Columns => m_aliasedTable.Columns;

	public ITabular Unaliased => m_aliasedTable;

	public ITabular TargetTable => m_aliasedTable;

	public TableAlias(ITabular aliasedTable, string alias)
		: base(alias)
	{
		m_aliasedTable = aliasedTable;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
