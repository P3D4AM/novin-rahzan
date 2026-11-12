using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class IndexedColumn : IMutableIndexedColumn, IIndexedColumn, IMetadataObject, IMutableMetadataObject
{
	private readonly IColumn m_referencedColumn;

	private SortOrder m_sortOrder;

	private bool m_isIncluded;

	public SortOrder SortOrder
	{
		get
		{
			return m_sortOrder;
		}
		set
		{
			m_sortOrder = value;
		}
	}

	public bool IsIncluded
	{
		get
		{
			return m_isIncluded;
		}
		set
		{
			m_isIncluded = value;
		}
	}

	public IColumn ReferencedColumn => m_referencedColumn;

	public string Name => m_referencedColumn.Name;

	public IndexedColumn(IColumn referencedColumn)
	{
		m_referencedColumn = referencedColumn;
	}

	public T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
