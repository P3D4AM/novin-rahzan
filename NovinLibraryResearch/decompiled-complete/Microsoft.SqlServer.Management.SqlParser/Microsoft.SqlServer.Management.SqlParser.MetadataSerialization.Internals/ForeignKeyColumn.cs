using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class ForeignKeyColumn : IForeignKeyColumn, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly int referencedColumn;

	private readonly int referencingColumn;

	IColumn IForeignKeyColumn.ReferencingColumn => model.GetObjectById<IColumn>(referencingColumn);

	IColumn IForeignKeyColumn.ReferencedColumn => model.GetObjectById<IColumn>(referencedColumn);

	string IMetadataObject.Name => ((IForeignKeyColumn)this).ReferencingColumn.Name;

	public ForeignKeyColumn(DeserializationModel model, int referencedColumn, int referencingColumn)
	{
		this.model = model;
		this.referencedColumn = referencedColumn;
		this.referencingColumn = referencingColumn;
	}

	public ForeignKeyColumn(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1])
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
