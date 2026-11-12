using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class IndexedColumn : IIndexedColumn, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool isIncluded;

	private readonly int referencedColumn;

	private readonly SortOrder sortOrder;

	IColumn IIndexedColumn.ReferencedColumn => model.GetObjectById<IColumn>(referencedColumn);

	SortOrder IIndexedColumn.SortOrder => sortOrder;

	bool IIndexedColumn.IsIncluded => isIncluded;

	string IMetadataObject.Name => ((IIndexedColumn)this).ReferencedColumn.Name;

	public IndexedColumn(DeserializationModel model, bool isIncluded, int referencedColumn, SortOrder sortOrder)
	{
		this.model = model;
		this.isIncluded = isIncluded;
		this.referencedColumn = referencedColumn;
		this.sortOrder = sortOrder;
	}

	public IndexedColumn(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (int)values[1], (SortOrder)values[2])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
