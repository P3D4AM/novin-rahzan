using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class UserDefinedTableType : SchemaOwnedObject, IMutableUserDefinedTableType, IUserDefinedTableType, IUserDefinedType, IDataType, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, ITableDataType, IDatabaseTable, ITabular, IMutableUserDefinedType, IMutableDataType, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject, IMutableTableDataType, IMutableDatabaseTable, IMutableTabular
{
	private readonly ColumnOrderedCollection m_columns;

	private readonly ConstraintCollection m_constraints;

	private readonly IndexCollection m_indexes;

	private readonly StatisticsCollection m_statistics;

	private bool m_isUnknown;

	public IMetadataCollection<IConstraint> Constraints => m_constraints;

	public IMetadataCollection<IIndex> Indexes => m_indexes;

	public IMetadataCollection<IStatistics> Statistics => m_statistics;

	public TabularType TabularType => TabularType.TableDataType;

	IMetadataOrderedCollection<IColumn> ITabular.Columns => m_columns;

	public ITabular Unaliased => this;

	public IMutableMetadataOrderedCollection<IColumn> Columns => m_columns;

	IMutableMetadataCollection<IConstraint> IMutableDatabaseTable.Constraints => m_constraints;

	IMutableMetadataCollection<IIndex> IMutableDatabaseTable.Indexes => m_indexes;

	IMutableMetadataCollection<IStatistics> IMutableDatabaseTable.Statistics => m_statistics;

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

	public bool IsScalar => false;

	public bool IsTable => true;

	public bool IsCursor => false;

	public UserDefinedTableType(ISchema schema, string name)
		: base(schema, name)
	{
		CollationInfo collationInfo = schema.Database.CollationInfo;
		m_columns = new ColumnOrderedCollection(collationInfo);
		m_constraints = new ConstraintCollection(collationInfo);
		m_indexes = new IndexCollection(collationInfo);
		m_statistics = new StatisticsCollection(collationInfo);
	}

	public override T Accept<T>(ISchemaOwnedObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
