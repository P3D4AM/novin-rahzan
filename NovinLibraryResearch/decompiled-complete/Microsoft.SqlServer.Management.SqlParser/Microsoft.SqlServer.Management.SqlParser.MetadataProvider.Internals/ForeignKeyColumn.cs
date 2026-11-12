using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class ForeignKeyColumn : IForeignKeyColumn, IMetadataObject
{
	private readonly IColumn m_referencingColumn;

	private readonly IColumn m_referencedColumn;

	public IColumn ReferencingColumn => m_referencingColumn;

	public IColumn ReferencedColumn => m_referencedColumn;

	public string Name => m_referencingColumn.Name;

	public ForeignKeyColumn(IColumn referencingColumn, IColumn referencedColumn)
	{
		m_referencingColumn = referencingColumn;
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
