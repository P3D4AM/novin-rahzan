using System;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMergeSpecification : SqlDmlSpecification
{
	private class MergeSpecificationBindingContext : DmlSpecificationBindingContext
	{
		private readonly string targetTableAlias;

		protected override bool HasDeletedTable => true;

		protected override bool HasInsertedTable => true;

		public override bool IsTargetTableAmbiguous => false;

		public override bool IsTargetTableLocal => false;

		public MergeSpecificationBindingContext(LocalBindingContext baseBindingContext, string targetAlias)
			: base(baseBindingContext)
		{
			targetTableAlias = targetAlias;
		}

		public override ITabular SetTableTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
		{
			ITabular result = base.Batch.FindBatchTableExpression(databaseName, schemaName, name, lexicalScope);
			SetTargetTable(result);
			return result;
		}

		public override ITableValuedFunction SetTableValuedFunctionTarget(string databaseName, string schemaName, string name, LexicalScope lexicalScope)
		{
			ITableValuedFunction result = base.Batch.FindTableValuedFunction(databaseName, schemaName, name, lexicalScope);
			SetTargetTable(result);
			return result;
		}

		public override ITableVariable SetTableVariableTarget(string name, LexicalScope lexicalScope)
		{
			ITableVariable result = base.Batch.FindLocalTableVariable(name, lexicalScope);
			SetTargetTable(result);
			return result;
		}

		private void SetTargetTable(ITabular targetTable)
		{
			if (targetTable != null)
			{
				if (!string.IsNullOrEmpty(targetTableAlias))
				{
					base.targetTable = base.MetadataFactory.Tabular.CreateTableAlias(targetTable, targetTableAlias);
				}
				else
				{
					base.targetTable = targetTable;
				}
				dmlTargetTables = Collection<ITabular>.CreateOrderedCollection(base.DatabaseCollation, base.targetTable);
				setClauseTables = Collection<ITabular>.Merge(dmlTargetTables, setClauseTables);
			}
		}

		protected override IMetadataCollection<ITabular> CreateOutputClauseTableCollection()
		{
			return Collection<ITabular>.Merge(CreateSpecialTableCollection(), dmlSourceTables);
		}
	}

	private readonly SqlIdentifier alias;

	private readonly SqlTableExpression source;

	private readonly SqlConditionClause onClause;

	private readonly SqlMergeActionClauseCollection actionClauses;

	internal override bool IsVisitable => true;

	public SqlIdentifier Alias => alias;

	public SqlTableExpression Source => source;

	public SqlConditionClause OnClause => onClause;

	public SqlMergeActionClauseCollection ActionClauses => actionClauses;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.TopSpecification != null)
			{
				yield return base.TopSpecification;
			}
			yield return base.Target;
			if (alias != null)
			{
				yield return alias;
			}
			yield return source;
			yield return onClause;
			foreach (SqlMergeActionClause actionClause in actionClauses)
			{
				yield return actionClause;
			}
			if (base.OutputClause != null)
			{
				yield return base.OutputClause;
			}
			if (base.OutputIntoClause != null)
			{
				yield return base.OutputIntoClause;
			}
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		LocalBindingContext localBindingContext = bindingContext.CreateLocalBindingContext();
		source.Bind(localBindingContext);
		base.BindingState |= source.BindingState;
		string targetAlias = ((alias != null) ? alias.Value : null);
		MergeSpecificationBindingContext bindingContext2 = new MergeSpecificationBindingContext(localBindingContext, targetAlias);
		BindDmlSpecification(bindingContext2);
		onClause.Bind(bindingContext2);
		foreach (SqlMergeActionClause actionClause in actionClauses)
		{
			actionClause.Bind(bindingContext2);
		}
	}

	internal override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		return null;
	}

	internal SqlMergeSpecification(SqlTopSpecification topSpecification, DmlTarget dmlTarget, SqlIdentifier alias, SqlTableExpression source, SqlConditionClause onClause, List<SqlMergeActionClause> mergeActionClauses, List<SqlOutputClause> outputClauses)
		: base(topSpecification, dmlTarget, outputClauses)
	{
		this.alias = alias;
		this.source = source;
		this.onClause = onClause;
		actionClauses = new SqlMergeActionClauseCollection(mergeActionClauses);
	}
}
