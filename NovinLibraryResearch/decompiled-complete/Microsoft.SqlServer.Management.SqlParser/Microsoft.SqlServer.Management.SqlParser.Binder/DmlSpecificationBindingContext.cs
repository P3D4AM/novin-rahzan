using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

internal abstract class DmlSpecificationBindingContext : LocalBindingContext
{
	private class OutputBindingContext : LocalBindingContext
	{
		private readonly IMetadataCollection<ITabular> outputClauseTables;

		public override IMetadataCollection<ITabular> LocalTableExpressions => outputClauseTables;

		public OutputBindingContext(DmlSpecificationBindingContext baseBindingContext)
			: base(baseBindingContext, null, new ColumnOrderedCollection(baseBindingContext.DatabaseCollation))
		{
			outputClauseTables = baseBindingContext.CreateOutputClauseTableCollection();
		}

		public override IColumn FindColumn(string name, out bool isAmbiguous)
		{
			isAmbiguous = false;
			return null;
		}

		public override ITabular AddLocalTableExpression(ITabular expression)
		{
			return null;
		}
	}

	protected ITabular targetTable;

	protected IMetadataCollection<ITabular> dmlTargetTables;

	protected IMetadataCollection<ITabular> dmlSourceTables;

	protected IMetadataCollection<ITabular> setClauseTables;

	private TemporaryBindMode bindMode;

	protected abstract bool HasInsertedTable { get; }

	protected abstract bool HasDeletedTable { get; }

	public abstract bool IsTargetTableAmbiguous { get; }

	public abstract bool IsTargetTableLocal { get; }

	public override IMetadataCollection<ITabular> LocalTableExpressions
	{
		get
		{
			switch (bindMode)
			{
			case TemporaryBindMode.ColumnAssignmentTarget:
			case TemporaryBindMode.MergeActionTarget:
				return dmlTargetTables;
			case TemporaryBindMode.MergeActionSource:
				return dmlSourceTables;
			default:
				return setClauseTables;
			}
		}
	}

	protected DmlSpecificationBindingContext(LocalBindingContext baseBindingContext)
		: base(baseBindingContext)
	{
		dmlTargetTables = Collection<ITabular>.Empty;
		dmlSourceTables = baseBindingContext.LocalTableExpressions;
		if (dmlSourceTables.Count == 0)
		{
			dmlSourceTables = Collection<ITabular>.Empty;
		}
		setClauseTables = dmlSourceTables;
	}

	public LocalBindingContext CreateOutputClauseBindingContext()
	{
		return new OutputBindingContext(this);
	}

	protected IMetadataCollection<ITabular> CreateSpecialTableCollection()
	{
		if (targetTable != null)
		{
			ITabularFactory tabular = base.MetadataFactory.Tabular;
			CollationInfo ordinalIgnoreCase = CollationInfo.OrdinalIgnoreCase;
			ITabular unaliased = targetTable.Unaliased;
			if (HasDeletedTable)
			{
				if (HasInsertedTable)
				{
					return Collection<ITabular>.CreateOrderedCollection(ordinalIgnoreCase, tabular.CreateDmlInsertedTable(unaliased), tabular.CreateDmlDeletedTable(unaliased));
				}
				return Collection<ITabular>.CreateOrderedCollection(ordinalIgnoreCase, tabular.CreateDmlDeletedTable(unaliased));
			}
			return Collection<ITabular>.CreateOrderedCollection(ordinalIgnoreCase, tabular.CreateDmlInsertedTable(unaliased));
		}
		return Collection<ITabular>.Empty;
	}

	public abstract ITabular SetTableTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope);

	public abstract ITableValuedFunction SetTableValuedFunctionTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope);

	public abstract ITableVariable SetTableVariableTarget(string name, LexicalScope lexicalScope);

	protected abstract IMetadataCollection<ITabular> CreateOutputClauseTableCollection();

	public override ITabular AddLocalTableExpression(ITabular expression)
	{
		return null;
	}

	public override void EnterTempBindMode(TemporaryBindMode mode)
	{
		if ((uint)(mode - 1) <= 2u)
		{
			bindMode = mode;
		}
	}

	public override void LeaveTempBindMode(TemporaryBindMode mode)
	{
		if ((uint)(mode - 1) <= 2u)
		{
			bindMode = TemporaryBindMode.None;
		}
	}
}
