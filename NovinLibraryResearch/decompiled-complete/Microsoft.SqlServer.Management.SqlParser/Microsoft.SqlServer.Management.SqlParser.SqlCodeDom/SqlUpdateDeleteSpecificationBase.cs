using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlUpdateDeleteSpecificationBase : SqlDmlSpecification
{
	internal abstract class UpdateDeleteSpecificationBindingContext : DmlSpecificationBindingContext
	{
		private enum TargetTableExpressionType
		{
			Table,
			Variable,
			TableValuedFunction
		}

		private ITabular localTargetTable;

		private bool isTargetTableAmbiguous;

		public override bool IsTargetTableAmbiguous => isTargetTableAmbiguous;

		public override bool IsTargetTableLocal => localTargetTable != null;

		public UpdateDeleteSpecificationBindingContext(LocalBindingContext baseBindingContext)
			: base(baseBindingContext)
		{
		}

		private bool IsNotLocalTargetTable(ITabular table)
		{
			return table != localTargetTable;
		}

		public override ITabular SetTableTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
		{
			return SetTarget(databaseName, schemaName, name, lexicalScope, TargetTableExpressionType.Table);
		}

		public override ITableValuedFunction SetTableValuedFunctionTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
		{
			return SetTarget(databaseName, schemaName, name, lexicalScope, TargetTableExpressionType.TableValuedFunction) as ITableValuedFunction;
		}

		public override ITableVariable SetTableVariableTarget(string name, LexicalScope lexicalScope)
		{
			return SetTarget(null, null, name, lexicalScope, TargetTableExpressionType.Variable) as ITableVariable;
		}

		private ITabular SetTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope, TargetTableExpressionType expressionType)
		{
			ITabular tabular = null;
			ITabular tabular2 = null;
			bool flag = false;
			tabular = FindLocalTableExpression(databaseName, schemaName, name);
			if (tabular == null)
			{
				switch (expressionType)
				{
				case TargetTableExpressionType.Table:
					tabular = base.Batch.FindBatchTableExpression(databaseName, schemaName, name, lexicalScope);
					break;
				case TargetTableExpressionType.TableValuedFunction:
					tabular = base.Batch.FindTableValuedFunction(databaseName, schemaName, name, lexicalScope);
					break;
				case TargetTableExpressionType.Variable:
					tabular = base.Batch.FindLocalTableVariable(name, lexicalScope);
					break;
				}
				if (tabular != null)
				{
					foreach (ITabular localTableExpression in LocalTableExpressions)
					{
						if (localTableExpression.Unaliased == tabular)
						{
							if (tabular2 != null)
							{
								flag = true;
								break;
							}
							tabular2 = localTableExpression;
						}
					}
				}
			}
			else
			{
				tabular2 = tabular;
			}
			if (tabular != null)
			{
				targetTable = tabular;
				localTargetTable = tabular2;
				isTargetTableAmbiguous = flag;
				dmlTargetTables = Collection<ITabular>.CreateOrderedCollection(base.DatabaseCollation, targetTable);
				if (tabular2 == null)
				{
					setClauseTables = Collection<ITabular>.Merge(dmlTargetTables, setClauseTables);
				}
			}
			return tabular;
		}

		protected override IMetadataCollection<ITabular> CreateOutputClauseTableCollection()
		{
			if (targetTable != null)
			{
				IMetadataCollection<ITabular> metadataCollection = CreateSpecialTableCollection();
				if (localTargetTable != null)
				{
					if (dmlSourceTables.Count == 1)
					{
						return metadataCollection;
					}
					return Collection<ITabular>.Merge(Collection<ITabular>.Filter(dmlSourceTables, IsNotLocalTargetTable), metadataCollection);
				}
				return Collection<ITabular>.Merge(metadataCollection, dmlSourceTables);
			}
			return dmlSourceTables;
		}
	}

	private readonly SqlFromClause fromClause;

	private readonly SqlWhereClause whereClause;

	public SqlFromClause FromClause => fromClause;

	public SqlWhereClause WhereClause => whereClause;

	internal abstract DmlSpecificationBindingContext CreateBindingContext(LocalBindingContext localBindingContext);

	internal override void Bind(IBindingContext bindingContext)
	{
		LocalBindingContext localBindingContext = bindingContext.CreateLocalBindingContext();
		if (fromClause != null)
		{
			fromClause.Bind(localBindingContext);
			base.BindingState |= fromClause.BindingState;
		}
		DmlSpecificationBindingContext bindingContext2 = CreateBindingContext(localBindingContext);
		BindDmlSpecification(bindingContext2);
		if (whereClause != null)
		{
			whereClause.Bind(bindingContext2);
			base.BindingState |= whereClause.BindingState;
		}
	}

	internal SqlUpdateDeleteSpecificationBase(SqlTopSpecification topSpecification, DmlTarget dmlTarget, List<SqlOutputClause> outputClauses, SqlFromClause fromClause, SqlWhereClause whereClause)
		: base(topSpecification, dmlTarget, outputClauses)
	{
		this.fromClause = fromClause;
		this.whereClause = whereClause;
	}
}
