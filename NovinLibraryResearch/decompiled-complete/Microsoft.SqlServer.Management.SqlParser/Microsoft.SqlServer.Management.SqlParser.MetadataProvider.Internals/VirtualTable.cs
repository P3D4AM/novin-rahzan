using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class VirtualTable : MetadataObjectBase, IMutableVirtualTable, IVirtualTable, ITabular, IMetadataObject, IMutableTabular, IMutableMetadataObject
{
	private readonly ColumnOrderedCollection m_columns;

	public IMutableMetadataOrderedCollection<IColumn> Columns => m_columns;

	public abstract TabularType TabularType { get; }

	IMetadataOrderedCollection<IColumn> ITabular.Columns => m_columns;

	public ITabular Unaliased => this;

	public virtual ITabular TargetTable => this;

	protected VirtualTable(string name, CollationInfo collationInfo)
		: base(name)
	{
		m_columns = new ColumnOrderedCollection(collationInfo);
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
