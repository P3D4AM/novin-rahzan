using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlInsertSpecification : SqlDmlSpecification
{
	private class InsertSpecificationBindingContext : DmlSpecificationBindingContext
	{
		protected override bool HasDeletedTable => false;

		protected override bool HasInsertedTable => true;

		public override bool IsTargetTableAmbiguous => false;

		public override bool IsTargetTableLocal => false;

		public override IMetadataCollection<ITabular> LocalTableExpressions => dmlTargetTables;

		public InsertSpecificationBindingContext(LocalBindingContext baseBindingContext)
			: base(baseBindingContext)
		{
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
			base.targetTable = targetTable;
			if (base.targetTable != null)
			{
				dmlTargetTables = Collection<ITabular>.CreateOrderedCollection(base.DatabaseCollation, base.targetTable);
			}
		}

		protected override IMetadataCollection<ITabular> CreateOutputClauseTableCollection()
		{
			return CreateSpecialTableCollection();
		}
	}

	private readonly SqlColumnRefExpressionCollection targetColumns;

	private readonly SqlInsertSource source;

	internal override bool IsVisitable => true;

	public SqlColumnRefExpressionCollection TargetColumns => targetColumns;

	public SqlInsertSource Source => source;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			if (base.TopSpecification != null)
			{
				yield return base.TopSpecification;
			}
			yield return base.Target;
			if (targetColumns != null)
			{
				foreach (SqlColumnRefExpression targetColumn in targetColumns)
				{
					yield return targetColumn;
				}
			}
			if (base.OutputIntoClause != null)
			{
				yield return base.OutputIntoClause;
			}
			if (base.OutputClause != null)
			{
				yield return base.OutputClause;
			}
			yield return source;
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
		InsertSpecificationBindingContext bindingContext2 = new InsertSpecificationBindingContext(localBindingContext);
		BindDmlSpecification(bindingContext2);
		if (targetColumns == null)
		{
			return;
		}
		foreach (SqlColumnRefExpression targetColumn in targetColumns)
		{
			targetColumn.Bind(bindingContext2);
			base.BindingState |= targetColumn.BindingState;
		}
	}

	internal override CompletionContext FindCompletionContextForCodeObject(SqlCodeObject codeObject)
	{
		CompletionContext result = CompletionContext.None;
		if (targetColumns != null)
		{
			for (int i = 0; i < targetColumns.Count; i++)
			{
				if (codeObject == targetColumns[i])
				{
					ITabular boundTable = base.Target.BoundTable;
					result = ((boundTable == null || !BindUtils.Table.IsTableValuedFunction(boundTable) || base.Target is SqlTableValuedFunctionRefExpression) ? CompletionContext.Columns : CompletionContext.SelectScalar);
					break;
				}
			}
		}
		return result;
	}

	internal SqlInsertSpecification(SqlTopSpecification topSpecification, DmlTarget dmlTarget, List<SqlOutputClause> outputClauses, SqlInsertSource source)
		: base(topSpecification, dmlTarget, outputClauses)
	{
		if (dmlTarget.TargetColumns != null)
		{
			targetColumns = new SqlColumnRefExpressionCollection(dmlTarget.TargetColumns);
		}
		this.source = source;
	}
}
