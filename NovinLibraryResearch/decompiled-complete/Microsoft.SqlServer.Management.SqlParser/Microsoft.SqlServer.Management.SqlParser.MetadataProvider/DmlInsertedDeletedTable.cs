using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal abstract class DmlInsertedDeletedTable : MetadataObjectBase, IVirtualTable, ITabular, IMetadataObject
{
	private readonly ITabular m_targetTable;

	public TabularType TabularType => TabularType.DmlInsertedDeleted;

	public IMetadataOrderedCollection<IColumn> Columns => m_targetTable.Columns;

	public ITabular Unaliased => this;

	public ITabular TargetTable => m_targetTable;

	protected DmlInsertedDeletedTable(string name, ITabular targetTable)
		: base(name)
	{
		m_targetTable = targetTable;
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
