using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class TableDataType : ITableDataType, IDataType, IMetadataObject, IDatabaseTable, ITabular
{
	private readonly DeserializationModel model;

	private readonly int columns;

	private readonly int constraints;

	private readonly int indexes;

	private readonly string name;

	private readonly int statistics;

	bool IDataType.IsScalar => this is IScalarDataType;

	bool IDataType.IsTable => this != null;

	bool IDataType.IsCursor => this is ICursorDataType;

	bool IDataType.IsUnknown => true;

	string IMetadataObject.Name => name;

	CollationInfo IDatabaseTable.CollationInfo => CollationInfo.Default;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => model.GetCollectionById<IConstraint>(constraints);

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => model.GetCollectionById<IIndex>(indexes);

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => model.GetCollectionById<IStatistics>(statistics);

	TabularType ITabular.TabularType => TabularType.TableDataType;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => model.GetOrderedCollectionById<IColumn>(columns);

	ITabular ITabular.Unaliased => this;

	public TableDataType(DeserializationModel model, int columns, int constraints, int indexes, string name, int statistics)
	{
		this.model = model;
		this.columns = columns;
		this.constraints = constraints;
		this.indexes = indexes;
		this.name = name;
		this.statistics = statistics;
	}

	public TableDataType(DeserializationModel model, object[] values)
		: this(model, (int)values[0], (int)values[1], (int)values[2], (string)values[3], (int)values[4])
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
