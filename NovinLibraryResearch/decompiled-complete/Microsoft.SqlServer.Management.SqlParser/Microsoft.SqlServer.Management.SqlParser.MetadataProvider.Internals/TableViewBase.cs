using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal abstract class TableViewBase : SchemaOwnedObject, IMutableTableViewBase, ITableViewBase, IDatabaseTable, ITabular, IMetadataObject, ISchemaOwnedObject, IDatabaseObject, IMutableDatabaseTable, IMutableTabular, IMutableMetadataObject, IMutableSchemaOwnedObject, IMutableDatabaseObject
{
	private readonly ColumnOrderedCollection m_columns;

	private readonly ConstraintCollection m_constraints;

	private readonly IndexCollection m_indexes;

	private readonly StatisticsCollection m_statistics;

	private readonly DmlTriggerCollection m_triggers;

	private bool m_isQuotedIdentifierOn;

	public IMutableMetadataCollection<IDmlTrigger> Triggers => m_triggers;

	public bool IsQuotedIdentifierOn
	{
		get
		{
			return m_isQuotedIdentifierOn;
		}
		set
		{
			m_isQuotedIdentifierOn = true;
		}
	}

	public IMutableMetadataCollection<IConstraint> Constraints => m_constraints;

	public IMutableMetadataCollection<IIndex> Indexes => m_indexes;

	public IMutableMetadataCollection<IStatistics> Statistics => m_statistics;

	public IMutableMetadataOrderedCollection<IColumn> Columns => m_columns;

	IMetadataCollection<IDmlTrigger> ITableViewBase.Triggers => m_triggers;

	IMetadataCollection<IConstraint> IDatabaseTable.Constraints => m_constraints;

	IMetadataCollection<IIndex> IDatabaseTable.Indexes => m_indexes;

	IMetadataCollection<IStatistics> IDatabaseTable.Statistics => m_statistics;

	public abstract TabularType TabularType { get; }

	IMetadataOrderedCollection<IColumn> ITabular.Columns => m_columns;

	public ITabular Unaliased => this;

	protected TableViewBase(ISchema schema, string name)
		: base(schema, name)
	{
		CollationInfo collationInfo = schema.Database.CollationInfo;
		m_columns = new ColumnOrderedCollection(collationInfo);
		m_constraints = new ConstraintCollection(collationInfo);
		m_indexes = new IndexCollection(collationInfo);
		m_statistics = new StatisticsCollection(collationInfo);
		m_triggers = new DmlTriggerCollection(collationInfo);
		m_isQuotedIdentifierOn = true;
	}

	protected TableViewBase(ITableViewBase tableViewBase)
		: base(tableViewBase)
	{
		CollationInfo collationInfo = base.Schema.Database.CollationInfo;
		m_columns = new ColumnOrderedCollection(tableViewBase.Columns.Count, collationInfo);
		m_constraints = new ConstraintCollection(tableViewBase.Constraints.Count, collationInfo);
		m_indexes = new IndexCollection(tableViewBase.Indexes.Count, collationInfo);
		m_statistics = new StatisticsCollection(tableViewBase.Indexes.Count, collationInfo);
		m_triggers = new DmlTriggerCollection(tableViewBase.Triggers.Count, collationInfo);
		m_columns.AddRange(tableViewBase.Columns);
		m_constraints.AddRange(tableViewBase.Constraints);
		m_indexes.AddRange(tableViewBase.Indexes);
		m_statistics.AddRange(tableViewBase.Statistics);
		m_triggers.AddRange(tableViewBase.Triggers);
		m_isQuotedIdentifierOn = tableViewBase.IsQuotedIdentifierOn;
	}
}
