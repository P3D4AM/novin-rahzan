using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class TableDataType : MetadataObjectBase, IMutableTableDataType, ITableDataType, IDataType, IMetadataObject, IDatabaseTable, ITabular, IMutableDataType, IMutableMetadataObject, IMutableDatabaseTable, IMutableTabular
{
	private readonly CollationInfo m_collationInfo;

	private readonly ColumnOrderedCollection m_columns;

	private readonly ConstraintCollection m_constraints;

	private readonly IndexCollection m_indexes;

	private readonly StatisticsCollection m_statistics;

	private bool m_isUnknown;

	public bool IsScalar => false;

	public bool IsTable => true;

	public bool IsCursor => false;

	public bool IsUnknown
	{
		get
		{
			return m_isUnknown;
		}
		set
		{
			m_isUnknown = value;
		}
	}

	public IScalarDataType AsScalarDataType => null;

	public ITableDataType AsTableDataType => this;

	public IUserDefinedType AsUserDefinedType => null;

	public CollationInfo CollationInfo => m_collationInfo;

	public IMetadataCollection<IConstraint> Constraints => m_constraints;

	public IMetadataCollection<IIndex> Indexes => m_indexes;

	public IMetadataCollection<IStatistics> Statistics => m_statistics;

	public TabularType TabularType => TabularType.TableDataType;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => m_columns;

	public ITabular Unaliased => this;

	IMutableMetadataCollection<IConstraint> IMutableDatabaseTable.Constraints => m_constraints;

	IMutableMetadataCollection<IIndex> IMutableDatabaseTable.Indexes => m_indexes;

	IMutableMetadataCollection<IStatistics> IMutableDatabaseTable.Statistics => m_statistics;

	public IMutableMetadataOrderedCollection<IColumn> Columns => m_columns;

	public TableDataType(string name, CollationInfo collationInfo)
		: base(name)
	{
		m_collationInfo = collationInfo;
		m_columns = new ColumnOrderedCollection(collationInfo);
		m_constraints = new ConstraintCollection(collationInfo);
		m_indexes = new IndexCollection(collationInfo);
		m_statistics = new StatisticsCollection(collationInfo);
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
